using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    [EditorPath("Value")]
    public class IntegerConstant : Value
    {
        [SerializeField] private int value;
        public override IValue GetValue()
        {
            return new Integer(value);
        }
    }
}