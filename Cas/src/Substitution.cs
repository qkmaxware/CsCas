using System;

namespace Qkmaxware.Cas {

public abstract class Substitution {
    public BaseExpression Value {get; private set;}
    public Substitution(BaseExpression value) {
        this.Value = value;
    }
    public abstract bool IsSubstitution(IAlgebraicEntity entity);
}

public class SymbolicSubstitution : Substitution {
    public Symbol Symbol {get; private set;}

    public SymbolicSubstitution(Symbol symbol, Real @for) : base(@for) {
        this.Symbol = symbol;
    }

    public override bool IsSubstitution(IAlgebraicEntity entity) {
        return entity switch  {
            Symbol sym => this.Symbol.Equals(sym),
            _ => false
        };
    }
}

}