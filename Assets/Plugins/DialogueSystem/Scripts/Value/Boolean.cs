using System;

namespace Plugins.DialogueSystem.Scripts.Value
{
    public class Boolean : IValue
    {
        private readonly bool _value;

        public Boolean(bool value)
        {
            _value = value;
        }

        public object Get()
        {
            return _value;
        }

        public IValue Equals(IValue value)
        {
            return new Boolean(_value.CompareTo(value.Get()) == 0);
        }

        public IValue NotEquals(IValue value)
        {
            return new Boolean(_value.CompareTo(value.Get()) != 0);
        }

        public IValue MuchThan(IValue value)
        {
            return new Boolean(_value.CompareTo(value.Get()) > 0);
        }

        public IValue MuchEqualsThan(IValue value)
        {
            return new Boolean(_value.CompareTo(value.Get()) >= 0);
        }

        public IValue LessThan(IValue value)
        {
            return new Boolean(_value.CompareTo(value.Get()) < 0);
        }

        public IValue LessEqualsThan(IValue value)
        {
            return new Boolean(_value.CompareTo(value.Get()) <= 0);
        }

        public IValue Add(IValue value)
        {
            return Or(value);
        }

        public IValue Sub(IValue value)
        {
            return Xor(value);
        }

        public IValue Mul(IValue value)
        {
            return And(value);
        }

        public IValue Div(IValue value)
        {
            return And(value);
        }

        public IValue Pow(IValue value)
        {
            return this;
        }

        public IValue Not()
        { 
            return new Boolean(!_value);
        }

        public IValue And(IValue value)
        {
            if (value is not Boolean boolean)
                throw new ArgumentException("Not boolean!");
            return new Boolean(_value && boolean._value);
        }

        public IValue Or(IValue value)
        {
            if (value is not Boolean boolean)
                throw new ArgumentException("Not boolean!");
            return new Boolean(_value || boolean._value);
        }

        public IValue Xor(IValue value)
        {
            if (value is not Boolean boolean)
                throw new ArgumentException("Not boolean!");
            return new Boolean(_value ^ boolean._value);
        }

        public IValue Abs()
        {
            throw new NotImplementedException();
        }

        public IValue Sin()
        {
            throw new NotImplementedException();
        }

        public IValue Cos()
        {
            throw new NotImplementedException();
        }
    }
}