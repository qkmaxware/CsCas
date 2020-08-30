namespace Qkmaxware.Cas {

public interface IAlgebraicEntity {
    BaseExpression When (params Substitution[] substitutions);
    BaseExpression Simplify();
}

}