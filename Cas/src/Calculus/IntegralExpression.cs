namespace Qkmaxware.Cas {

/// <summary>
/// Expression for integrals that cannot be solved via the solver
/// </summary>
public class IntegralExpression : BaseExpression {
    private Symbol wrt;
    private IExpression expr;

    public IntegralExpression(Symbol wrt, IExpression expr) {
        this.wrt = wrt;
        this.expr = expr;
    }

    public override bool Equals(object obj) {
        return obj switch {
            IntegralExpression expr => expr.expr.Equals(this.expr) && expr.wrt.Equals(this.wrt),
            _ => base.Equals(obj)
        };
    }

    public override int GetHashCode() {
        return System.HashCode.Combine(expr, wrt);
    }

    public override IExpression Simplify() {
        return new IntegralExpression(wrt, this.expr.Simplify());
    }

    public override bool IsConstant() => this.expr.IsConstant();

    public override string ToString() {
        return $"∫({expr})d{wrt}";
    }

    public override IExpression When(params Substitution[] substitutions) {
        return new DerivativeExpression(wrt, this.expr.When(substitutions));
    }
}

}