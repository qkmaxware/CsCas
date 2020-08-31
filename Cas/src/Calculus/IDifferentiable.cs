using System;

namespace Qkmaxware.Cas.Calculus {

public interface IDifferentiable {
    BaseExpression GetDerivativeExpressionWithArg(BaseExpression arg);
}

}