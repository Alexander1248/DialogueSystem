using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.DrawerNodes
{
    public class SimpleDrawer : Drawer
    {
        [SerializeField] private string narrator;

        private Narrator _narrator;
        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.narrator = narrator;
            return node;
        }
        
        public override void OnDrawStart(Dialogue dialogue, Storyline storyline)
        {
            _narrator = dialogue.GetNarrator(narrator);
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

        public override void Draw(Dialogue dialogue)
        {
            _narrator?.Speak(container.GetText());
        }

        public override void PauseDraw(Dialogue dialogue)
        {
            _narrator.Clear();
        }

        public override void PlayDraw(Dialogue dialogue)
        {
            Draw(dialogue);
        }

        public override bool IsCompleted()
        {
            return true;
        }
    }
}