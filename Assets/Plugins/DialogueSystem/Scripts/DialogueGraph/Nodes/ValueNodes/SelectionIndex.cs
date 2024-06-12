using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    [EditorPath("Value")]
    public class SelectionIndex : Value
    {
        [FormerlySerializedAs("choicer")]
        [InputPort]
        [HideInInspector]
        public BranchChooser chooser;
        public override IValue GetValue()
        {
            return new Integer(chooser.SelectionIndex);
        }
    }
}