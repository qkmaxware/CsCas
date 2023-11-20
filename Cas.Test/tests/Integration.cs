using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Cas;
using Qkmaxware.Cas.Calculus;
using Qkmaxware.Cas.Functions;

namespace Cas.Test {

[TestClass]
public class Integration {
    // https://www.mathsisfun.com/calculus/integration-rules.html
    [TestMethod]
    public void TestConstant() {
        var x = new Symbol("x");
        var a = new Real(4);

        IExpression expr = a;
        IExpression intg = expr.Integrate(x); 
        IExpression res =  a*x + new ConstantOfIntegration();

        Assert.AreEqual(res, intg);
    }
    [TestMethod]
    public void TestVariable() {
        var x = new Symbol("x");

        IExpression expr = x;
        IExpression intg = expr.Integrate(x); 
        IExpression res =  (x^2)/2 + new ConstantOfIntegration();

        Assert.AreEqual(res, intg);
    }
    [TestMethod]
    public void TestReciprocal() {
        var x = new Symbol("x");

        IExpression expr = 1/x;
        IExpression intg = expr.Integrate(x); 
        IExpression res =  Log.Ln(x) + new ConstantOfIntegration();

        Assert.AreEqual(res, intg);
    }
    [TestMethod]
    public void TestTrig() {
        var x = new Symbol("x");

        IExpression expr = new Cos(x);
        IExpression intg = expr.Integrate(x); 
        IExpression res =  new Sin(x) + new ConstantOfIntegration();
        Assert.AreEqual(res, intg);

        expr = new Sin(x);
        intg = expr.Integrate(x);
        res = new Multiplication(Real.NegativeOne, new Cos(x)) + new ConstantOfIntegration();
        Assert.AreEqual(res, intg);

        expr = new Tan(x);
        intg = expr.Integrate(x);
        res = Real.NegativeOne * Log.Ln(Trig.Cos(x)) + new ConstantOfIntegration(); // https://www.wolframalpha.com/input?i=integrate+tan%28x%29+dx
        Assert.AreEqual(res, intg);
    }
}

}