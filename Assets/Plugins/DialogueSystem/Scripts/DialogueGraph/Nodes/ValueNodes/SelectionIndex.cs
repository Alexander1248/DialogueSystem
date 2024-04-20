using System;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using String = Plugins.DialogueSystem.Scripts.Value.String;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    public class SelectionIndex : Value
    {
        [InputPort]
        [HideInInspector]
        public BranchChoicer choicer;
        public override IValue GetValue()
        {
            return new Integer(choicer.SelectionIndex);
        }
    }
}