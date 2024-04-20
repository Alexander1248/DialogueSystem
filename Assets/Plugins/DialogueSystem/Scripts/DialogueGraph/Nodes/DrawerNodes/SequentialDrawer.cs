using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.DrawerNodes
{
    public class SequentialDrawer : Drawer
    {
        [SerializeField] private string narrator;
        [SerializeField] private float time = 1;

        private Narrator _narrator;
        private string _currentText;
        private float _time;

        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.narrator = narrator;
            node.time = time;
            return node;
        }

        public override void OnDrawStart(Dialogue dialogue, Storyline storyline)
        {
            _narrator = dialogue.GetNarrator(narrator);
            _currentText = container.GetText();
            _time = 0;
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
            PlayDraw(dialogue);
            _time += Time.deltaTime;
        }

        public override void PauseDraw(Dialogue dialogue)
        {
            _narrator.Clear();
        }

        public override void PlayDraw(Dialogue dialogue)
        {
            _narrator?.Speak(_currentText[..(int) ((_currentText.Length + 1) * _time / time)]);
        }

        public override bool IsCompleted()
        {
            return _time > time;
        }
    }
}