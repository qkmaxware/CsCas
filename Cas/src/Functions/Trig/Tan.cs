using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Tan : Function, IInvertable, IDifferentiable, IIntegrable {
    public Tan(IExpression arg) : base(arg) {}

    public Function GetInverseWithArg(IExpression arg) {
        return new Atan(arg);
    }

    public IExpression GetDerivativeExpressionWithArg(IExpression arg) {
        // sec^2(x) == 1 + tan^2(x)
        return new Addition(
            Real.One,
            new Multiplication(
                new Tan(arg),
                new Tan(arg)
            )
        );
    }

    public IExpression GetDerivative() {
        // sec^2(x) == 1 + tan^2(x)
        return new Addition(
            Real.One,
            new Multiplication(
                new Tan(this.Argument),
                new Tan(this.Argument)
            )
        );
    }

    public IExpression GetIntegral() {
        return new Multiplication(
            Real.NegativeOne,
            Log.Ln(
                new Cos(this.Argument)
            )
        );
    }

    public override IExpression When(params Substitution[] substitutions) {
        return new Tan(this.Argument.When(substitutions));
    }

    public override IExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Tan(realArg.Value));
        } else {
            return new Tan(newArg);
        }
    } 

    public override bool IsConstant() => this.Argument.IsConstant();
    
}

}