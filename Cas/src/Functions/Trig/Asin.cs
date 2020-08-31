using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Asin : Function, IInvertable, IDifferentiable {
    public Asin(BaseExpression arg) : base(arg) {}

    public Function GetInverseWithArg(BaseExpression arg) {
        return new Sin(arg);
    }

    public BaseExpression GetDerivativeExpressionWithArg(BaseExpression arg) {
        // 1 / sqrt(1 - x^2)
        return new Division(
            Real.One,
            new Exponentiation(
                new Subtraction(Real.One, new Multiplication(arg, arg)),
                Real.Sqrt
            )
        );
    }

    public override BaseExpression When(params Substitution[] substitutions) {
        return new Asin(this.Argument.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Asin(realArg.Value));
        } else {
            return new Asin(newArg);
        }
    } 
    
}

}