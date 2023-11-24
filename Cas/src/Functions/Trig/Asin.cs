using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Asin : Function, IInvertable, IDifferentiable {
    public Asin(IExpression arg) : base(arg) {}

    public Function GetInverseWithArg(IExpression arg) {
        return new Sin(arg);
    }

    public IExpression GetDerivativeExpressionWithArg(IExpression arg) {
        // 1 / sqrt(1 - x^2)
        return new Division(
            Real.One,
            Root.Square(new Subtraction(Real.One, new Multiplication(arg, arg)))
        );
    }

    public IExpression GetDerivative() {
        // 1 / sqrt(1 - x^2)
        return new Division(
            Real.One,
            Root.Square(new Subtraction(Real.One, new Multiplication(this.Argument, this.Argument)))
        );
    }

    public override IExpression When(params Substitution[] substitutions) {
        return new Asin(this.Argument.When(substitutions));
    }

    public override IExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Asin(realArg.Value));
        } else {
            return new Asin(newArg);
        }
    } 

    public override bool IsConstant() => this.Argument.IsConstant();
    
}

}