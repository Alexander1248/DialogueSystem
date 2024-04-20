using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using Boolean = Plugins.DialogueSystem.Scripts.Value.Boolean;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    public class BoolConstant : Value
    {
        [SerializeField] private bool value;
        public override IValue GetValue()
        {
            return new Boolean(value);
        }
    }
}