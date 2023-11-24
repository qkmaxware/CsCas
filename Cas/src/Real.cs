using System;

namespace Qkmaxware.Cas {

public class Real : Complex {
    public static readonly Real Zero = new Real(0);
    public static readonly Real Half = new Real(0.5);
    public static readonly Real One = new Real(1);
    public static readonly Real Two = new Real(2);
    public static readonly Real Three = new Real(3);
    public static readonly Real NegativeOne = new Real(-1);
    public static readonly Real E = new Real(System.Math.E);

    public double Value => this.Real;

    public Real(double value) : base(value, 0) { }

    public static implicit operator Real(double d) {
        return new Real(d);
    }
}

}