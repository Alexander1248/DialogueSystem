using System.Collections.Generic;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.BranchChoicers
{
    public class ConditionChoicer : BranchChoicer
    {
        [InputPort("Conditions")]
        [HideInInspector]
        public List<Value> conditions = new();
        
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            for (var i = 0; i < conditions.Count; i++)
                if (conditions[i].GetValue().Get() is true)
                {
                    SelectionIndex = i;
                    break;
                }
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            
        }
    }
}