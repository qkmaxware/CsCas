namespace Qkmaxware.Cas {

public abstract class BaseExpression : IAlgebraicEntity {

    // +, -, *, /, ^, (<= | >=)?

    /*
        var x = new Symbol();                               
        var y = new Symbol();                              
        var a = new Symbol();                               
        var expr  = a^2 <= x^2 + y^2;                       
        var expr2 = expr.SolveFor(x);                               // x <= (a^2 - y^2) ^ (1/2)
        var expr3 = (double)expr2.When(y == 4, a == 2).Simplify();    // x <= (-12) ^ (0.5)
    */

    public abstract BaseExpression When (params Substitution[] substitutions);
    public abstract BaseExpression Simplify();

    public static Exponentiation operator ^ (BaseExpression a, BaseExpression b) {
        return new Exponentiation(a, b);
    }

    public static Exponentiation operator ^ (double a, BaseExpression b) {
        return new Exponentiation(new Constant(a), b);
    }

    public static Exponentiation operator ^ (BaseExpression a, double b) {
        return new Exponentiation(a, new Constant(b));
    }

    public static Addition operator + (BaseExpression a, BaseExpression b) {
        return new Addition(a, b);
    }

    public static Addition operator + (double a, BaseExpression b) {
        return new Addition(new Constant(a), b);
    }

    public static Addition operator + (BaseExpression a, double b) {
        return new Addition(a, new Constant(b));
    }

    public static Subtraction operator - (BaseExpression a, BaseExpression b) {
        return new Subtraction(a, b);
    }

    public static Subtraction operator - (double a, BaseExpression b) {
        return new Subtraction(new Constant(a), b);
    }

    public static Subtraction operator - (BaseExpression a, double b) {
        return new Subtraction(a, new Constant(b));
    }

    public static Multiplication operator * (BaseExpression a, BaseExpression b) {
        return new Multiplication(a, b);
    }

    public static Multiplication operator * (double a, BaseExpression b) {
        return new Multiplication(new Constant(a), b);
    }

    public static Multiplication operator * (BaseExpression a, double b) {
        return new Multiplication(a, new Constant(b));
    }

    public static Division operator / (BaseExpression a, BaseExpression b) {
        return new Division(a, b);
    }

    public static Division operator / (double a, BaseExpression b) {
        return new Division(new Constant(a), b);
    }

    public static Division operator / (BaseExpression a, double b) {
        return new Division(a, new Constant(b));
    }

    public static Subtraction operator - (BaseExpression a) {
        return new Subtraction(Constant.Zero, a);
    }

    public static Assignment operator <= (BaseExpression a, BaseExpression b) {
        return new Assignment(a, b);
    }
    public static Assignment operator <= (double a, BaseExpression b) {
        return new Assignment(new Constant(a), b);
    }
    public static Assignment operator <= (BaseExpression a, double b) {
        return new Assignment(a, new Constant(b));
    }

    public static Assignment operator >= (BaseExpression a, BaseExpression b) {
        return new Assignment(b, a);
    }
    public static Assignment operator >= (double a, BaseExpression b) {
        return new Assignment(b, new Constant(a));
    }
    public static Assignment operator >= (BaseExpression a, double b) {
        return new Assignment(new Constant(b), a);
    }

}

}