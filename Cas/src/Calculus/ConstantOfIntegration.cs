using System;

namespace Qkmaxware.Cas.Calculus {

/// <summary>
/// A symbol representing some kind of constant of integration
/// </summary>
public class ConstantOfIntegration : Symbol {
    private string guid; 
    
    public ConstantOfIntegration() : base() {
        this.guid = this.Identifier;
        this.Identifier = "C" + this.Identifier;
    }

    public override int GetHashCode() => base.GetHashCode();
    public override bool Equals(object obj) {
        return obj switch {
            ConstantOfIntegration C => true,
            Symbol s => s.Identifier == this.Identifier,
            _ => base.Equals(obj)
        };
    }
}

}