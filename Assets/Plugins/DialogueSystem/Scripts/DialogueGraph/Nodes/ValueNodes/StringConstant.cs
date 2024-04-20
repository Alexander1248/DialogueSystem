using System;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using Boolean = Plugins.DialogueSystem.Scripts.Value.Boolean;
using String = Plugins.DialogueSystem.Scripts.Value.String;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    public class StringConstant : Value
    {
        [SerializeField] private string value;
        public override IValue GetValue()
        {
            return new String(value);
        }
    }
}