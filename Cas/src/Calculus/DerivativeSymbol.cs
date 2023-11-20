namespace Qkmaxware.Cas {

/// <summary>
/// Symbol for a variable that has been taken the derivative of
/// </summary>
public class DerivativeSymbol : Symbol {
    private Symbol baseSymbol;

    public DerivativeSymbol(Symbol symbol) {
        this.baseSymbol = symbol;
        this.Identifier = symbol.Identifier + "`";
    }

    public override bool Equals(object obj) {
        return obj switch {
            DerivativeSymbol expr => expr.baseSymbol.Equals(this.baseSymbol),
            _ => base.Equals(obj)
        };
    }

    public override int GetHashCode() {
        return baseSymbol.GetHashCode();
    }

    public override string ToString() {
        return Identifier;
    }
}

}