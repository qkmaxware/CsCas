namespace Qkmaxware.Cas {

public abstract class ExpressionVisitor {

    public abstract BaseExpression VisitSymbol(Symbol symbol);
    public abstract BaseExpression VisitContant(Real constant);
    public abstract BaseExpression VisitAddition(Addition addition);
    public abstract BaseExpression VisitSubtraction(Subtraction subtraction);
    public abstract BaseExpression VisitMultiplication(Multiplication multiplication);
    public abstract BaseExpression VisitDivision(Division division);
    public abstract BaseExpression VisitExponentiation(Exponentiation exponentiation);
    public abstract BaseExpression VisitLogarithm(Logarithm logarithm);
    public abstract BaseExpression VisitFunction(Function function);
    
    public virtual BaseExpression VisitExpressionNode(BaseExpression expression) {
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
                return VisitContant(@const);
            } 
            default: {
                throw new System.ArgumentException("Invalid expression type");
            }
        }
    }

    public virtual Assignment VisitAssignment(Assignment assignment) {
        return new Assignment(VisitExpressionNode(assignment.Lhs), VisitExpressionNode(assignment.Rhs));
    }

}

}