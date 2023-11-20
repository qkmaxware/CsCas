using System;
using System.Diagnostics;

namespace Qkmaxware.Cas.Calculus {

/*
var expr = y <= x^2
expr.Integrate(x)
//or
expr.Integrate(y, 10..20);
*/

/// <summary>
/// Extensions for performing integration
/// </summary>
public static class IntegrationExtensions {
    /// <summary>
    /// Compute the indefinite integral
    /// </summary>
    /// <param name="expr">expression to integrate</param>
    /// <param name="dx">variable to integrate with respect to</param>
    /// <returns>integral</returns>
    public static IExpression Integrate(this IExpression expr, Symbol dx) {
        return new IntegrationMatcher(dx).Integrate(expr);
    }
    /// <summary>
    /// Compute the definite integral
    /// </summary>
    /// <param name="expr">expression to integrate</param>
    /// <param name="dx">variable to integrate with respect to</param>
    /// <param name="bounds">upper and lower bounds of the integration variable</param>
    /// <returns>integral</returns>
    public static IExpression Integrate(this IExpression expr, Symbol dx, Range bounds) {
        var indefinite = expr.Integrate(dx);
        return new Subtraction(
            indefinite.When(new SymbolicSubstitution(dx, bounds.End.Value)),
            indefinite.When(new SymbolicSubstitution(dx, bounds.Start.Value))
        );
    }

}

/// <summary>
/// Integration expression transformer
/// </summary>
public class IntegrationMatcher {
    public Symbol wrt;

    public IntegrationMatcher(Symbol wrt) {
        this.wrt = wrt;
    }

    public IExpression Integrate(IExpression expr) {
        var C = new ConstantOfIntegration();
        return expr switch {
            #region Common functions
            Complex complex => (complex * wrt) + C,                                         // Constant
            Symbol symbol when symbol != wrt => (symbol * wrt) + C,                         // Constant by not being ME
            Symbol symbol when symbol == wrt => new Exponentiation(expr,new Real(2))/2 + C, // Variable
            Division { Numerator: Real { Real: 1, Imaginary: 0 }, Denominator: Symbol symbol} when symbol == wrt => Log.Ln(symbol) + C, // Reciprocal
            Exponentiation { Root: Real a, Power: Symbol symbol } when symbol == wrt => new Division(expr, Log.Ln(a)) + C, // Exponential
            IIntegrable integrable => new Addition(integrable.GetIntegral(), C),            // Known function
            #endregion

            #region Common rules
            Multiplication { Lhs: Real a } mul => new Multiplication(a, mul.Rhs.Integrate(wrt)), // Constant multiplication rule
            Exponentiation { Root: Symbol x, Power: Real n } when x == wrt => (x^(n + 1))/(n + 1), // Power rule 
            Addition addition => new Addition(addition.Lhs.Integrate(wrt), addition.Rhs.Integrate(wrt)), // Sum rule
            Subtraction subtraction => new Subtraction(subtraction.Lhs.Integrate(wrt), subtraction.Rhs.Integrate(wrt)),// Difference rule
            #endregion

            #region Special rules
                #region Source: Shaum's Outline 2E (1968) page 57+ 
                // Page 64 in some PDF copies
                #endregion
            #endregion

            #region Fallback
            _ => new IntegralExpression(this.wrt, expr) // Generic fallback, just wrap it with an integration call
            #endregion
        };
    }
}

}