namespace Qkmaxware.Cas {

public interface IValueLike {
    public bool IsZero();
    //public static IValueLike operator +(IValueLike left, IValueLike right);
}

public interface IAdd {
    public bool CanAdd(IValueLike? value);
    public BaseExpression Add(IValueLike? value);
}

public interface ISubtract {
    public bool CanSubtract(IValueLike? value);
    public BaseExpression Subtract(IValueLike? value);
}

public interface IMultiply {
    public bool CanMultiply(IValueLike? value);
    public BaseExpression Multiply(IValueLike? value);
}

public interface IDivide {
    public bool CanDivide(IValueLike? value);
    public BaseExpression Divide(IValueLike? value);
}

public interface IExponent {
    public bool CanRaise(IValueLike? value);
    public BaseExpression Raise(IValueLike? value);
}

public interface INumber : IValueLike, IAdd, ISubtract, IMultiply, IDivide, IExponent {
    public bool IsNaN();
    public bool IsPositiveInfinity();
    public bool IsNegativeInfinity();
}
    
}