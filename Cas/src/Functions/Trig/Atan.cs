using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Atan : Function, IInvertable, IDifferentiable {
    public Atan(BaseExpression arg) : base(arg) {}

    public Function GetInverseWithArg(BaseExpression arg) {
        return new Tan(arg);
    }

    public BaseExpression GetDerivativeExpressionWithArg(BaseExpression arg) {
        // 1 / (1 + x^2)
        return new Division(
            Real.One,
            new Addition(Real.One, new Multiplication(arg, arg))
        );
    }

    public override BaseExpression When(params Substitution[] substitutions) {
        return new Atan(this.Argument.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Atan(realArg.Value));
        } else {
            return new Atan(newArg);
        }
    } 
    
}

}