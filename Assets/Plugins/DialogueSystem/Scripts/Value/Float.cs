using System;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.Value
{
    public class Float : IValue
    {
        private readonly float _value;

        public Float(float value)
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
            if (value.Get() is int i)
                return new Float(_value + i);
            if (value.Get() is float f)
                return new Float(_value + f);
            return null;
        }

        public IValue Sub(IValue value)
        {
            if (value.Get() is int i)
                return new Float(_value - i);
            if (value.Get() is float f)
                return new Float(_value - f);
            return null;
        }

        public IValue Mul(IValue value)
        {
            if (value.Get() is int i)
                return new Float(_value * i);
            if (value.Get() is float f)
                return new Float(_value * f);
            return null;
        }

        public IValue Div(IValue value)
        {
            if (value.Get() is int i)
                return new Float(_value / i);
            if (value.Get() is float f)
                return new Float(_value / f);
            return null;
        }

        public IValue Pow(IValue value)
        { 
            if (value.Get() is int i)
                return new Float(Mathf.Pow(_value, i));
            if (value.Get() is float f)
                return new Float(Mathf.Pow(_value, f));
            return null;
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
            return new Float(Mathf.Abs(_value));
        }

        public IValue Sin()
        {
            return new Float(Mathf.Sin(_value));
        }

        public IValue Cos()
        {
            return new Float(Mathf.Cos(_value));
        }
    }
}