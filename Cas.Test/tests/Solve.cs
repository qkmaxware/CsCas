using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Cas;

namespace Cas.Test {

[TestClass]
public class Solve {
    [TestMethod]
    public void TestBaseCases() {
        var x = new Symbol("x");
        var y = new Symbol("y");

        var ans = (x <= 3);

        var expr1 = ans.SolveFor(x);
        Assert.AreEqual(ans, expr1);

        var expr2 = (3 <= x).SolveFor(x);
        Assert.AreEqual(ans, expr2);
        
        var expr3 = (3 <= y).SolveFor(x);
        Assert.IsNull(expr3);
    }

    [TestMethod]
    public void TestRecursiveCase() {
        var x = new Symbol("x");
        var y = new Symbol("y");

        var equation = y <= x + 3;
        var answer = x <= y - 3;

        Assert.AreEqual(answer, equation.SolveFor(x));
    }
}

}
