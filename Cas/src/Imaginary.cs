using System;

namespace Qkmaxware.Cas {

public class Imaginary : Complex {

    public static readonly Imaginary I = new Cas.Imaginary(1);

    public double Value => this.Imaginary;

    public Imaginary(double value) : base(0, value) { }
    
}

public static class ImaginaryNumberExtensions {
    public static Imaginary i(this double d) => new Imaginary(d);
}

}