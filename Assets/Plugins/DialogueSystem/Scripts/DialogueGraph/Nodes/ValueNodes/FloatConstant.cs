using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    [EditorPath("Value")]
    public class FloatConstant : Value
    {
        [SerializeField] private float value;
        public override IValue GetValue()
        {
            return new Float(value);
        }
    }
}