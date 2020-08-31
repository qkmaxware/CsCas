using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Tan : Function, IInvertable, IDifferentiable {
    public Tan(BaseExpression arg) : base(arg) {}

    public Function GetInverseWithArg(BaseExpression arg) {
        return new Atan(arg);
    }

    public BaseExpression GetDerivativeExpressionWithArg(BaseExpression arg) {
        // sec^2(x) == 1 + tan^2(x)
        return new Addition(
            Real.One,
            new Multiplication(
                new Tan(arg),
                new Tan(arg)
            )
        );
    }

    public override BaseExpression When(params Substitution[] substitutions) {
        return new Tan(this.Argument.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Tan(realArg.Value));
        } else {
            return new Tan(newArg);
        }
    } 
    
}

}