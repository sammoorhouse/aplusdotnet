﻿using System;
using System.Collections.Generic;
using System.Linq;

using AplusCore.Runtime;
using AplusCore.Runtime.Callback;
using AplusCore.Runtime.Function.Dyadic;
using AplusCore.Runtime.Function.Dyadic.NonScalar.Other;
using AplusCore.Runtime.Function.Monadic;
using AplusCore.Runtime.Function.Monadic.NonScalar.Other;
using AplusCore.Types;

using DLR = System.Linq.Expressions;
using DYN = System.Dynamic;

namespace AplusCore.Compiler.AST
{
    /// <summary>
    /// Represents an Assignment in an A+ AST.
    /// </summary>
    public class Assign : Node
    {
        #region Variables

        private Node target;
        private Node expression;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the target of the assignment.
        /// </summary>
        public Node Target
        {
            get { return this.target; }
        }

        /// <summary>
        /// Gets the value of the assignment.
        /// </summary>
        public Node Expression
        {
            get { return this.expression; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="Assign"/> AST node.
        /// </summary>
        /// <param name="target">The target of the assignment.</param>
        /// <param name="expression">The value of the assignment.</param>
        public Assign(Node target, Node expression)
        {
            this.target = target;
            this.expression = expression;
        }

        #endregion

        #region DLR Generator

        public override DLR.Expression Generate(AplusScope scope)
        {
            DLR.ParameterExpression temp = DLR.Expression.Parameter(typeof(AType), "__TEMP__");

            // Clone the rhs value of the assignment to ensure correct results
            // in case of: a:=b:=[...:=] [rhs]  assignments
            DLR.ParameterExpression environment = scope.GetRuntimeExpression();
            DLR.Expression value =
                DLR.Expression.Block(
                    new DLR.ParameterExpression[] { temp },
                    DLR.Expression.Assign(temp, this.expression.Generate(scope)),
                    DLR.Expression.Condition(
                        DLR.Expression.Property(temp, "IsMemoryMappedFile"),
                        temp,
                        DLR.Expression.Call(
                            temp,
                            typeof(AType).GetMethod("Clone")
                        )
                    )
                );

            DLR.Expression result = null;

            if (this.target is Identifier)
            {
                result = GenerateIdentifierAssign(scope, (AST.Identifier)this.target, value);
            }
            else if (this.target is Strand)
            {
                result = GenerateStrandAssign(scope, (Strand)this.target, value);
            }
            else if (this.target is Indexing)
            {
                Indexing target = (Indexing)this.target;

                // in case of:  a[,] := ...
                if (target.IndexExpression != null && target.IndexExpression[0] is BuiltInFunction)
                {
                    BuiltInFunction function = (BuiltInFunction)target.IndexExpression[0];
                    if (function.Function.Type != Grammar.Tokens.RAVEL)
                    {
                        // incorrect function inside the index expression
                        throw new ParseException("Incorrect function", false);
                    }

                    var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static;
                    result = DLR.Expression.Call(
                        typeof(Assign).GetMethod("AppendItem", flags),
                        value,
                        target.Item.Generate(scope),
                        environment
                    );
                }
                else
                {
                    result = GenerateIndexAssign(scope, target, value);
                }
            }
            else if (Node.TestMonadicToken(this.target, Grammar.Tokens.VALUE))
            {
                var method = typeof(Value).GetMethod("Assign");
                var targetDLR = ((MonadicFunction)this.target).Expression.Generate(scope);

                result = DLR.Expression.Call(
                    method,
                    targetDLR,
                    value,
                    environment
                );
            }
            else if (Node.TestDyadicToken(this.target, Grammar.Tokens.VALUEINCONTEXT))
            {
                var targetNode = (DyadicFunction)this.target;
                var method = typeof(ValueInContext).GetMethod("Assign");
                var targetDLR = targetNode.Right.Generate(scope);
                var contextNameDLR = targetNode.Left.Generate(scope);

                result = DLR.Expression.Call(
                    method,
                    targetDLR,
                    contextNameDLR,
                    value,
                    environment
                );
            }
            else if (Node.TestDyadicToken(this.target, Grammar.Tokens.PICK))
            {
                result = BuildPickAssign(scope, value);
            }
            else
            {
                var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static;
                // switch on Assignment flag
                scope.IsAssignment = true;

                // Find the correct method for assignment
                // for pick we need a different method currently..
                var method = typeof(Assign).GetMethod("AssignHelper", flags);

                result = DLR.Expression.Call(
                    method,
                    value,
                    this.target.Generate(scope)
                );

                // switch off Assignment flag
                scope.IsAssignment = false;
            }

            return result;
        }

        #endregion

        #region DLR assign

        private DLR.Expression BuildPickAssign(AplusScope scope, DLR.Expression value)
        {
            DyadicFunction function = (DyadicFunction)this.target;
            bool isValueinContext = Node.TestDyadicToken(function.Left, Grammar.Tokens.VALUEINCONTEXT);
            bool isValue = Node.TestMonadicToken(function.Right, Grammar.Tokens.VALUE);

            if (!(function.Right is Identifier || isValueinContext || isValue))
            {
                throw new ParseException("assign pick target", false);
            }

            DLR.Expression left = function.Left.Generate(scope);
            DLR.Expression right = function.Right.Generate(scope);

            var method = DyadicFunctionInstance.Pick;
            DLR.Expression pickDLR = DLR.Expression.Call(
                DLR.Expression.Constant(method),
                method.GetType().GetMethod("AssignExecute"),
                right,
                left,
                scope.GetRuntimeExpression()
            );

            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static;

            DLR.Expression result = DLR.Expression.Call(
                typeof(Assign).GetMethod("PickAssignHelper", flags),
                value,
                pickDLR
            );

            return result;
        }

        internal static AType PickAssignHelper(AType value, PickAssignmentTarget pickResult)
        {
            if (pickResult.FromBox)
            {
                pickResult.Target.Data = value.Data;
            }
            else
            {
                Utils.PerformAssign(pickResult.Target, value);
            }
            return value;
        }

        internal static AType AssignHelper(AType value, AType target)
        {
            //TODO
            Utils.PerformAssign(target, value);
            return value;
        }

        #endregion

        #region DLR: append

        private static AType AppendItem(AType value, AType target, Aplus environment)
        {
            if (target.Rank == 0)
            {
                throw new Error.Rank("assign");
            }

            if (!Utils.DifferentNumberType(target, value) && target.Type != value.Type)
            {
                // The target and value are not numbers and they are of a different type?
                throw new Error.Type("assign");
            }

            if (target.Shape.SequenceEqual<int>(value.Shape))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    target.Add(value[i].Clone());
                }
            }
            else if (target.Shape.GetRange(1, target.Shape.Count - 1).SequenceEqual<int>(value.Shape))
            {
                target.Add(value.Clone());
            }
            else if (value.Rank == 0)
            {
                AType item = DyadicFunctionInstance.Reshape.Execute(
                    value,
                    target.Shape.GetRange(1, target.Shape.Count - 1).ToAArray()
                );
                target.Add(item);
            }
            else if (value.Rank == target.Rank)
            {
                target.AddRange(value);
            }
            else
            {
                throw new Error.Length("assign");
            }

            return value;
        }

