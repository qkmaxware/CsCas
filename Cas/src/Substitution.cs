using System;

namespace Qkmaxware.Cas {

public abstract class Substitution {
    public abstract bool IsSubstitution(IExpression entity);
    public abstract IExpression GetReplacement();
}

public class SymbolicSubstitution : Substitution {
    public Symbol Symbol {get; private set;}
    public IExpression Replacement {get; private set;}

    public SymbolicSubstitution(Symbol symbol, IExpression @for) : base() {
        this.Symbol = symbol;
        this.Replacement = @for;
    }

    public override bool IsSubstitution(IExpression entity) {
        return entity switch  {
            Symbol sym => this.Symbol.Equals(sym),
            _ => false
        };
    }

    public override IExpression GetReplacement() => Replacement;
}

}