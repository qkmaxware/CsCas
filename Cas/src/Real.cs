namespace Qkmaxware.Cas {

public class Real : BaseExpression {
    public static readonly Real Zero = new Real(0);
    public static readonly Real One = new Real(1);
    public static readonly Real Sqrt = new Real(0.5);
    public static readonly Real NegativeOne = new Real(-1);
    public static readonly Real E = new Real(System.Math.E);

    public double Value {get; private set;}
    public Real(double value) {
        this.Value = value;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        return this;
    }

    public override BaseExpression Simplify() {
        return this;
    }

    public static implicit operator Real (double value) {
        return new Real(value);
    }

    public override bool Equals(object obj) {
        return obj switch {
            Real expr => expr.Value.Equals(this.Value),
            _ => base.Equals(obj)
        };
    }

    public override int GetHashCode() {
        return Value.GetHashCode();
    }

    public override string ToString() {
        return Value.ToString();
    }
}

}