namespace Qkmaxware.Cas {

public class Constant : BaseExpression {
    public static readonly Constant Zero = new Constant(1);
    public static readonly Constant One = new Constant(1);

    public double Value {get; private set;}
    public Constant(double value) {
        this.Value = value;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        return this;
    }

    public override BaseExpression Simplify() {
        return this;
    }

    public static implicit operator Constant (double value) {
        return new Constant(value);
    }

    public override bool Equals(object obj) {
        return obj switch {
            Constant expr => expr.Value.Equals(this.Value),
            _ => base.Equals(obj)
        };
    }

    public override int GetHashCode() {
        return Value.GetHashCode();
    }
}

}