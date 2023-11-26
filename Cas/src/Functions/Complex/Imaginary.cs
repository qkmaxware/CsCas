using System;

namespace Qkmaxware.Cas.Functions;

public class Imaginary : Function {

    public Imaginary(IExpression arg) : base(arg) {}

    public override bool IsConstant() => this.Argument.IsConstant();

    public override IExpression Simplify() {
        var arg = this.Argument.Simplify();
        if (arg is not Complex value) {
            return new Imaginary(arg);
        }
        return new Complex(0, value.Imaginary);
    }

    public override IExpression When(params Substitution[] substitutions) => new Transpose(this.Argument.When(substitutions));
}