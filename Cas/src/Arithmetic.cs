using System;
using System.Linq;

namespace Qkmaxware.Cas {

public class Addition : BaseExpression {

    public IExpression Lhs {get; private set;}
    public IExpression Rhs {get; private set;}

    public Addition(IExpression lhs, IExpression rhs) {
        this.Lhs = lhs;
        this.Rhs = rhs;
    }

    public override IExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.GetReplacement();
        return new Addition(this.Lhs.When(substitutions), this.Rhs.When(substitutions));
    }

    public override IExpression Simplify() {
        var newLHS = Lhs.Simplify();
        var newRHS = Rhs.Simplify();
        // If both are constants
        if (newLHS is IAdd constLhs && newRHS is IValueLike constRhs && constLhs.CanAdd(constRhs)) {
            return constLhs.Add(constRhs);
        } 
        // If one of them is zero, omit it
        if (newLHS is IValueLike zeroLhs && zeroLhs.IsZero()) {
            return newRHS;
        }
        else if (newRHS is IValueLike zeroRhs && zeroRhs.IsZero()) {
            return newLHS;
        }
        // No simplification
        else {
            return new Addition(newLHS, newRHS);
        }
    }

    public override bool IsConstant() => this.Lhs.IsConstant() && this.Rhs.IsConstant();

    public override bool Equals(object obj) {
        return obj switch {
            Addition bop => (this.Lhs.Equals(bop.Lhs) && this.Rhs.Equals(bop.Rhs)) || (this.Lhs.Equals(bop.Rhs) && this.Rhs.Equals(bop.Lhs)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Lhs, this.Rhs);
    }

    public override string ToString() {
        return $"({Lhs} + {Rhs})";
    }
}

public class Subtraction: BaseExpression {

    public IExpression Lhs {get; private set;}
    public IExpression Rhs {get; private set;}

    public Subtraction(IExpression lhs, IExpression rhs) {
        this.Lhs = lhs;
        this.Rhs = rhs;
    }

    public override IExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.GetReplacement();
        return new Subtraction(this.Lhs.When(substitutions), this.Rhs.When(substitutions));
    }

    public override IExpression Simplify() {
        var newLHS = Lhs.Simplify();
        var newRHS = Rhs.Simplify();
        // If both are constants
        if (newLHS is ISubtract constLhs && newRHS is IValueLike constRhs && constLhs.CanSubtract(constRhs)) {
            return constLhs.Subtract(constRhs);
        } 
        // If rhs is zero, omit it
        if (newRHS is IValueLike zeroRhs && zeroRhs.IsZero()) {
            return newLHS;
        }
        // No simplification
        else {
            return new Subtraction(newLHS, newRHS);
        }
    }

    public override bool IsConstant() => this.Lhs.IsConstant() && this.Rhs.IsConstant();

    public override bool Equals(object obj) {
        return obj switch {
            Subtraction bop => (this.Lhs.Equals(bop.Lhs) && this.Rhs.Equals(bop.Rhs)) || (this.Lhs.Equals(bop.Rhs) && this.Rhs.Equals(bop.Lhs)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Lhs, this.Rhs);
    }

    public override string ToString() {
        return $"({Lhs} - {Rhs})";
    }
}

public class Multiplication : BaseExpression {

    public IExpression Lhs {get; private set;}
    public IExpression Rhs {get; private set;}

    public Multiplication(IExpression lhs, IExpression rhs) {
        this.Lhs = lhs;
        this.Rhs = rhs;
    }

    public override IExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.GetReplacement();
        return new Multiplication(this.Lhs.When(substitutions), this.Rhs.When(substitutions));
    }

    public override bool IsConstant() => this.Lhs.IsConstant() && this.Rhs.IsConstant();

    public override IExpression Simplify() {
        var newLHS = Lhs.Simplify();
        var newRHS = Rhs.Simplify();
        // If both are constants 
        if (newLHS is IMultiply constLhs && newRHS is IValueLike constRhs && constLhs.CanMultiply(constRhs)) {
            return constLhs.Multiply(constRhs);
        } 
        // If one of them are zero the product is zero
        if (newLHS is IValueLike zeroLhs && zeroLhs.IsZero()) {
            return Real.Zero;
        }
        else if (newRHS is IValueLike zeroRhs && zeroRhs.IsZero()) {
            return Real.Zero;
        }
        // No simplification
        else {
            return new Multiplication(newLHS, newRHS);
        }
    }

    public override bool Equals(object obj) {
        return obj switch {
            Multiplication bop => (this.Lhs.Equals(bop.Lhs) && this.Rhs.Equals(bop.Rhs)) || (this.Lhs.Equals(bop.Rhs) && this.Rhs.Equals(bop.Lhs)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Lhs, this.Rhs);
    }

    public override string ToString() {
        return $"({Lhs} · {Rhs})";
    }
}

public class Division : BaseExpression {

    public IExpression Numerator {get; private set;}
    public IExpression Denominator {get; private set;}

    public Division(IExpression lhs, IExpression rhs) {
        this.Numerator = lhs;
        this.Denominator = rhs;
    }

    public override IExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.GetReplacement();
        return new Division(this.Numerator.When(substitutions), this.Denominator.When(substitutions));
    }