        #endregion

        #region DLR: Assing to indexed target

        private static DLR.Expression GenerateIndexAssign(AplusScope scope, Indexing target, DLR.Expression value)
        {
            DLR.Expression result;

            if (target.IndexExpression == null)
            {
                result = DLR.Expression.Dynamic(
                    scope.GetRuntime().SetIndexBinder(new DYN.CallInfo(0)),
                    typeof(object),
                    target.Item.Generate(scope),
                    DLR.Expression.Constant(null),
                    value
                );
            }
            else
            {
                result = DLR.Expression.Dynamic(
                    scope.GetRuntime().SetIndexBinder(new System.Dynamic.CallInfo(target.IndexExpression.Length)),
                    typeof(object),
                    target.Item.Generate(scope),
                    DLR.Expression.Call(
                        typeof(Helpers).GetMethod("BuildIndexerArray"),
                        DLR.Expression.NewArrayInit(
                            typeof(AType),
                            target.IndexExpression.Items.Reverse().Select(item => { return item.Generate(scope); })
                        )
                    ),
                    value
                );
            }
            return DLR.Expression.Convert(result, typeof(AType));
        }

        #endregion

        #region DLR: Assign to Strand

        /// <summary>Generates a DLR Expression for Strand assignment.</summary>
        /// <remarks>
        ///  Transform this:
        /// (a;b;...) := .....    // don't care what is there :)
        /// 
        /// to this in DLR:
        /// 
        /// $__VALUES__ = (AType) .....; // ..... is a generated DLR expression now!
        /// 
        /// if($__VALUES__.IsArray)     // case A
        /// {
        ///     if($VALUES.length != 2)  // 2 the # of targets
        ///     {
        ///         throw new Error.Length("assign");
        ///     }
        ///     
        ///     a = disclose( ((AArray)$__VALUES__)[0] );
        ///     b = disclose( ((AArray)$__VALUES__)[1] );
        ///     ...
        /// }
        /// else    // case B
        /// {
        ///     // need to disclose the box
        ///     if($__VALUES__.Type == ATypes.Box)
        ///     {
        ///         $__VALUES__ = disclose($__VALUES__);
        ///     }
        ///     a = $VALUES.Clone();
        ///     b = $VALUES.Clone();
        ///     ...
        /// }
        /// $__DependencyManager__.InvalidateDependencies(string[] { a,b, .. })
        /// $__DependencyManager__.ValidateDependencies(string[] { a,b, .. })
        /// </remarks>
        /// <param name="scope"></param>
        /// <param name="targets">
        /// Strand containing Identifiers.
        /// If a node which in not an Identifier found, ParseException is thrown
        /// </param>
        /// <param name="value">The generated value which will be assigned to the targets</param>
        /// <returns>A generated DLR expression for strand assignment</returns>
        private static DLR.Expression GenerateStrandAssign(AplusScope scope, Strand targets, DLR.Expression value)
        {
            AbstractMonadicFunction disclose = MonadicFunctionInstance.Disclose;
            Aplus runtime = scope.GetRuntime();
            DLR.ParameterExpression environment = scope.GetRuntimeExpression();

            DLR.ParameterExpression valuesParam = DLR.Expression.Parameter(typeof(AType), "__VALUES__");
            // for dependency evaluation
            List<string> targetVariables = new List<string>();

            // for case A) assigns
            List<DLR.Expression> strand2StrandAssigns = new List<DLR.Expression>();
            // for case B) assigns
            List<DLR.Expression> strand2ValueAssigns = new List<DLR.Expression>()
            {
                DLR.Expression.Assign(
                    valuesParam,
                    DLR.Expression.Call(
                        DLR.Expression.Constant(disclose),
                        disclose.GetType().GetMethod("Execute"),
                        valuesParam,
                        environment
                    )
                )
            };

            int indexCounter = 0;

            foreach (Node node in targets.Items)
            {
                if (!(node is Identifier))
                {
                    throw new ParseException("assign lhs");
                }
                Identifier id = (Identifier)node;

                DLR.Expression strandValue =
                    DLR.Expression.Call(
                        DLR.Expression.Constant(disclose),
                        disclose.GetType().GetMethod("Execute"),
                        DLR.Expression.MakeIndex(
                            valuesParam,
                            typeof(AType).GetIndexerProperty(typeof(int)), // The indexer property which will be used
                            new DLR.Expression[] { DLR.Expression.Constant(indexCounter, typeof(int)) }
                        ),
                        environment
                );

                // case A) $TARGETS[i] = disclose($VALUES[i])
                strand2StrandAssigns.Add(GenerateIdentifierAssign(scope, id, strandValue, isStrand: true));

                // case B) $TARGETS[i] = $VALUE.Clone()
                strand2ValueAssigns.Add(
                    GenerateIdentifierAssign(
                        scope,
                        id,
                        DLR.Expression.Condition(
                            DLR.Expression.Property(valuesParam, "IsMemoryMappedFile"),
                            valuesParam,
                            DLR.Expression.Call(
                                valuesParam,
                                typeof(AType).GetMethod("Clone")
                            )
                        ),
                        isStrand: true
                    )
                );

                // gather the global variables for dependency validation/invalidation. 
                if ((scope.IsMethod && id.IsEnclosed) || !scope.IsMethod)
                {
                    targetVariables.Add(id.BuildQualifiedName(runtime.CurrentContext));
                }

                indexCounter++;
            }

            // case A)
            DLR.Expression caseStrand2Strand =
                DLR.Expression.IfThenElse(
                    DLR.Expression.NotEqual(
                        DLR.Expression.PropertyOrField(valuesParam, "Length"),
                        DLR.Expression.Constant(indexCounter, typeof(int))
                    ),
                    DLR.Expression.Throw(
                        DLR.Expression.New(
                            typeof(Error.Length).GetConstructor(new Type[] { typeof(string) }),
                            DLR.Expression.Constant("assign", typeof(string))
                        )
                    ),
                    DLR.Expression.Block(strand2StrandAssigns)
            );

            DLR.Expression dependencyManager = DLR.Expression.Property(scope.GetRuntimeExpression(), "DependencyManager");

            DLR.Expression result = DLR.Expression.Block(
                new DLR.ParameterExpression[] { valuesParam },
                DLR.Expression.Assign(valuesParam, DLR.Expression.Convert(value, typeof(AType))),
                DLR.Expression.IfThenElse(
                    DLR.Expression.PropertyOrField(valuesParam, "IsArray"),
                // case A)
                    caseStrand2Strand,
                // case B)
                    DLR.Expression.Block(strand2ValueAssigns)
                ),
                DLR.Expression.Call(
                    dependencyManager,
                    typeof(DependencyManager).GetMethod("InvalidateDependencies", new Type[] { typeof(string[]) }),
                    DLR.Expression.Constant(targetVariables.ToArray())
                ),
                DLR.Expression.Call(
                    dependencyManager,
                    typeof(DependencyManager).GetMethod("ValidateDependencies"),
                    DLR.Expression.Constant(targetVariables.ToArray())
                ),
                valuesParam
            );

            return result;
        }

