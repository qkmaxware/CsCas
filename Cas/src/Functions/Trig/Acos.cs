using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Acos : Function, IInvertable, IDifferentiable {
    public Acos(IExpression arg) : base(arg) {}

    public Function GetInverseWithArg(IExpression arg) {
        return new Cos(arg);
    }

    public IExpression GetDerivativeExpressionWithArg(IExpression arg) {
        // -1 / sqrt(1 - x^2)
        return new Division(
            Real.NegativeOne,
            new Exponentiation(
                new Subtraction(Real.One, new Multiplication(arg, arg)),
                Real.Sqrt
            )
        );
    }

    public IExpression GetDerivative() {
        // -1 / sqrt(1 - x^2)
        return new Division(
            Real.NegativeOne,
            new Exponentiation(
                new Subtraction(Real.One, new Multiplication(this.Argument, this.Argument)),
                Real.Sqrt
            )
        );
    }

    public override IExpression When(params Substitution[] substitutions) {
        return new Cos(this.Argument.When(substitutions));
    }

    public override IExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Acos(realArg.Value));
        } else {
            return new Acos(newArg);
        }
    } 

    public override bool IsConstant() => this.Argument.IsConstant();
    
}

}