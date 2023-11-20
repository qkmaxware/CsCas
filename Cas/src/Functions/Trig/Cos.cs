using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Cos : Function, IInvertable, IDifferentiable, IIntegrable {
    public Cos(IExpression arg) : base(arg) {}

    public Function GetInverseWithArg(IExpression arg) {
        return new Acos(arg);
    }

    public IExpression GetDerivativeExpressionWithArg(IExpression arg) {
        return new Multiplication(
            Real.NegativeOne,
            new Sin(arg)
        );
    }

    public IExpression GetDerivative() {
        return new Multiplication(
            Real.NegativeOne,
            new Sin(this.Argument)
        );
    }

    public IExpression GetIntegral() {
        return new Sin(this.Argument);
    }

    public override IExpression When(params Substitution[] substitutions) {
        return new Cos(this.Argument.When(substitutions));
    }

    public override IExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Cos(realArg.Value));
        } else {
            return new Cos(newArg);
        }
    } 

    public override bool IsConstant() => this.Argument.IsConstant();
    
}

}