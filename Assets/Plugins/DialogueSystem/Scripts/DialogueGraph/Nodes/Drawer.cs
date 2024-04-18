using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    public abstract class Drawer : AbstractNode
    {
        [HideInInspector] public TextContainer container;
        public abstract void OnDrawStart(Dialogue dialogue, Storyline storyline);
        public abstract void OnDrawEnd(Dialogue dialogue, Storyline storyline);
        public abstract void OnDelayStart(Dialogue dialogue, Storyline storyline);
        public abstract void OnDelayEnd(Dialogue dialogue, Storyline storyline);
        public abstract void Draw(Dialogue dialogue);
        public abstract bool IsCompleted();
        public abstract void PauseDraw(Dialogue dialogue);
        public abstract void PlayDraw(Dialogue dialogue);
    }
}