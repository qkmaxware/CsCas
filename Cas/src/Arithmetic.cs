using System;
using System.Linq;

namespace Qkmaxware.Cas {

public class Addition : BaseExpression {

    public BaseExpression Lhs {get; private set;}
    public BaseExpression Rhs {get; private set;}

    public Addition(BaseExpression lhs, BaseExpression rhs) {
        this.Lhs = lhs;
        this.Rhs = rhs;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.Value;
        return new Addition(this.Lhs.When(substitutions), this.Rhs.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newLHS = Lhs.Simplify();
        var newRHS = Rhs.Simplify();
        // If both are constants
        if (newLHS is Real constLhs && newRHS is Real constRhs) {
            return new Real(constLhs.Value + constRhs.Value);
        } 
        // If one of them is zero, omit it
        if (newLHS is Real zeroLhs && zeroLhs.Value == 0) {
            return newRHS;
        }
        else if (newRHS is Real zeroRhs && zeroRhs.Value == 0) {
            return newLHS;
        }
        // No simplification
        else {
            return new Addition(newLHS, newRHS);
        }
    }

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

    public BaseExpression Lhs {get; private set;}
    public BaseExpression Rhs {get; private set;}

    public Subtraction(BaseExpression lhs, BaseExpression rhs) {
        this.Lhs = lhs;
        this.Rhs = rhs;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.Value;
        return new Subtraction(this.Lhs.When(substitutions), this.Rhs.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newLHS = Lhs.Simplify();
        var newRHS = Rhs.Simplify();
        // If both are constants
        if (newLHS is Real constLhs && newRHS is Real constRhs) {
            return new Real(constLhs.Value - constRhs.Value);
        } 
        // If rhs is zero, omit it
        if (newRHS is Real zeroRhs && zeroRhs.Value == 0) {
            return newLHS;
        }
        // No simplification
        else {
            return new Subtraction(newLHS, newRHS);
        }
    }

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

    public BaseExpression Lhs {get; private set;}
    public BaseExpression Rhs {get; private set;}

    public Multiplication(BaseExpression lhs, BaseExpression rhs) {
        this.Lhs = lhs;
        this.Rhs = rhs;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.Value;
        return new Multiplication(this.Lhs.When(substitutions), this.Rhs.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newLHS = Lhs.Simplify();
        var newRHS = Rhs.Simplify();
        // If both are constants
        if (newLHS is Real constLhs && newRHS is Real constRhs) {
            return new Real(constLhs.Value * constRhs.Value);
        } 
        // If one of them are zero the product is zero
        if (newLHS is Real zeroLhs && zeroLhs.Value == 0) {
            return Real.Zero;
        }
        else if (newRHS is Real zeroRhs && zeroRhs.Value == 0) {
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

    public BaseExpression Lhs {get; private set;}
    public BaseExpression Rhs {get; private set;}

    public Division(BaseExpression lhs, BaseExpression rhs) {
        this.Lhs = lhs;
        this.Rhs = rhs;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.Value;
        return new Division(this.Lhs.When(substitutions), this.Rhs.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newLHS = Lhs.Simplify();
        var newRHS = Rhs.Simplify();
        // If both are constants
        if (newLHS is Real constLhs && newRHS is Real constRhs) {
            return new Real(constLhs.Value / constRhs.Value);
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

    public override bool Equals(object obj) {
        return obj switch {
            Division bop => (this.Lhs.Equals(bop.Lhs) && this.Rhs.Equals(bop.Rhs)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Lhs, this.Rhs);
    }

    public override string ToString() {
        return $"({Lhs} / {Rhs})";
    }
}

public class Exponentiation : BaseExpression {
    public BaseExpression Root {get; private set;}
    public BaseExpression Power {get; private set;}

    public Exponentiation(BaseExpression root, BaseExpression power) {
        this.Root = root;
        this.Power = power;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.Value;
        return new Exponentiation(this.Root.When(substitutions), this.Power.When(substitutions));
    }

    public override BaseExpression Simplify() {
        var newLHS = Root.Simplify();
        var newRHS = Power.Simplify();
        // If both are constants
        if (newLHS is Real constLhs && newRHS is Real constRhs) {
            return new Real(Math.Pow(constLhs.Value, constRhs.Value));
        } 
        // No simplification
        else {
            return new Exponentiation(newLHS, newRHS);
        }
    }

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

public class Logarithm : BaseExpression  {
    public BaseExpression Base {get; private set;}
    public BaseExpression Argument {get; private set;}

    public Logarithm(BaseExpression @base, BaseExpression argument) {
        this.Base = @base;
        this.Argument = argument;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        var sub = substitutions.Where(sub => sub.IsSubstitution(this)).FirstOrDefault();
        if (sub != null)
            return sub.Value;
        return new Logarithm(this.Base.When(substitutions), this.Argument.When(substitutions));
    }

    public override BaseExpression Simplify() {
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