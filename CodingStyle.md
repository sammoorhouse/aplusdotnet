

# General #

Curly brackets ({}) are always in a new line.


---

# Using directive #
Use alphabetical order.
Group them by:
  1. .NET Framework namespaces
  1. 3rd party namespaces
  1. Project's namespaces
  1. Aliases for namespaces
Keep an empty line between groups.

Example:
```
using System;
using System.Linq;

using Tool.A;
using Tool.B;

using MyProject.A;
using MyProject.B;

using A = MyProject.A;
```


---


# Regions #
#### Keep the following order for regions and one empty line between regions. ####
```
class A
{
    #region Variables
    ....
    #endregion

    #region Properties
    ....
    #endregion

    #region Constructors
    ....
    #endregion

    #region [Other regions..]
    ....
    #endregion    
}
```

#### Keep one empty line after `#region` and before `#endregion` ####

#### Good: ####
```
class A
{
    #region outer

    public void MyFunc()
    {
        ....
    }

    #endregion
}
```

Bad:
```
class A
{
    #region outer
    public void MyFunc()
    {
        ....
    }
    #endregion
}

```

#### Do not indent nested regions. ####

#### Good: ####
```
class A
{
    #region outer
    #region inner
    #endregion
    #endregion
}
```

Bad:
```
class A
{
    #region outer
        #region inner
        #endregion
    #endregion
}
```


---


# Properties #

#### If both get and set is a one-liner. ####

#### Good: ####
```
public int PropertyA()
{
    get { return variableA; }
    set { variableA = value; }
}
```

Bad:
```
public int PropertyA()
{
    get { return variableA; }
    set
    { 
         variableA = value; 
    }
}
```


#### If get or set is not a one-liner. ####
Keep an empty line between the get and set.

#### Good: ####
```
public int PropertyA()
{
    get
    {
        return variableA;
    }

    set
    { 
        DoBThing();
        variableA = value; 
    }
}
```

Bad:
```
public int PropertyA()
{
    get { return variableA; }
    set
    { 
        DoBThing();
        variableA = value; 
    }
}
```


---


# Control flow #
Always use curly brackets {}. This applies to _if_, _foreach_ etc.

#### Good: ####
```
if(condition)
{
    SingeLineStatement();
}
```

Bad:
```
if(condition)
    SingeLineStatement();
```


## If-else and if-else if ##
_else_ and _else if_ goes to a new line.

#### Good: ####
```
if(conditionA)
{
    ...
}
else if(conditionB)
{
    ...
}
else
{
    ...
}
```

Bad:
```
if(conditionA)
{
    ...
} else if(conditionB) {
    ...
} 
else {
    ...
}
```


---


# Comments #

## Single line ##
#### Single line comments starts with a space and lower cased letter. Also no dot at the end. ####

#### Good: ####
```
// good comment
DoesThis();
```

Bad:
```
//Bad comment.
DoesOther();
```