namespace Qkmaxware.Cas {

public interface IEquation : IMathEntity { 

}

public interface IExpression : IMathEntity { 
    public bool IsConstant();
    public IExpression When (params Substitution[] substitutions);
    public IExpression Simplify();
}

public interface IMathEntity {

}

}