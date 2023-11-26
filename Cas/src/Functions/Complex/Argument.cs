using System;

namespace Qkmaxware.Cas.Functions;

public class ComplexArgument : Function {

    public ComplexArgument(IExpression arg) : base(arg) {}

    public override bool IsConstant() => this.Argument.IsConstant();

    public override IExpression Simplify() {
        var arg = this.Argument.Simplify();
        if (arg is not Complex value) {
            return new ComplexArgument(arg);
        }
        return new Complex(value.Arg(), 0);
    }

    public override IExpression When(params Substitution[] substitutions) => new Transpose(this.Argument.When(substitutions));
}