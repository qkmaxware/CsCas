namespace Qkmaxware.Cas.Calculus {

/*
var expr = y <= x^2
expr.Differentiate().WithRespectTo(y);
//or
expr.Differentiate(y);
*/

/// <summary>
/// Extensions for performing derivatives
/// </summary>
public static class DifferentiationExtensions {
    /// <summary>
    /// Compute the derivative
    /// </summary>
    /// <param name="expr">expression to compute derivative of</param>
    /// <param name="dx">variable to compute derivative with respect to</param>
    /// <returns>derivative</returns>
    public static IExpression Differentiate(this IExpression expr, Symbol dx) {
        return new DifferentiationVisitor(dx).VisitExpressionNode(expr);
    }
    /// <summary>
    /// Compute the derivative of both sides of an assignment
    /// </summary>
    /// <param name="expr">expression to compute derivative of</param>
    /// <param name="dx">variable to compute derivative with respect to</param>
    /// <returns>derivative</returns>
    public static Assignment Differentiate(this Assignment assignment, Symbol dx) {
        return new DifferentiationVisitor(dx).VisitAssignment(assignment);
    }
}

/// <summary>
/// Derivative expression transformer
/// </summary>
public class DifferentiationVisitor : ExpressionVisitor {

    private Symbol derivativeSymbol;

    public DifferentiationVisitor(Symbol withRespectTo) {
        this.derivativeSymbol = withRespectTo;
    }

    public override IExpression VisitAddition(Addition addition) {
        // f + g -> f` + g`
        return new Addition(this.VisitExpressionNode(addition.Lhs), this.VisitExpressionNode(addition.Rhs));
    }

    public override IExpression VisitSubtraction(Subtraction subtraction) {
        // f - g -> f` - g`
        return new Subtraction(this.VisitExpressionNode(subtraction.Lhs), this.VisitExpressionNode(subtraction.Rhs));
    }

    public override IExpression VisitMultiplication(Multiplication multiplication) {
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

    public override IExpression VisitDivision(Division division) {
        // f/g -> (f` * g − g` * f)/g2
        var f = division.Numerator;
        var g = division.Denominator;
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
            rhs: new Multiplication(
                g,
                g
            )
        );
    }

    public override IExpression VisitExponentiation(Exponentiation exponentiation) {
        // (g(x)^f(x)) = g(x)^(f(x) - 1) * (g(x) * f'(x) * log(g(x)) + f(x) * g'(x))
        var g = exponentiation.Root;
        var f = exponentiation.Power;

        return new Multiplication(
            new Exponentiation(
                g,
                new Subtraction(f, Real.One)
            ),
            new Addition(
                new Multiplication(
                    new Multiplication(
                        g,
                        this.VisitExpressionNode(f)
                    ),
                    new Logarithm(
                        Real.E,
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

    public override IExpression VisitRoot(NthRoot root) {
        return VisitExponentiation(new Exponentiation(root.Radicand, new Division(Real.One, root.Degree)));
    }

    public override IExpression VisitLogarithm(Logarithm logarithm) {
        // log_a(x) -> 1 / (x ln(a))
        var x = logarithm.Argument;
        var a = logarithm.Base;
        return new Division(
            lhs: Real.One,
            rhs: new Multiplication(
                x,
                new Logarithm(Real.E, a)
            )
        );
    }

    public override IExpression VisitFunction(Function func) {
        if (!(func is IDifferentiable)) {
            // Can't figure out the derivative... just mark it as a placeholder
            return new DerivativeExpression(derivativeSymbol, func);
        }
        // f(g(x)) = f`(g(x))*g`(x)
        var f = func;
        var g = func.Argument;

        var df_g = ((IDifferentiable)func).GetDerivative();
        var dg = this.VisitExpressionNode(g);

        return new Multiplication(df_g, dg);
    }

    public override IExpression VisitSymbol(Symbol symbol) {
        if (symbol == this.derivativeSymbol) {
            return Real.One;
        } else {
            return new DerivativeSymbol(symbol); 
        }
    }

    public override IExpression VisitConstant(Real constant) {
        return Real.Zero;
    }

    public override IExpression VisitConstant(Complex constant) {
        return Real.Zero;
    }

    public override IExpression VisitMatrix(Matrix maybeConstant) {
        if (maybeConstant.IsConstant()) {
            return Real.Zero;
        }
        return new DerivativeExpression(derivativeSymbol, maybeConstant);
    }

    public override IExpression VisitUnrecognizedExpression(IExpression expression) {
        if (expression is IntegralExpression integral && integral.WithRespectTo == this.derivativeSymbol) {
            return integral.ExpressionToIntegrate;
        }
        if (expression.IsConstant()) {
            return Real.Zero;
        }
        return new DerivativeExpression(derivativeSymbol, expression);
    }
}

}