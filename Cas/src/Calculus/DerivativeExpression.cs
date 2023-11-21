namespace Qkmaxware.Cas {

/// <summary>
/// Expression for derivatives that cannot be solved via the solver
/// </summary>
public class DerivativeExpression : BaseExpression {
    private Symbol wrt;
    public Symbol WithRespectTo => wrt;
    private IExpression expr;
    public IExpression ExpressionToDerivate => expr;

    public DerivativeExpression(Symbol wrt, IExpression expr) {
        this.wrt = wrt;
        this.expr = expr;
    }

    public override bool Equals(object obj) {
        return obj switch {
            DerivativeExpression expr => expr.expr.Equals(this.expr) && expr.wrt.Equals(this.wrt),
            _ => base.Equals(obj)
        };
    }

    public override int GetHashCode() {
        return System.HashCode.Combine(expr, wrt);
    }

    public override IExpression Simplify() {
        return new DerivativeExpression(wrt, this.expr.Simplify());
    }

    public override bool IsConstant() => this.expr.IsConstant();

    public override string ToString() {
        return $"d/d{wrt}({expr})";
    }

    public override IExpression When(params Substitution[] substitutions) {
        return new DerivativeExpression(wrt, this.expr.When(substitutions));
    }
}

}