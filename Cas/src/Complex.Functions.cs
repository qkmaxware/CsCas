using System;

namespace Qkmaxware.Cas {

public partial class Complex {
    public static IExpression Arg(IExpression value) => new Functions.ComplexArgument(value);
    public static IExpression ExtractReal(IExpression value) => new Functions.ExtractReal(value);
    public static IExpression ExtractImaginary(IExpression value) => new Functions.ExtractImaginary(value);
}

}