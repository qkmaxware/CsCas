using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas.Functions {

public class Cos : Function, IInvertable, IDifferentiable {
    public Cos(BaseExpression arg) : base(arg) {}

    public Function GetInverseWithArg(BaseExpression arg) {
        throw new NotImplementedException();
    }

    public BaseExpression GetDerivativeExpressionWithArg(BaseExpression arg) {
        return new Multiplication(
            new Real(-1),
            new Sin(arg)
        );
    }

    public override BaseExpression When(params Substitution[] substitutions) {
        return new Cos(this.Argument.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newArg = this.Argument.Simplify();
        // Simplifications
        // If the argument is a real number
        if (newArg is Real realArg) {
            return new Real(Math.Asin(realArg.Value));
        } else {
            return new Cos(newArg);
        }
    } 
    
}

}