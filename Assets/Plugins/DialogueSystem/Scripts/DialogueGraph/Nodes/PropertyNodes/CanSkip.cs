using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.PropertyNodes
{
    [EditorPath("Property")]
    public class CanSkip : Property
    {
        [SerializeField] private Stage stage;
        [SerializeField] private bool canSkip;
        
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
            if (stage != Stage.OnDrawStart) return;
            dialogue.canSkip = canSkip;
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDrawEnd) return;
            dialogue.canSkip = canSkip;
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayStart) return;
            dialogue.canSkip = canSkip;
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayEnd) return;
            dialogue.canSkip = canSkip;
        }
        public override AbstractNode Clone()
        {
            var clone = Instantiate(this);
            clone.stage = stage;
            clone.canSkip = canSkip;
            return clone;
        }
    }
}