        #endregion

        #region DLR: Assign to Identifier

        internal static DLR.Expression GenerateIdentifierAssign(
            AplusScope scope, Identifier target, DLR.Expression value, bool isStrand = false)
        {
            Aplus runtime = scope.GetRuntime();
            DLR.Expression result = null;

            if (scope.IsMethod)
            {
                // If the generation is inside of a method
                // check if it has the variablename as function parameter
                DLR.Expression functionParameter = scope.FindIdentifier(target.Name);
                if (functionParameter != null)
                {
                    // Found variable as a function parameter, do a simple assign
                    // the variable is already defined in AST.UserDefFunction
                    return DLR.Expression.Assign(functionParameter, value);
                }

                DLR.Expression functionScopeParam = scope.GetModuleExpression();
                DLR.Expression globalScopeParam = scope.Parent.GetModuleExpression();

                switch (target.Type)
                {
                    case IdentifierType.UnQualifiedName:
                        // Need to check if we are inside an eval block:
                        if (scope.IsEval)
                        {
                            // Code is inside an eval block, we behave slightly differently!

                            #region description about what are we doing in this case

                            //
                            // Check if the variable exists in the function's scope
                            //  |-> Exists: set that variable's value
                            //  |-> otherwise: set the global variable's value
                            //
                            // if(((IDictionary<String, Object>)($FunctionScope).ContainsKey($VARIABLE))
                            // {
                            //      $FunctionScope.$VARIABLE = $VALUE;
                            // }
                            // else
                            // {
                            //      $GlobalScope.$VARIABLE = $VALUE;
                            // }
                            //
                            #endregion

                            result = DLR.Expression.Condition(
                                // Check if the varaible exists in the function scope
                                DLR.Expression.Call(
                                    DLR.Expression.Convert(functionScopeParam, typeof(IDictionary<string, object>)),
                                    typeof(IDictionary<string, object>).GetMethod("ContainsKey"),
                                    DLR.Expression.Constant(target.Name)
                                ),
                                // found the variable in function scope, so assign a value to it
                                DLR.Expression.Dynamic(
                                    runtime.SetMemberBinder(target.Name),
                                    typeof(object),
                                    functionScopeParam,
                                    value
                                ),
                                // did NOT found the variable in the function scope
                                // perform the assignment in the global scope
                                BuildGlobalAssignment(scope, runtime, globalScopeParam, target, value, isStrand)
                            );

                        }
                        else if (target.IsEnclosed)
                        {
                            result = BuildGlobalAssignment(scope, runtime, globalScopeParam, target, value, isStrand);
                        }
                        else
                        {
                            // Simple case, we are inside a user defined function, but not inside an eval block
                            result = DLR.Expression.Dynamic(
                                runtime.SetMemberBinder(target.Name),
                                typeof(object),
                                functionScopeParam,
                                value
                            );
                        }
                        break;

                    case IdentifierType.QualifiedName:
                    case IdentifierType.SystemName:
                    default:
                        // Do an assignment on the global scope
                        result = BuildGlobalAssignment(scope, runtime, globalScopeParam, target, value);
                        break;
                }
            }
            else
            {
                result = BuildGlobalAssignment(scope, runtime, scope.GetModuleExpression(), target, value);
            }


            return DLR.Expression.Convert(result, typeof(AType));
        }


