using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Sin : Function, IInvertable, IDifferentiable {
    public Sin(BaseExpression arg) : base(arg) {}

    public Function GetInverseWithArg(BaseExpression arg) {
        return new Asin(arg);
    }

    public BaseExpression GetDerivativeExpressionWithArg(BaseExpression arg) {
        return new Cos(arg);
    }

    public override BaseExpression When(params Substitution[] substitutions) {
        return new Sin(this.Argument.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Sin(realArg.Value));
        } else {
            return new Sin(newArg);
        }
    } 
}

}