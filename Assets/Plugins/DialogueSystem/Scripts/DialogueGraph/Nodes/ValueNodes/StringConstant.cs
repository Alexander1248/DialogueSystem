using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using String = Plugins.DialogueSystem.Scripts.Value.String;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    [EditorPath("Value")]
    public class StringConstant : Value
    {
        [SerializeField] private string value;
        public override IValue GetValue()
        {
            return new String(value);
        }
    }
}