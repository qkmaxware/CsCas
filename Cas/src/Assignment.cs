using System;
using System.Reflection;

namespace Qkmaxware.Cas {

public class Assignment: IEquation {
    public IExpression Lhs {get; private set;}
    public IExpression Rhs {get; private set;}

    public Assignment(IExpression lhs, IExpression rhs) {
        this.Lhs = lhs;
        this.Rhs = rhs;
    }

    public Assignment When (params Substitution[] substitutions) {
        return new Assignment(this.Lhs.When(substitutions), this.Rhs.When(substitutions));
    }

    public Assignment Simplify() {
        return new Assignment(this.Lhs.Simplify(), this.Rhs.Simplify());
    }

    private Function GetInverse(Function fn, IExpression arg) {
        if (!(fn is IInvertable)) {
            throw new System.ArgumentException($"No known inverse for function '{fn.GetType().Name}'");
        }
        return ((IInvertable)fn).GetInverseWithArg(arg);
    }

    public Assignment? SolveFor(Symbol symbol) {
        // Do base cases (already rearranged for one of the symbol instances)
        if (Lhs.Equals(symbol)) {
            return this;
        } else if (Rhs.Equals(symbol)) {
            return new Assignment(this.Rhs, this.Lhs);
        }
        // Do recursive case
        // Re-write based on possible algebraic inverses for this given tree
        // https://www.cs.utexas.edu/users/novak/algebra.pdf
        Assignment? solved = null;

        // LHS Operation & Inverse(s)
        switch (this.Lhs) {
            case Addition bop: {
                /*
                    bop.lhs + bop.rhs = this.rhs
                    bop.lhs = this.rhs - bop.rhs
                    bop.rhs = this.rhs - bop.lhs
                */
                var try1 = new Assignment(bop.Lhs, new Subtraction(this.Rhs, bop.Rhs));
                var try2 = new Assignment(bop.Rhs, new Subtraction(this.Rhs, bop.Lhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Subtraction bop : {
                /*
                    bop.lhs - bop.rhs = this.rhs
                    bop.lhs = this.rhs + bop.rhs
                    bop.rhs = bop.lhs - this.rhs
                */
                var try1 = new Assignment(bop.Lhs, new Addition(this.Rhs, bop.Rhs));
                var try2 = new Assignment(bop.Rhs, new Subtraction(bop.Lhs, this.Rhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Multiplication bop : {
                /*
                    bop.lhs * bop.rhs = this.rhs
                    bop.lhs = this.rhs / bop.rhs
                    bop.rhs = this.rhs / bop.lhs
                */
                var try1 = new Assignment(bop.Lhs, new Division(this.Rhs, bop.Rhs));
                var try2 = new Assignment(bop.Rhs, new Division(this.Rhs, bop.Lhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Division bop : {
                /*
                    bop.lhs / bop.rhs = this.rhs
                    bop.lhs = this.rhs * bop.rhs
                    bop.rhs = bop.lhs / this.rhs
                */
                var try1 = new Assignment(bop.Lhs, new Multiplication(this.Rhs, bop.Rhs));
                var try2 = new Assignment(bop.Rhs, new Division(bop.Lhs, this.Rhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Exponentiation bop : {
                /*
                    bop.lhs ^ bop.rhs = this.rhs
                    bop.lhs = bop.rhs √ this.rhs  =>  this.rhs ^ (1/bop.rhs)
                    bop.rhs = log_{bop.lhs}(this.rhs)
                */
                var try1 = new Assignment(bop.Root, new Exponentiation(this.Rhs, new Division(Real.One, bop.Power)));
                var try2 = new Assignment(bop.Power, new Logarithm(bop.Root, this.Rhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Logarithm log : {
                /*
                    log_{bop.base}(bop.arg) = this.rhs
                    bop.base = this.rhs √ bop.arg  => bop.arg ^ (1/this.rhs)
                    bop.arg = bop.base ^ this.rhs
                */
                var try1 = new Assignment(log.Base, new Exponentiation(log.Argument, new Division(Real.One, this.Rhs)));
                var try2 = new Assignment(log.Argument, new Exponentiation(log.Base, this.Rhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Function fn : {
                /*
                  fn(arg) = this.rhs => arg = fn^-1(this.rhs)
                */
                var inverse = GetInverse(fn, this.Rhs);
            
                var try1 = new Assignment(fn.Argument, inverse);
                solved = solved ?? try1.SolveFor(symbol);
                break;
            }
            default: {
                break; // DO nothing... no recursive case
            }
        }

        // RHS Operation & Inverse(s)
        switch (this.Rhs) {
            case Addition bop: {
                /*
                    this.lhs = bop.lhs + bop.rhs
                    bop.lhs = this.lhs - bop.rhs
                    bop.rhs = this.lhs - bop.lhs
                */
                var try1 = new Assignment(bop.Lhs, new Subtraction(this.Lhs, bop.Rhs));
                var try2 = new Assignment(bop.Rhs, new Subtraction(this.Lhs, bop.Lhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Subtraction bop : {
                /*
                    this.lhs = bop.lhs - bop.rhs
                    bop.lhs = this.lhs + bop.rhs
                    bop.rhs = bop.lhs - this.lhs
                */
                var try1 = new Assignment(bop.Lhs, new Addition(this.Lhs, bop.Rhs));
                var try2 = new Assignment(bop.Rhs, new Subtraction(bop.Lhs, this.Lhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Multiplication bop : {
                /*
                    this.lhs = bop.lhs * bop.rhs
                    bop.lhs = this.lhs / bop.rhs
                    bop.rhs = this.lhs / bop.lhs
                */
                var try1 = new Assignment(bop.Lhs, new Division(this.Lhs, bop.Rhs));
                var try2 = new Assignment(bop.Rhs, new Division(this.Lhs, bop.Lhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Division bop : {
                /*
                    this.lhs = bop.lhs / bop.rhs
                    bop.lhs = this.lhs * bop.rhs
                    bop.rhs = bop.lhs / this.lhs
                */
                var try1 = new Assignment(bop.Lhs, new Multiplication(this.Lhs, bop.Rhs));
                var try2 = new Assignment(bop.Rhs, new Division(bop.Lhs, this.Lhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Exponentiation bop : {
                /*
                    this.lhs = bop.lhs ^ bop.rhs
                    bop.lhs = bop.rhs √ this.lhs  =>  this.lhs ^ (1/bop.rhs)
                    bop.rhs = log_{bop.lhs}(this.lhs)
                */
                var try1 = new Assignment(bop.Root, new Exponentiation(this.Lhs, new Division(Real.One, bop.Power)));
                var try2 = new Assignment(bop.Power, new Logarithm(bop.Root, this.Lhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Logarithm log : {
                /*
                    this.lhs = log_{bop.base}(bop.arg)
                    bop.base = this.lhs √ bop.arg  => bop.arg ^ (1/this.lhs)
                    bop.arg = bop.base ^ this.lhs
                */
                var try1 = new Assignment(log.Base, new Exponentiation(log.Argument, new Division(Real.One, this.Lhs)));
                var try2 = new Assignment(log.Argument, new Exponentiation(log.Base, this.Lhs));
                solved = solved ?? try1.SolveFor(symbol) ?? try2.SolveFor(symbol);
                break;
            }
            case Function fn : {
                /*
                  this.lhs = fn(arg) => fn^-1(this.lhs) = arg
                */
                var inverse = GetInverse(fn, this.Lhs);
            
                var try1 = new Assignment(inverse, fn.Argument);
                solved = solved ?? try1.SolveFor(symbol);
                break;
            }
            default: {
                break; // DO nothing... no recursive case
            }
        }

        // Return solved expression or null
        return solved;
    }

    public double? Value {
        get {
            if (this.Rhs is Real value) {
                return value.Value;
            } else {
                return null;
            }
        }
    }

    public static explicit operator double(Assignment assignment) {
        var value = assignment.Value;
        return value == null || !value.HasValue ? default(double) : value.Value;
    } 

    public override bool Equals(object obj) {
        return obj switch {
            Assignment ass => (this.Lhs.Equals(ass.Lhs) && this.Rhs.Equals(ass.Rhs)) || (this.Lhs.Equals(ass.Rhs) && this.Rhs.Equals(ass.Lhs)),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Lhs, this.Rhs);
    }

    public override string ToString() {
        return $"{Lhs} = {Rhs}";
    }
}

}