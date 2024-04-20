using System;
using System.Collections.Generic;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.BranchChoicers
{
    public class ValueChoicer : BranchChoicer
    {
        [InputPort("Value")]
        [HideInInspector]
        public Value value;
        
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            if (value.GetValue() is not Integer integer)
                throw new ArgumentException("Not integer value!");
            SelectionIndex = (int) integer.Get();
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            
        }
    }
}