<p align="center">
  <img width="120" height="120" src="logo.svg">
</p>

# C# CAS
Simple computer algebra system in C# using a DSL (domain specific language) to construct mathematical expression trees which can be manipulated, transformed, and evaluated.

# Build Status
![](https://github.com/qkmaxware/CsCas/workflows/Build/badge.svg)

# Getting Started
The library is available as a NuGet package for any .Net implementation that supports the .Net Standard 2.0. Visit the [Packages](https://github.com/qkmaxware/CsCas/packages) page for downloads.

## Examples
### Creating expressions
```cs
var x = Symbol("x");
var y = Symbol("y");

var expression = y <= (x^2) + 6; 
```

### Evaluating expressions
```cs
var x = Symbol("x");
var y = Symbol("y");

var expression = y <= (x^2) + 6; 
var y6 = expression.Where(x == 6).Value;
```

### Algebraic rearrangement
```cs
var x = Symbol("x");
var y = Symbol("y");

var expression = y <= (x^2) + 6; 
var expression_for_x = expression.SolveFor(x);
```

### Differentiation
```cs
Symbol y = new Symbol("y");
Symbol x = new Symbol("x");

var expr = y <= (x^2);
var ddx = expr.Differentiate(x).Simplify();
```

### Functions for Expression Trees
```cs
var x = new Symbol("x");
var y = new Symbol("y");

var equation = y <= Trig.Sin(x);
```
Several common functions have already been created, such as the trigonometric functions which can be accessed using static methods like seen above. For adding custom functions, extend the `Function` class. In order for custom functions to work with algebraic rearrangement, additionally implement the `IInvertable` interface. In order for custom functions to work with the derivative calculator, additionally implement the `IDifferentiable` interface.

## Made With
- [.Net Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
  
## License
See [License](LICENSE.md) for license details.