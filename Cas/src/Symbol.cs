using System;
using System.Linq;

namespace Qkmaxware.Cas {

public class Symbol : BaseExpression {
    
    public string Identifier {get; protected set;}

    public Symbol() : this(Guid.NewGuid().ToString()) {}

    public Symbol(string name) {
        this.Identifier = name;
    }

    public override BaseExpression When (params Substitution[] substitutions) {
        var replacement = substitutions.Where((sub) => sub.IsSubstitution(this)).Select(sub => sub.Value).FirstOrDefault();
        return replacement ?? this;
    }

    public override BaseExpression Simplify() {
        return this;
    }

    public override bool Equals(object obj) {
        return obj switch {
            Symbol expr => expr.Identifier.Equals(this.Identifier),
            _ => base.Equals(obj)
        };
    }

    public override int GetHashCode() {
        return Identifier.GetHashCode();
    }

    public override string ToString() {
        return Identifier;
    }

    public static SymbolicSubstitution operator == (Symbol symbol, Constant constant) {
        return new SymbolicSubstitution(symbol, constant);
    }

    public static SymbolicSubstitution operator != (Symbol symbol, Constant constant) {
        throw new NotImplementedException();
    }
}

}