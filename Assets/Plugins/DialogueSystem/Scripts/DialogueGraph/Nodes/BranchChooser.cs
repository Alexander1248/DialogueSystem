using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    [OutputPort(typeof(BranchChooser), "BranchChoicer")]
    public abstract class BranchChooser : AbstractNode
    {
        static BranchChooser()
        {
            NodeColors.Colors.Add(typeof(BranchChooser), new Color(0.3f, 0.5f, 1f));
        }
        public int SelectionIndex { get; protected set; }
        public abstract void OnDrawStart(Dialogue dialogue, Storyline node);
        public abstract void OnDrawEnd(Dialogue dialogue, Storyline storyline);
        public abstract void OnDelayStart(Dialogue dialogue, Storyline storyline);
        public abstract void OnDelayEnd(Dialogue dialogue, Storyline storyline);
    }
}