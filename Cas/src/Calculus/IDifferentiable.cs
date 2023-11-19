using System;

namespace Qkmaxware.Cas.Calculus {

public interface IDifferentiable {
    IExpression GetDerivativeExpressionWithArg(IExpression arg);
}

}