        private static DLR.Expression BuildGlobalAssignment(
            AplusScope scope,
            Aplus runtime,
            DLR.Expression variableContainer,
            Identifier target,
            DLR.Expression value,
            bool isStrand = false)
        {
            // Build the ET for updating the dependency
            DLR.Expression dependencyCall;
            string qualifiedName = target.BuildQualifiedName(runtime.CurrentContext);

            if (isStrand)
            {
                // We are inside a starnd assignment, do nothing.
                // Dependency update will be handled later.
                dependencyCall = DLR.Expression.Empty();
            }
            else
            {
                // Otherwise build the dependency invalidation call.
                DLR.Expression dependencyManager = DLR.Expression.Property(scope.GetRuntimeExpression(), "DependencyManager");
                dependencyCall = DLR.Expression.Call(
                    dependencyManager,
                    typeof(DependencyManager).GetMethod("InvalidateDependencies", new Type[] { typeof(string) }),
                    DLR.Expression.Constant(qualifiedName)
                );
            }

            DLR.ParameterExpression valueParam = DLR.Expression.Parameter(typeof(object), "__VALUE__");

            // callback
            /* 
             * CallbackItem callback;
             * if(CallbackManager.TryGetCallback(globalName, out callback)
             * {
             *     CallbackBinder.Invoke(...);
             * }
             */
            DLR.ParameterExpression callbackParameter = DLR.Expression.Parameter(typeof(CallbackItem), "__CALLBACK__");
            DLR.Expression callback = DLR.Expression.Block(
                new DLR.ParameterExpression[] { callbackParameter },
                DLR.Expression.IfThen(
                    DLR.Expression.Call(
                        scope.GetRuntimeExpression().Property("CallbackManager"),
                        typeof(CallbackManager).GetMethod("TryGetCallback"),
                        DLR.Expression.Constant(qualifiedName),
                        callbackParameter
                    ),
                    DLR.Expression.Dynamic(
                // TODO: do not instantiate the binder here
                        new Runtime.Binder.CallbackBinder(),
                        typeof(object),
                        callbackParameter,
                        scope.GetRuntimeExpression(),
                        valueParam
                    )
                )
            );


            // value assignment
            DLR.Expression result = DLR.Expression.Block(
                new DLR.ParameterExpression[] { valueParam },
                DLR.Expression.Assign(valueParam, value),
                VariableHelper.SetVariable(
                    runtime,
                    variableContainer,
                    target.CreateContextNames(runtime.CurrentContext),
                    valueParam
                ),
                dependencyCall,
                callback,
                valueParam
            );

            return result;
        }

