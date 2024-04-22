using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.PropertyNodes
{
    public class Manual : Property
    {
        [SerializeField] private Stage stage;
        [SerializeField] private bool manual;
        
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
            if (stage != Stage.OnDrawStart) return;
            dialogue.manual = manual;
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDrawEnd) return;
            dialogue.manual = manual;
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayStart) return;
            dialogue.manual = manual;
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayEnd) return;
            dialogue.manual = manual;
        }
        public override AbstractNode Clone()
        {
            var clone = Instantiate(this);
            clone.stage = stage;
            clone.manual = manual;
            return clone;
        }
    }
}