    public override IExpression Simplify() {
        var newLHS = Numerator.Simplify();
        var newRHS = Denominator.Simplify();
        // If both are constants
        if (newLHS is IDivide constLhs && newRHS is IValueLike constRhs && constLhs.CanDivide(constRhs)) {
            return constLhs.Divide(constRhs);
        } 
        // Cancellation of similar terms
        else if (newLHS is Multiplication top1 && newRHS is Multiplication bottom1 && top1.Lhs == bottom1.Lhs) {
            return new Division(top1.Rhs, bottom1.Rhs) ;
        }
        else if (newLHS is Multiplication top2 && newRHS is Multiplication bottom2 && top2.Rhs == bottom2.Rhs) {
            return new Division(top2.Lhs, bottom2.Lhs) ;
        }
        else if (newLHS is Multiplication top3 && newRHS is Multiplication bottom3 && top3.Lhs == bottom3.Rhs) {
            return new Division(top3.Rhs, bottom3.Lhs) ;
        }
        else if (newLHS is Multiplication top4 && newRHS is Multiplication bottom4 && top4.Rhs == bottom4.Lhs) {
            return new Division(top4.Lhs, bottom4.Rhs) ;
        }
        // No simplification
        else {
            return new Division(newLHS, newRHS);
        }
    }

    public override bool IsConstant() => this.Numerator.IsConstant() && this.Denominator.IsConstant();

    public override bool Equals(object obj) {
        return obj switch {
            Division bop => (this.Numerator.Equals(bop.Numerator) && this.Denominator.Equals(bop.Denominator)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Numerator, this.Denominator);
    }

    public override string ToString() {
        return $"({Numerator} / {Denominator})";
    }
}

public class Exponentiation : BaseExpression {
    public IExpression Root {get; private set;}
    public IExpression Power {get; private set;}

    public Exponentiation(IExpression root, IExpression power) {
        this.Root = root;
        this.Power = power;
    }

    public override IExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.GetReplacement();
        return new Exponentiation(this.Root.When(substitutions), this.Power.When(substitutions));
    }

    public override IExpression Simplify() {
        var newLHS = Root.Simplify();
        var newRHS = Power.Simplify();
        // If both are constants
        if (newLHS is Complex constLhs && newRHS is Complex constRhs) {
            return constLhs.Pow(constRhs);
        } 
        // No simplification
        else {
            return new Exponentiation(newLHS, newRHS);
        }
    }

    public override bool IsConstant() => this.Root.IsConstant() && this.Power.IsConstant();

    public override bool Equals(object obj) {
        return obj switch {
            Exponentiation bop => (this.Root.Equals(bop.Root) && this.Power.Equals(bop.Power)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Root, this.Power);
    }

    public override string ToString() {
        return $"({Root} ^ {Power})";
    }
}

public class NthRoot : BaseExpression {
    public IExpression Degree {get; private set;}
    public IExpression Radicand {get; private set;}

    public NthRoot(IExpression degree, IExpression radicand) {
        this.Degree = degree;
        this.Radicand = radicand;
    }

    public override IExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.GetReplacement();
        return new Exponentiation(this.Degree.When(substitutions), this.Radicand.When(substitutions));
    }

    public override IExpression Simplify() {
        var newLHS = Degree.Simplify();
        var newRHS = Radicand.Simplify();
        // If both are constants
        if (newLHS is Complex constLhs && newRHS is Complex constRhs) {
            return constRhs.Pow(Real.One.Divide(constLhs));
        } 
        // No simplification
        else {
            return new NthRoot(newLHS, newRHS);
        }
    }

    public override bool IsConstant() => this.Degree.IsConstant() && this.Radicand.IsConstant();

    public override bool Equals(object obj) {
        return obj switch {
            NthRoot bop => (this.Degree.Equals(bop.Degree) && this.Radicand.Equals(bop.Radicand)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Degree, this.Radicand);
    }

    public override string ToString() {
        return $"({Radicand} ^ 1/{Degree})";
    }
}

public class Logarithm : BaseExpression  {
    public IExpression Base {get; private set;}
    public IExpression Argument {get; private set;}

    public Logarithm(IExpression @base, IExpression argument) {
        this.Base = @base;
        this.Argument = argument;
    }

    public override IExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.GetReplacement();
        return new Logarithm(this.Base.When(substitutions), this.Argument.When(substitutions));
    }

    public override IExpression Simplify() {
        var newLHS = Base.Simplify();
        var newRHS = Argument.Simplify();
        // If both are constants
        if (newLHS is Real constLhs && newRHS is Real constRhs) {
            return new Real(Math.Log(constRhs.Value, constLhs.Value));
        } 
        // No simplification
        else {
            return new Logarithm(newLHS, newRHS);
        }
    }

    public override bool IsConstant() => this.Base.IsConstant() && this.Argument.IsConstant();

    public override bool Equals(object obj) {
        return obj switch {
            Logarithm bop => (this.Base.Equals(bop.Base) && this.Argument.Equals(bop.Argument)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Base, this.Argument);
    }

    public override string ToString() {
        return $"log_{{{Base}}}({Argument})";
    }
}

}