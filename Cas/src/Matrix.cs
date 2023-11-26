using System;

namespace Qkmaxware.Cas {

public class DimensionMismatchException : ArithmeticException {
    public DimensionMismatchException() : base() {}
}

public partial class Matrix : BaseExpression, IValueLike, IAdd, ISubtract, IMultiply, IDivide {
    private IExpression[] elements;
    public int Rows {get; private set;}
    public int Columns {get; private set;}
    public bool IsSquare() => Rows == Columns;

    public IExpression this[int row, int col] {
        get => this.elements[col + row * this.Columns];
        set => this.elements[col + row * this.Columns] = value;
    }

    public Matrix(int rows, int columns) {
        this.Rows = rows;
        this.Columns = columns;
        this.elements = new IExpression[this.Rows * this.Columns];
    }

    public Matrix(BaseExpression[,] elements) {
        this.Rows = elements.GetLength(0);
        this.Columns = elements.GetLength(1);
        this.elements = new IExpression[this.Rows * this.Columns];
        for (var row = 0; row < this.Rows; row++) {
            for (var col = 0; col < this.Columns; col++) {
                this[row, col] = elements[row, col];
            }
        }
    }

    public static implicit operator Matrix (BaseExpression[,] elements) => new Matrix(elements);
    public static implicit operator Matrix (int[,] elements) => new Matrix(Transform<int, BaseExpression>(elements, (e) => new Real(e)));
    public static implicit operator Matrix (float[,] elements) => new Matrix(Transform<float, BaseExpression>(elements, (e) => new Real(e)));
    public static implicit operator Matrix (double[,] elements) => new Matrix(Transform<double, BaseExpression>(elements, (e) => new Real(e)));

    private static T2[,] Transform<T1, T2>(T1[,] original, Func<T1, T2> mapping) {
        T2[,] rs = new T2[original.GetLength(0), original.GetLength(1)];
        for (var row = 0; row < original.GetLength(0); row++) {
            for (var col = 0; col < original.GetLength(1); col++) {
                rs[row, col] = mapping(original[row, col]);
            }
        }
        return rs;
    }

    public override bool IsConstant() {
        foreach (var element in this.elements) {
            if (!element.IsConstant())
                return false;
        }
        return true;
    }
    public bool IsZero() => false;

    public override IExpression When(params Substitution[] substitutions) {
        var next = new Matrix(this.Rows, this.Columns);
        for(var i = 0; i < next.elements.Length; i++)
            next.elements[i] = this.elements[i].When(substitutions);
        return next;
    }

    public override IExpression Simplify() {
        var next = new Matrix(this.Rows, this.Columns);
        for(var i = 0; i < next.elements.Length; i++)
            next.elements[i] = this.elements[i].Simplify();
        return next;
    }

    public static void AssertSquare(Matrix mtx) {
        if(!mtx.IsSquare()) {
            throw new DimensionMismatchException();
        }
    }

    public static void AssertSameDimensions(Matrix m1, Matrix m2) {
        if(m1.Rows != m2.Rows || m1.Columns != m2.Columns) {
            throw new DimensionMismatchException();
        }
    }

    public static void AssertCanMultiply(Matrix m1, Matrix m2) {
        if(m1.Columns != m2.Rows) {
            throw new DimensionMismatchException();
        }
    }

    public bool CanAdd(IValueLike? value)=>!ReferenceEquals(value, null) && (value is Matrix m && m.Columns == this.Columns && m.Rows == this.Rows);

    public BaseExpression Add(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));
        
