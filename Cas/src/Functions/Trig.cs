namespace Qkmaxware.Cas {

/// <summary>
/// Trigonometric functions as expression trees
/// </summary>
public static class Trig {
    /// <summary>
    /// Sine
    /// </summary>
    /// <param name="expr">argument expression tree</param>
    /// <returns>Sine expression tree</returns>
    public static Functions.Sin Sin(BaseExpression expr) {
        return new Functions.Sin(expr);
    } 
    /// <summary>
    /// Cosine
    /// </summary>
    /// <param name="expr">argument expression tree</param>
    /// <returns>Cosine expression tree</returns>
    public static Functions.Cos Cos(BaseExpression expr) {
        return new Functions.Cos(expr);
    }
    /// <summary>
    /// Tangent
    /// </summary>
    /// <param name="expr">argument expression tree</param>
    /// <returns>Tangent expression tree</returns>
    public static Functions.Tan Tan(BaseExpression expr) {
        return new Functions.Tan(expr);
    }  

    /// <summary>
    /// Inverse Sine
    /// </summary>
    /// <param name="expr">argument expression tree</param>
    /// <returns>Inverse sine expression tree</returns>
    public static Functions.Asin Asin(BaseExpression expr) {
        return new Functions.Asin(expr);
    } 
    /// <summary>
    /// Inverse Cosine
    /// </summary>
    /// <param name="expr">argument expression tree</param>
    /// <returns>Inverse cosine expression tree</returns>
    public static Functions.Acos Acos(BaseExpression expr) {
        return new Functions.Acos(expr);
    }
    /// <summary>
    /// Inverse Tangent
    /// </summary>
    /// <param name="expr">argument expression tree</param>
    /// <returns>Inverse tangent expression tree</returns>
    public static Functions.Atan Atan(BaseExpression expr) {
        return new Functions.Atan(expr);
    }
}

}