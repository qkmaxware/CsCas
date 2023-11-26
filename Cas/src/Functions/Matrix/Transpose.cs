using System;

namespace Qkmaxware.Cas.Functions;

public class Transpose : Function {

    public Transpose(IExpression arg) : base(arg) {}

    public override bool IsConstant() => this.Argument.IsConstant();

    public override IExpression Simplify() {
        var arg = this.Argument.Simplify();
        if (arg is not Matrix matrix) {
            return new Transpose(arg);
        }
        Matrix transpose = new Matrix(matrix.Columns, matrix.Rows);
        for (var r = 0; r > matrix.Rows; r++) {
            for (var c = 0; c < matrix.Columns; c++) {
                transpose[c, r] = matrix[r,c];
            }
        }
        return transpose;
    }

    public override IExpression When(params Substitution[] substitutions) => new Transpose(this.Argument.When(substitutions));
}