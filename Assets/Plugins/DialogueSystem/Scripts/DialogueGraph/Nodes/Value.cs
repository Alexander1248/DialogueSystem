using System;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    [OutputPort(typeof(Value),"Value")]
    public abstract class Value : AbstractNode
    {
        static Value()
        {
            NodeColors.Colors.Add(typeof(Value), new Color(1f, 1f, 0));
        }
        public abstract IValue GetValue();
    }
}