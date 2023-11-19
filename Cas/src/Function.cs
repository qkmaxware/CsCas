namespace Qkmaxware.Cas {
    
public abstract class Function : BaseExpression { 
    public IExpression Argument {get; private set;}

    public Function(IExpression argument) {
        this.Argument = argument;
    }

    public override bool Equals(object obj) {
        return obj switch {
            Function fn => fn.GetType().Equals(this.GetType()) && fn.Argument.Equals(this.Argument),
            _ => base.Equals(obj)
        };
    }
    
    public override int GetHashCode(){
        return System.HashCode.Combine(this.Argument);
    }

    public override string ToString() {
        return $"{this.GetType().Name}({this.Argument})";
    }
}

}