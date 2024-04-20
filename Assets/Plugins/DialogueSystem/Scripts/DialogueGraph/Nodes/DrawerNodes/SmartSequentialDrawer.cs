using Plugins.DialogueSystem.Scripts.Utils;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.DrawerNodes
{
    public class SmartSequentialDrawer : Drawer
    {
        [SerializeField] private string narrator;
        [SerializeField] private UDictionary<char, float> symbolTime;
        [SerializeField] private float defaultSymbolTime;

        private Narrator _narrator;
        private string _currentText;
        private float _timeLimit;
        private float _time;
        private int _index;
        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.narrator = narrator;
            node.symbolTime = symbolTime;
            node.defaultSymbolTime = defaultSymbolTime;
            return node;
        }
        
        public override void OnDrawStart(Dialogue dialogue, Storyline storyline)
        {
            _narrator = dialogue.GetNarrator(narrator);
            _currentText = container.GetText();
            _time = 0;
            _index = 0;
            ComputeLimit();
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

        private void ComputeLimit()
        {
            if (IsCompleted())
            {
                _timeLimit = 0;
                return;
            }
            var key = _currentText[_index];
            _timeLimit = symbolTime.TryGetValue(key, out var value) ? value : defaultSymbolTime;
        }

        public override void Draw(Dialogue dialogue)
        {
            PlayDraw(dialogue);
            
            _time += Time.deltaTime;
            if (_time < _timeLimit) return;
            while (_time >= _timeLimit)
            {
                _index++;
                _time -= _timeLimit;
            }
            ComputeLimit();
        }

        public override void PauseDraw(Dialogue dialogue)
        {
            _narrator.Clear();
        }

        public override void PlayDraw(Dialogue dialogue)
        {
            if (!IsCompleted())
                _narrator?.Speak(_currentText[..(_index + 1)]);
        }

        public override bool IsCompleted()
        {
            return _index >= _currentText.Length;
        }
    }
}