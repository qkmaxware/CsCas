namespace Qkmaxware.Cas {

/// <summary>
/// Common mathematical functions as expression trees
/// </summary>
public static class Log {
    /// <summary>
    /// Natural logarithm (ln)
    /// </summary>
    /// <param name="expression">argument expression tree</param>
    /// <returns>Expression tree  for logarithm</returns>
    public static Logarithm Ln(BaseExpression expression) {
        return new Logarithm(Real.E, expression);
    }

    /// <summary>
    /// Base 2 logarithm
    /// </summary>
    /// <param name="expression">argument expression tree</param>
    /// <returns>Expression tree for logarithm</returns>
    public static Logarithm Log2(BaseExpression expression) {
        return new Logarithm(new Real(2), expression);
    }

    /// <summary>
    /// Base 10 logarithm
    /// </summary>
    /// <param name="expression">argument expression tree</param>
    /// <returns>Expression tree for logarithm</returns>
    public static Logarithm Log10(BaseExpression expression) {
        return new Logarithm(new Real(10), expression);
    }

}

}