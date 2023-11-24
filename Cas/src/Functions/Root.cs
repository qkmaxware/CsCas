namespace Qkmaxware.Cas {

/// <summary>
/// Common Nth roots as expression trees
/// </summary>
public static class Root {
    /// <summary>
    /// Square root of the expression
    /// </summary>
    /// <param name="expr">expression to take the square root of</param>
    /// <returns>sqrt</returns>
    public static NthRoot Square(IExpression expr) {
        return new NthRoot(Real.Two, expr);
    }
    /// <summary>
    /// Cube root of the expression
    /// </summary>
    /// <param name="expr">expression to take the cube root of</param>
    /// <returns>cube root</returns>
    public static NthRoot Cube(IExpression expr) {
        return new NthRoot(Real.Three, expr);
    }
}

}