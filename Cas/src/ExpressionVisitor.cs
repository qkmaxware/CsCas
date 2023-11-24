namespace Qkmaxware.Cas {

public abstract class ExpressionVisitor {

    public abstract IExpression VisitSymbol(Symbol symbol);
    public abstract IExpression VisitConstant(Real constant);
    public abstract IExpression VisitConstant(Complex constant);
    public abstract IExpression VisitMatrix(Matrix constant);
    public abstract IExpression VisitAddition(Addition addition);
    public abstract IExpression VisitSubtraction(Subtraction subtraction);
    public abstract IExpression VisitMultiplication(Multiplication multiplication);
    public abstract IExpression VisitDivision(Division division);
    public abstract IExpression VisitExponentiation(Exponentiation exponentiation);
    public abstract IExpression VisitRoot (NthRoot root);
    public abstract IExpression VisitLogarithm(Logarithm logarithm);
    public abstract IExpression VisitFunction(Function function);

    public virtual IExpression VisitUnrecognizedExpression(IExpression expression) {
        throw new System.ArgumentException("Invalid expression type");
    }
    
    public virtual IExpression VisitExpressionNode(IExpression expression) {
        switch (expression) {
            case Addition bop: {
                return VisitAddition(bop);
            }
            case Subtraction bop : {
                return VisitSubtraction(bop);
            }
            case Multiplication bop : {
                return VisitMultiplication(bop);
            }
            case Division bop : {
                return VisitDivision(bop);
            }
            case Exponentiation bop : {
                return VisitExponentiation(bop);
            }
            case NthRoot root: {
                return VisitRoot(root);
            }
            case Logarithm bop : {  
                return VisitLogarithm(bop);
            }
            case Function func: {
                return VisitFunction(func);
            }
            case Symbol sym: {
                return VisitSymbol(sym);
            }
            case Real @const: {
                return VisitConstant(@const);
            } 
            case Complex com: {
                return VisitConstant(com);
            }
            case Matrix mat: {
                return VisitMatrix(mat);
            }
            default: {
                return VisitUnrecognizedExpression(expression);
            }
        }
    }

    public virtual Assignment VisitAssignment(Assignment assignment) {
        return new Assignment(VisitExpressionNode(assignment.Lhs), VisitExpressionNode(assignment.Rhs));
    }

}

}