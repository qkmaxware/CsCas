using System;
using Qkmaxware.Cas.Calculus;

namespace Qkmaxware.Cas {

public interface IInvertable  {
    Function GetInverseWithArg(IExpression arg);
}

}