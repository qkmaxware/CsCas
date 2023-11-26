using System;

namespace Qkmaxware.Cas {

public partial class Matrix {
    public static IExpression Transpose(IExpression expr) => new Functions.Transpose(expr);
    public IExpression Transpose() => Transpose(this);
}

}