using System;
using System.Linq;

namespace Plugins.DialogueSystem.Scripts.Value
{
    public class String : IValue
    {
        private readonly string _value;

        public String(string value)
        {
            _value = value;
        }

        public IValue Cos()
        {
            throw new NotImplementedException();
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
            if (value.Get() is int i)
                return new String(_value + i);
            if (value.Get() is float f)
                return new String(_value + f);
            if (value.Get() is string s)
                return new String(_value + s);
            return null;
        }

        public IValue Sub(IValue value)
        {
            throw new NotImplementedException();
        }

        public IValue Mul(IValue value)
        {
            if (value.Get() is int i)
                return new String(string.Join("", Enumerable.Repeat(_value, i)));
            if (value.Get() is not float f) return null;
            var r = (int)f;
            var str = string.Join("", Enumerable.Repeat(_value, r));
            str += _value[..(int)((f - r) * _value.Length)];
            return new String(str);

        }

        public IValue Div(IValue value)
        {
            throw new NotImplementedException();
        }

        public IValue Pow(IValue value)
        {
            throw new NotImplementedException();
        }

        public IValue Not()
        {
            throw new NotImplementedException();
        }

        public IValue And(IValue value)
        {
            throw new NotImplementedException();
        }

        public IValue Or(IValue value)
        {
            throw new NotImplementedException();
        }
        public IValue Xor(IValue value)
        {
            throw new NotImplementedException();
        }

        public IValue Abs()
        {
            throw new NotImplementedException();
        }

        public IValue Sin()
        {
            throw new NotImplementedException();
        }
    }
}