        #endregion

        #region Assignment DLR Helpers

        /// <summary>
        /// Builds DLR expression for:  (iota rho argument)
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        internal static DLR.Expression BuildIndicesList(AplusScope scope, DLR.Expression argument)
        {
            DLR.ParameterExpression environment = scope.GetRuntimeExpression();
            var execute = typeof(AbstractMonadicFunction).GetMethod("Execute");

            // (iota rho x)
            DLR.Expression indexes = DLR.Expression.Call(
                DLR.Expression.Constant(MonadicFunctionInstance.Interval),
                execute,
                DLR.Expression.Call(
                    DLR.Expression.Constant(MonadicFunctionInstance.Shape),
                    execute,
                    argument,
                    environment
                ),
                environment
            );

            return indexes;
        }

        /// <summary>
        /// Build DLR expression for: (,target)[indexExpression]
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="target"></param>
        /// <param name="indexExpression">The indexer expression containing the expression returned from <see cref="BuildIndicesList"/></param>
        /// <returns></returns>
        internal static DLR.Expression BuildIndexing(AplusScope scope, DLR.Expression target, DLR.Expression indexExpression)
        {
            var execute = typeof(AbstractMonadicFunction).GetMethod("Execute");
            DLR.ParameterExpression indexes = DLR.Expression.Parameter(typeof(AType), "_INDEX_");

            // (,x)
            DLR.Expression raveledRight = DLR.Expression.Call(
                DLR.Expression.Constant(MonadicFunctionInstance.Ravel),
                execute,
                target,
                scope.GetRuntimeExpression()
            );

            // (,x)[indexExpression] := ..
            DLR.Expression result = DLR.Expression.Convert(
                DLR.Expression.Dynamic(
                    scope.GetRuntime().GetIndexBinder(new DYN.CallInfo(1)),
                    typeof(object),
                    raveledRight,
                    DLR.Expression.Call(
                        typeof(Helpers).GetMethod("BuildIndexerArray"),
                        DLR.Expression.NewArrayInit(typeof(AType), indexExpression)
                    )
                ),
                typeof(AType)
            );

            return result;
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return String.Format("Assign({0} {1})", this.target, this.expression);

        }

        public override bool Equals(object obj)
        {
            if (obj is Assign)
            {
                Assign other = (Assign)obj;
                return this.target.Equals(other.target) && this.expression.Equals(other.expression);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.target.GetHashCode() ^ this.expression.GetHashCode();
        }

        #endregion
    }

    #region Construction helper

    public partial class Node
    {
        /// <summary>
        /// Build a AST node representing an assignment.
        /// </summary>
        /// <param name="target">The target of the assignment.</param>
        /// <param name="expression">The value of the assignment.</param>
        /// <returns><see cref="Assign">Assignment</see> AST node.</returns>
        public static Assign Assign(Node target, Node expression)
        {
            return new Assign(target, expression);
        }
    }

    #endregion
}
