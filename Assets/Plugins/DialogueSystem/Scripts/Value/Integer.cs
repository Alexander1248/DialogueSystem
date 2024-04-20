using System;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.Value
{
    public class Integer : IValue
    {
        private readonly int _value;

        public Integer(int value)
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
                return new Integer(_value + i);
            if (value.Get() is float f)
                return new Integer((int)(_value + f));
            return null;
        }

        public IValue Sub(IValue value)
        {
            if (value.Get() is int i)
                return new Integer(_value - i);
            if (value.Get() is float f)
                return new Integer((int)(_value - f));
            return null;
        }

        public IValue Mul(IValue value)
        {
            if (value.Get() is int i)
                return new Integer(_value * i);
            if (value.Get() is float f)
                return new Integer((int)(_value * f));
            return null;
        }

        public IValue Div(IValue value)
        {
            if (value.Get() is int i)
                return new Integer(_value / i);
            if (value.Get() is float f)
                return new Integer((int)(_value / f));
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
            return new Integer(~_value);
        }

        public IValue And(IValue value)
        {
            if (value is not Integer val)
                throw new ArgumentException("Not boolean!");
            return new Integer(_value & val._value);
        }

        public IValue Or(IValue value)
        {
            if (value is not Integer val)
                throw new ArgumentException("Not boolean!");
            return new Integer(_value | val._value);
        }

        public IValue Xor(IValue value)
        {
            if (value is not Integer val)
                throw new ArgumentException("Not boolean!");
            return new Integer(_value ^ val._value);
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