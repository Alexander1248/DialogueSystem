using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    [OutputPort(typeof(Drawer),"Drawer")]
    public abstract class Drawer : AbstractNode
    {
        static Drawer()
        {
            NodeColors.Colors.Add(typeof(Drawer), new Color(0.8f, 0.4f, 0));
        }
        [InputPort("Text")]
        [HideInInspector]
        public TextContainer container;
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