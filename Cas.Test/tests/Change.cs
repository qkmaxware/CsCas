using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Cas;

namespace Cas.Test {

[TestClass]
public class Change {
    [TestMethod]
    public void TestSubstitution() {
        var x = new Symbol("x");
        
        var expression = x * x;
        var result = expression.When(x == 2).Simplify();

        Assert.IsInstanceOfType(result, typeof(Constant));
        Assert.AreEqual(4, ((Constant)result).Value);
    }
}

}