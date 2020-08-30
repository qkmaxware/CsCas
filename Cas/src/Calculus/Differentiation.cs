namespace Qkmaxware.Cas.Calculus {

/*
var expr = y <= x^2
expr.Differentiate().WithRespectTo(y);
//or
expr.Differentiate(y);
*/

public static class DifferentiationExtentions {
    public static BaseExpression Differentiate(this BaseExpression expr, Symbol dx) {
        return new DifferentiationVisitor(dx).VisitExpressionNode(expr);
    }

    public static Assignment Differentiate(this Assignment assignment, Symbol dx) {
        return new DifferentiationVisitor(dx).VisitAssignment(assignment);
    }
}

public class DifferentiationVisitor : ExpressionVisitor {

    private Symbol derivativeSymbol;

    public DifferentiationVisitor(Symbol withRespectTo) {
        this.derivativeSymbol = withRespectTo;
    }

    public override BaseExpression VisitAddition(Addition addition) {
        // f + g -> f` + g`
        return new Addition(this.VisitExpressionNode(addition.Lhs), this.VisitExpressionNode(addition.Rhs));
    }

    public override BaseExpression VisitSubtraction(Subtraction subtraction) {
        // f - g -> f` - g`
        return new Subtraction(this.VisitExpressionNode(subtraction.Lhs), this.VisitExpressionNode(subtraction.Rhs));
    }

    public override BaseExpression VisitMultiplication(Multiplication multiplication) {
        // f*g -> f*g` + f`*g
        var f = multiplication.Lhs;
        var g = multiplication.Rhs;
        return new Addition(
            lhs: new Multiplication(
                f,
                this.VisitExpressionNode(g)
            ),
            rhs: new Multiplication(
                this.VisitExpressionNode(f),
                g
            )
        );
    }

    public override BaseExpression VisitDivision(Division division) {
        // f/g -> (f` * g − g` * f)/g2
        var f = division.Lhs;
        var g = division.Rhs;
        return new Division(
            lhs: new Subtraction(
                lhs: new Multiplication(
                    this.VisitExpressionNode(f),
                    g
                ),
                rhs: new Multiplication(
                    this.VisitExpressionNode(g),
                    f
                )
            ),
            rhs: new Division(
                g,
                g
            )
        );
    }

    public override BaseExpression VisitExponentiation(Exponentiation exponentiation) {
        // (g(x)^f(x)) = g(x)^(f(x) - 1) * (g(x) * f'(x) * log(g(x)) + f(x) * g'(x))
        var g = exponentiation.Root;
        var f = exponentiation.Power;

        return new Multiplication(
            new Exponentiation(
                g,
                new Subtraction(f, Constant.One)
            ),
            new Addition(
                new Multiplication(
                    new Multiplication(
                        g,
                        this.VisitExpressionNode(f)
                    ),
                    new Logarithm(
                        Constant.E,
                        g
                    )
                ),
                new Multiplication(
                    f,
                    this.VisitExpressionNode(g)
                )
            )
        );
    }

    public override BaseExpression VisitLogarithm(Logarithm logarithm) {
        // log_a(x) -> 1 / (x ln(a))
        var x = logarithm.Argument;
        var a = logarithm.Base;
        return new Division(
            lhs: Constant.One,
            rhs: new Multiplication(
                x,
                new Logarithm(Constant.E, a)
            )
        );
    }

    public override BaseExpression VisitSymbol(Symbol symbol) {
        if (symbol == this.derivativeSymbol) {
            return Constant.One;
        } else {
            return new DerivativeSymbol(symbol); 
        }
    }

    public override BaseExpression VisitContant(Constant constant) {
        return Constant.Zero;
    }
}

}