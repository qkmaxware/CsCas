namespace Qkmaxware.Cas {

/// <summary>
/// Common mathematical functions as exponents
/// </summary>
public static class Pow {

    /// <summary>
    /// Square root
    /// </summary>
    /// <param name="expression">argument expression tree</param>
    /// <returns>Expression tree for square root</returns>
    public static Exponentiation Sqrt(BaseExpression expression) {
        return new Exponentiation(expression, Real.Sqrt);
    }

}

}