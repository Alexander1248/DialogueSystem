using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.BranchChoicers
{
    public class RandomChoicer : BranchChoicer
    {

        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
            SelectionIndex = node.next.Keys[Random.Range(0, node.next.Keys.Count)];
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            
        }

        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            return node;
        }
    }
}