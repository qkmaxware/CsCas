using System;
using System.Linq;

namespace Qkmaxware.Cas {

public class Symbol : BaseExpression, IValueLike {
    
    public string Identifier {get; protected set;}

    public Symbol() : this(Guid.NewGuid().ToString()) {}

    public Symbol(string name) {
        this.Identifier = name;
    }

    public override IExpression When (params Substitution[] substitutions) {
        var replacement = substitutions.Where((sub) => sub.IsSubstitution(this)).Select(sub => sub.Value).FirstOrDefault();
        return replacement ?? this;
    }

    public override IExpression Simplify() {
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

    public override bool IsConstant() => false; // Symbols can be redefined and are therefor not const
    public bool IsZero() => false;

    public static SymbolicSubstitution operator == (Symbol symbol, Real constant) {
        return new SymbolicSubstitution(symbol, constant);
    }

    public static SymbolicSubstitution operator != (Symbol symbol, Real constant) {
        throw new NotImplementedException();
    }
}

}