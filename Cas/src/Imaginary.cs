using System;

namespace Qkmaxware.Cas {

public class Imaginary : Complex {

    public double Value => this.Imaginary;

    public Imaginary(double value) : base(0, value) { }
    
}

public static class ImaginaryNumberExtensions {
    public static Imaginary i(this double d) => new Imaginary(d);
}

}