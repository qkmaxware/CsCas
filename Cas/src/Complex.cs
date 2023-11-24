using System;

namespace Qkmaxware.Cas {

public class Complex : BaseExpression, IValueLike, INumber {
    public double Real {get; private set;}
    public double Imaginary {get; private set;}

    public bool IsPurelyReal => Imaginary == 0;
    public bool IsPurelyImaginary => Real == 0;

    public Complex() {}
    public Complex(double real, double imm = 0) {
        this.Real = real;
        this.Imaginary = imm;
    }

    public override bool IsConstant() => true;
    public bool IsZero() => this.Real == 0 && this.Imaginary == 0;
    public bool IsNaN() => Double.IsNaN(this.Real) || Double.IsNaN(this.Imaginary);
    public bool IsPositiveInfinity() => Double.IsPositiveInfinity(this.Real) || Double.IsPositiveInfinity(this.Imaginary);
    public bool IsNegativeInfinity() => Double.IsNegativeInfinity(this.Real) || Double.IsNegativeInfinity(this.Imaginary);

    public override IExpression Simplify() {
        return this;
    }

    public override IExpression When(params Substitution[] substitutions) {
        return this;
    }

    public override int GetHashCode() {
        return HashCode.Combine(this.Real, this.Imaginary);
    }
    public override bool Equals(object obj) {
        if (obj is Complex complex) {
            return this.Real == complex.Real && this.Imaginary == complex.Imaginary;
        } else {
            return false;
        }
    }

    public override string ToString() {
        if (IsPurelyReal){
            return Real.ToString();
        } else if (IsPurelyImaginary) {
            return Imaginary.ToString()+ "i";
        } else {
            return "(" + Real.ToString() + "+" + Imaginary.ToString()+ "i)";
        }
    }

    /// <summary>
    /// The complex argument
    /// </summary>
    public double Arg(){
        return Math.Atan2(this.Imaginary, this.Real);
    }

    /// <summary>
    /// Complex conjugate
    /// </summary>
    public Complex Conjugate(){
        return new Complex(this.Real, this.Imaginary*-1);
    }

    /// <summary>
    /// Complex reciprocal
    /// </summary>
    public Complex Reciprocal(){
        Complex den = this.Multiply(this.Conjugate());
        return this.Conjugate().Divide(den);
    }

    public bool CanAdd(IValueLike? value) => !ReferenceEquals(value, null) && (value is Real || value is Complex);

    public BaseExpression Add(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));

        return value switch {
            Complex complex => new Complex(this.Real + complex.Real, this.Imaginary + complex.Imaginary),
            _ => throw new ArgumentException("Cannot add a value of type " + value.GetType())
        };
    }

    public bool CanSubtract(IValueLike? value) => !ReferenceEquals(value, null) && (value is Real || value is Complex);

    public BaseExpression Subtract(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));

        return value switch {
            Complex complex => new Complex(this.Real - complex.Real, this.Imaginary - complex.Imaginary),
            _ => throw new ArgumentException("Cannot subtract a value of type " + value.GetType())
        };
    }

    public bool CanMultiply(IValueLike? value) => !ReferenceEquals(value, null) && (value is Real || value is Complex || value is Matrix);

    public BaseExpression Multiply(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));
        
        return value switch {
            Complex complex => this.Multiply(complex),
            Matrix mtx => mtx.ScalarMultiply(this),
            _ => throw new ArgumentException("Cannot multiply a value of type " + value.GetType())
        };
    }

    public bool CanDivide(IValueLike? value) => !ReferenceEquals(value, null) && (value is Real || value is Complex);

    public BaseExpression Divide(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));
        
        return value switch {
            Complex complex => this.Divide(complex),
            _ => throw new ArgumentException("Cannot divide a value of type " + value.GetType())
        };
    }

    public bool CanRaise(IValueLike? value) => !ReferenceEquals(value, null) && (value is Real || value is Complex);
    public BaseExpression Raise(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));
        
        return value switch {
            Complex complex => this.Pow(complex),
            _ => throw new ArgumentException("Cannot raise to the power of a value of type " + value.GetType())
        };
    }
    
    /// <summary>
    /// Multiply this complex number by another
    /// </summary>
    /// <param name="c">number to multiply by</param>
    public Complex Multiply(Complex c){
        return new Complex(this.Real * c.Real - this.Imaginary * c.Imaginary, this.Real * c.Imaginary + this.Imaginary * c.Real);
    }
    
    /// <summary>
    /// Divide this complex number by another
    /// </summary>
    /// <param name="c">number to divide by</param>
    public Complex Divide(Complex c){
        double d = (c.Real*c.Real + c.Imaginary*c.Imaginary);
        double r = (this.Real*c.Real + this.Imaginary*c.Imaginary)/d;
        double i = (this.Imaginary*c.Real - this.Real*c.Imaginary)/d;
        return new Complex(r,i);
    }

    public Complex Pow(Complex exp) {
        //In exponential form 
        //(a+ib)^(c+id) = e^(ln(r)(c+id)+i theta (c+id))
        // -> ln(r)c + ln(r)id + i0c - 0d
        //e^(i theta) = cos0 + isin0
        //e^(ln(r)c - 0d) * e^(i(ln(r)*d + 0c))
        double r = Math.Sqrt(this.Real*this.Real + this.Imaginary*this.Imaginary);
        double theta = this.Arg();
        double lnr = Math.Log(r);
        
        //e^(ln(r)c - 0d)
        double scalar = Math.Pow(Math.E, lnr*exp.Real - theta*exp.Imaginary);
        
        //e^(i(ln(r)*d + 0c)) = e^(i a) = cos(a) + isin(a)
        double real = Math.Cos(lnr*exp.Imaginary + theta*exp.Real);
        double img =  Math.Sin(lnr*exp.Imaginary + theta*exp.Real);
        
        return new Complex(scalar * real, scalar * img);
    }

    public static implicit operator Complex(double d) {
        return new Complex(d);
    }
}

}