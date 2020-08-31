using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Cas;
using Qkmaxware.Cas.Calculus;

namespace Cas.Test {

[TestClass]
public class Derivative {

    [TestMethod]
    public void TestMultiplicationByConstant() {
        Symbol y = new Symbol("y");
        Symbol x = new Symbol("x");

        var expr = y <= 4 * x;
        var ddx = new DerivativeSymbol(y) <= 4;

        Assert.AreEqual(ddx, expr.Differentiate(x).Simplify());
    }

    [TestMethod]
    public void TestPowerRule() {
        Symbol y = new Symbol("y");
        Symbol x = new Symbol("x");

        var expr = y <= (x^2);
        var ddx = new DerivativeSymbol(y) <= 2 * (x^1);

        Assert.AreEqual(ddx, expr.Differentiate(x).Simplify());
    }

    [TestMethod]
    public void TestSumRule() {
        Symbol y = new Symbol("y");
        Symbol x = new Symbol("x");

        var expr = y <= x + 5;
        var ddx = new DerivativeSymbol(y) <= 1;

        Assert.AreEqual(ddx, expr.Differentiate(x).Simplify());
    }

    [TestMethod]
    public void TestDifferenceRule() {
        Symbol y = new Symbol("y");
        Symbol x = new Symbol("x");

        var expr = y <= x - 5;
        var ddx = new DerivativeSymbol(y) <= 1;

        Assert.AreEqual(ddx, expr.Differentiate(x).Simplify());
    }

    [TestMethod]
    public void TestProductRule() {
        Symbol z = new Symbol("z");
        Symbol y = new Symbol("y");
        Symbol x = new Symbol("x");

        var expr = y <= x * z;
        var ddx = new DerivativeSymbol(y) <= 1 * z + x * new DerivativeSymbol(z);
        
        Assert.AreEqual(ddx, expr.Differentiate(x).Simplify());
    }

    [TestMethod]
    public void TestQuotientRule() {
        Symbol z = new Symbol("z");
        Symbol y = new Symbol("y");
        Symbol x = new Symbol("x");

        var expr = y <= x / z;
        var ddx = new DerivativeSymbol(y) <= (1 * z - new DerivativeSymbol(z) * x) / (z * z);

        Assert.AreEqual(ddx, expr.Differentiate(x).Simplify());
    }

    [TestMethod]
    public void TestChainRule() {
        Symbol y = new Symbol("y");
        Symbol x = new Symbol("x");

        var expr = y <= Trig.Sin(x);
        var dx = new DerivativeSymbol(y) <= Trig.Cos(x) * 1;

        Assert.AreEqual(dx, expr.Differentiate(x).Simplify());
    }

}

}