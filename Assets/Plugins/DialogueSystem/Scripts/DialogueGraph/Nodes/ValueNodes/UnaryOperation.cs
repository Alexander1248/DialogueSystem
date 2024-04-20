using System;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    [OutputPort(typeof(Value), "Result")]
    public class UnaryOperation : Value
    {
        
        [InputPort("Value")][HideInInspector] public Value a;
        public UnaryOperationType type;
        public override IValue GetValue()
        {
            return type switch
            {
                UnaryOperationType.Not => a.GetValue().Not(),
                UnaryOperationType.Abs => a.GetValue().Abs(),
                UnaryOperationType.Sin => a.GetValue().Sin(),
                UnaryOperationType.Cos => a.GetValue().Cos(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    [Serializable]
    public enum UnaryOperationType
    {
        Not,
        Abs,
        Sin,
        Cos
    }
}