
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    public abstract class PropertyScript : AbstractNode
    {
        public abstract void OnDrawStart(Dialogue dialogue, Storyline node);
        public abstract void OnDrawEnd(Dialogue dialogue, Storyline storyline);
        public abstract void OnDelayStart(Dialogue dialogue, Storyline storyline);
        public abstract void OnDelayEnd(Dialogue dialogue, Storyline storyline);
    }
}