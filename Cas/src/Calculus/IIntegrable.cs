using System;

namespace Qkmaxware.Cas.Calculus {

/// <summary>
/// Interface for functions that have known integrals
/// </summary>
public interface IIntegrable {
    IExpression GetIntegral();
}

}