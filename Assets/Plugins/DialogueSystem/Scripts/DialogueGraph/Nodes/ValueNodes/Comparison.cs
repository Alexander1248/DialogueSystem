using System;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    public class Comparison : ValueNode
    {
        
        [InputPort(65535, "A")][HideInInspector] public ValueNode a;
        [InputPort(65535, "B")][HideInInspector] public ValueNode b;
        public ComparisonFilter filter;
        public override object GetValue()
        {
            if (a.GetValue() is not IComparable aValue)
                throw new ArgumentException("Value A not comparable!");
            if (b.GetValue() is not IComparable bValue)
                throw new ArgumentException("Value B not comparable!");
            
            var compare = aValue.CompareTo(bValue);
            return filter switch
            {
                ComparisonFilter.Equal => compare == 0,
                ComparisonFilter.NotEqual => compare != 0,
                ComparisonFilter.Much => compare > 0,
                ComparisonFilter.MuchEqual => compare >= 0,
                ComparisonFilter.Less => compare < 0,
                ComparisonFilter.LessEqual => compare <= 0,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    [Serializable]
    public enum ComparisonFilter
    {
        Equal,
        NotEqual,
        Much,
        MuchEqual,
        Less,
        LessEqual
    }
}