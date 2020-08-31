namespace Qkmaxware.Cas {
    
/*
Trigonometry (x is in radians)	sin(x)	cos(x)
                                cos(x)	−sin(x)
                                tan(x)	sec2(x)
Inverse Trigonometry	sin-1(x)	1/√(1−x2)
                        cos-1(x)	−1/√(1−x2)
                        tan-1(x)	1/(1+x2)
*/

public static class Trig {
    public static Functions.Sin Sin(BaseExpression expr) {
        return new Functions.Sin(expr);
    } 

    public static Functions.Cos Cos(BaseExpression expr) {
        return new Functions.Cos(expr);
    } 

    public static Functions.Asin Asin(BaseExpression expr) {
        return new Functions.Asin(expr);
    } 
}

}