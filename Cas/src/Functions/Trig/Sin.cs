using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Sin : Function, IInvertable, IDifferentiable, IIntegrable {
    public Sin(IExpression arg) : base(arg) {}

    public Function GetInverseWithArg(IExpression arg) {
        return new Asin(arg);
    }

    public IExpression GetDerivativeExpressionWithArg(IExpression arg) {
        return new Cos(arg);
    }

    public IExpression GetDerivative() {
        return new Cos(this.Argument);
    }

    public IExpression GetIntegral() {
        return new Multiplication(Real.NegativeOne, new Cos(this.Argument));
    }

    public override IExpression When(params Substitution[] substitutions) {
        return new Sin(this.Argument.When(substitutions));
    }

    public override IExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Sin(realArg.Value));
        } else {
            return new Sin(newArg);
        }
    } 

    public override bool IsConstant() => this.Argument.IsConstant();
}

}