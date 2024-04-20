namespace Plugins.DialogueSystem.Scripts.Value
{
    public interface IValue
    {
        public IValue Equals(IValue value);
        public IValue NotEquals(IValue value);
        public IValue MuchThan(IValue value);
        public IValue MuchEqualsThan(IValue value);
        public IValue LessThan(IValue value);
        public IValue LessEqualsThan(IValue value);
        
        public IValue Add(IValue value);
        public IValue Sub(IValue value);
        public IValue Mul(IValue value);
        public IValue Div(IValue value);
        public IValue Pow(IValue value);
        
        public IValue Not();
        public IValue And(IValue value);
        public IValue Or(IValue value);
        public IValue Xor(IValue value);
        
        
        public IValue Abs();
        public IValue Sin();
        public IValue Cos();

        public object Get();
    }
}