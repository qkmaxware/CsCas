<p align="center">
  <img width="120" height="120" src="logo.svg">
</p>

# C# Numerics
Simple computer algebra system in C# using a DSL (domain specific language) to construct mathematical expression trees which can be manipulated, transformed, and evaluated.

# Build Status
![](https://github.com/qkmaxware/CsCas/workflows/Build/badge.svg)

# Getting Started
The library is available as a NuGet package for any .Net implementation that supports the .Net Standard 2.0. Visit the [Packages](https://github.com/qkmaxware/CsCas/packages) page for downloads.

# Examples
## Creating expressions
```cs
var x = Symbol("x");
var y = Symbol("y");

var expression = y <= x^2 + 6; 
```

## Evaluating expressions
```cs
var x = Symbol("x");
var y = Symbol("y");

var expression = y <= x^2 + 6; 
var y6 = expression.Where(x == 6).Value;
```

## Algebraic rearrangement
```cs
var x = Symbol("x");
var y = Symbol("y");

var expression = y <= x^2 + 6; 
var expression_for_x = expression.SolveFor(x);
```

## Made With
- [.Net Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
  
## License
See [License](LICENSE.md) for license details.