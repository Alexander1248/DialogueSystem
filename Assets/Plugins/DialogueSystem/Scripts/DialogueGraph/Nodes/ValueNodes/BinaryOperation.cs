using System;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    [OutputPort(typeof(Value), "Result")]
    public class BinaryOperation : Value
    {
        
        [InputPort("A")][HideInInspector] public Value a;
        [InputPort("B")][HideInInspector] public Value b;
        public BinaryOperationType type;
        public override IValue GetValue()
        {
            return type switch
            {
                BinaryOperationType.Equal => a.GetValue().Equals(b.GetValue()),
                BinaryOperationType.NotEqual => a.GetValue().NotEquals(b.GetValue()),
                BinaryOperationType.Much => a.GetValue().MuchThan(b.GetValue()),
                BinaryOperationType.MuchEqual => a.GetValue().MuchEqualsThan(b.GetValue()),
                BinaryOperationType.Less => a.GetValue().LessThan(b.GetValue()),
                BinaryOperationType.LessEqual => a.GetValue().LessEqualsThan(b.GetValue()),
                BinaryOperationType.Add => a.GetValue().Add(b.GetValue()),
                BinaryOperationType.Sub => a.GetValue().Sub(b.GetValue()),
                BinaryOperationType.Mul => a.GetValue().Mul(b.GetValue()),
                BinaryOperationType.Div => a.GetValue().Div(b.GetValue()),
                BinaryOperationType.Pow => a.GetValue().Pow(b.GetValue()),
                BinaryOperationType.And => a.GetValue().And(b.GetValue()),
                BinaryOperationType.Or => a.GetValue().Or(b.GetValue()),
                BinaryOperationType.Xor => a.GetValue().Xor(b.GetValue()),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    [Serializable]
    public enum BinaryOperationType
    {
        Equal,
        NotEqual,
        Much,
        MuchEqual,
        Less,
        LessEqual,
        
        Add,
        Sub,
        Mul,
        Div,
        Pow,
        
        And,
        Or,
        Xor
    }
}