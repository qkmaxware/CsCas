namespace Qkmaxware.Cas {

/// <summary>
/// Common mathematical equations
/// </summary>
public static class Equation {
    /// <summary>
    /// Standard slope-intersection linear equation
    /// </summary>
    /// <param name="m">multiplier for x</param>
    /// <param name="b">offset</param>
    /// <returns>y = mx + b</returns>
    public static Assignment SlopeIntersectLine(IExpression m, IExpression b) {
        Symbol y = new Symbol("y");
        Symbol x = new Symbol("x");
        return new Assignment(y, new Addition(new Multiplication(m, x), b));
    }
}

}