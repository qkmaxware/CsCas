namespace Qkmaxware.Cas {

/// <summary>
/// Symbol for a variable that has been taken the derivative of
/// </summary>
public class DerivativeSymbol : DerivativeExpression {
    public DerivativeSymbol(Symbol symbol) : base(symbol, symbol) {}
}

}