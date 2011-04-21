﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;

namespace AplusCore.Runtime
{
    public class DependencyItem
    {
        #region Variables

        private string variableName;
        private HashSet<string> dependentItems;
        private DependencyState state;
        private AType function;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the Qualified name of the associated variable.
        /// </summary>
        public string VariableName
        {
            get { return this.variableName; }
        }

        /// <summary>
        /// State of the Dependency
        /// </summary>
        public DependencyState State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Function associated for the dependency
        /// </summary>
        public AType Function
        {
            get { return this.function; }
        }

        #endregion

        #region Constructors

        public DependencyItem(string variableName, HashSet<string> dependentItems, AType function)
        {
            this.variableName = variableName;
            this.dependentItems = dependentItems;
            this.function = function;

            // By default the dependencies are invalid.
            MarkAsInvalid();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the dependency definition contains the variable.
        /// </summary>
        /// <param name="variableName">Name of the variable to check for.</param>
        /// <returns>True if the variable is used inside the dependency's definition.</returns>
        public bool ContainsVariable(string variableName)
        {
            return this.dependentItems.Contains(variableName);
        }

        /// <summary>
        /// Check if the dependency definition contains any of the variables.
        /// </summary>
        /// <param name="variableNames">Names of the variables to check for.</param>
        /// <returns>True if any of the variables is used inside the dependency's definition.</returns>
        public bool ContainsVariable(string[] variableNames)
        {
            return this.dependentItems.Any(item => { return variableNames.Contains(item); });
        }

        /// <summary>
        /// Mark the dependency as invalid.
        /// </summary>
        public void MarkAsInvalid()
        {
            this.state = DependencyState.Invalid;
        }

        /// <summary>
        /// Mark the dependency.
        /// </summary>
        /// <param name="state"></param>
        public void Mark(DependencyState state)
        {
            this.state = state;
        }

        #endregion
    }
}
