using System;

namespace Qkmaxware.Cas.Functions;

public class Real : Function {

    public Real(IExpression arg) : base(arg) {}

    public override bool IsConstant() => this.Argument.IsConstant();

    public override IExpression Simplify() {
        var arg = this.Argument.Simplify();
        if (arg is not Complex value) {
            return new Real(arg);
        }
        return new Complex(value.Real, 0);
    }

    public override IExpression When(params Substitution[] substitutions) => new Transpose(this.Argument.When(substitutions));
}