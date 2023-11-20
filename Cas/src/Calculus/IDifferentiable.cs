using System;

namespace Qkmaxware.Cas.Calculus {

/// <summary>
/// Interface for functions that have known derivatives
/// </summary>
public interface IDifferentiable {
    public IExpression GetDerivative();
}

}