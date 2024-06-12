using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    [OutputPort(typeof(TextPlayer),"TextPlayer")]
    public abstract class TextPlayer : AbstractNode
    {
        static TextPlayer()
        {
            NodeColors.Colors.Add(typeof(TextPlayer), new Color(0.8f, 0.4f, 0));
        }
        [FormerlySerializedAs("container")]
        [InputPort("Text")]
        [HideInInspector]
        public TextContainer textContainer;
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