        return value switch {
            Matrix mtx => this.Add(mtx),
            _ => throw new ArgumentException("Cannot add a value of type " + value.GetType())
        };
    }
    public Matrix Add(Matrix other) {
        AssertSameDimensions(this, other);
        Matrix rs = new Matrix(this.Rows, this.Columns);
        for(int i = 0; i < this.Rows; i++) {
            for(int j = 0; j < this.Columns; j++) {
                rs[i,j] = new Addition (this[i,j], (other[i,j]));
            }
        }
        return rs;
    }

    public bool CanSubtract(IValueLike? value)=>!ReferenceEquals(value, null) && (value is Matrix m && m.Columns == this.Columns && m.Rows == this.Rows);

    public BaseExpression Subtract(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));
        
        return value switch {
            Matrix mtx => this.Subtract(mtx),
            _ => throw new ArgumentException("Cannot subtract a value of type " + value.GetType())
        };
    }
    public Matrix Subtract(Matrix other) {
        AssertSameDimensions(this, other);
        Matrix rs = new Matrix(this.Rows, this.Columns);
        for(int i = 0; i < this.Rows; i++) {
            for(int j = 0; j < this.Columns; j++) {
                rs[i,j] = new Subtraction(this[i,j], (other[i,j]));
            }
        }
        return rs;
    }

    public bool CanMultiply(IValueLike? value)
    =>!ReferenceEquals(value, null) 
    && (
        (value is Matrix m && m.Columns == m.Rows)
        || (value is Real)
        || (value is Complex)
    );

    public BaseExpression Multiply(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));
        
        return value switch {
            Complex complex => this.ScalarMultiply(complex),
            Matrix mtx => this.Multiply(mtx),
            _ => throw new ArgumentException("Cannot multiply a value of type " + value.GetType())
        };
    }
    public Matrix Multiply(Matrix other) {
        AssertCanMultiply(this, other);
        int rows = this.Rows;
        int columns = other.Columns;
        var rs = new Matrix(rows,columns);
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                BaseExpression sum = Real.Zero;
                for (int k = 0; k < this.Columns; k++) {
                    if (k == 0) {
                        sum = new Multiplication(this[i,k], (other[k,j])); //Row Column Format
                    } else {
                        sum = new Addition(sum, (new Multiplication(this[i,k], other[k,j]))); //Row Column Format
                    }
                }
                rs[i,j] = sum; //Row Column Format
            }
        }
        return rs;
    }
    public Matrix ScalarMultiply(Real r) {
        Matrix rs = new Matrix(this.Rows, this.Columns);
         for(int i = 0; i < this.Rows; i++) {
            for(int j = 0; j < this.Columns; j++) {
                rs[i,j] = new Multiplication(this[i,j], r);
            }
        }
        return rs;
    }
    public Matrix ScalarMultiply(Complex r) {
        Matrix rs = new Matrix(this.Rows, this.Columns);
         for(int i = 0; i < this.Rows; i++) {
            for(int j = 0; j < this.Columns; j++) {
                rs[i,j] = new Multiplication(this[i,j], r);
            }
        }
        return rs;
    }

    public bool CanDivide(IValueLike? value)
    =>!ReferenceEquals(value, null) 
    && (
        (value is Real)
        || (value is Complex)
    );

    public BaseExpression Divide(IValueLike? value) {
        if (ReferenceEquals(value, null))
            throw new NullReferenceException(nameof(value));
        
        return value switch {
            Complex complex => this.ScalarDivide(complex),
            _ => throw new ArgumentException("Cannot subtract a value of type " + value.GetType())
        };
    }
    public Matrix ScalarDivide(Real r) {
        Matrix rs = new Matrix(this.Rows, this.Columns);
         for(int i = 0; i < this.Rows; i++) {
            for(int j = 0; j < this.Columns; j++) {
                rs[i,j] = new Division(this[i,j] , r);
            }
        }
        return rs;
    }
    public Matrix ScalarDivide(Complex r) {
        Matrix rs = new Matrix(this.Rows, this.Columns);
         for(int i = 0; i < this.Rows; i++) {
            for(int j = 0; j < this.Columns; j++) {
                rs[i,j] = new Division(this[i,j] , r);
            }
        }
        return rs;
    }
}

}