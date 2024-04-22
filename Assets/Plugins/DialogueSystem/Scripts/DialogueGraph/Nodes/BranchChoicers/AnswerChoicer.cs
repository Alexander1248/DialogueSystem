using System;
using System.Collections.Generic;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.BranchChoicers
{
    public class AnswerChoicer : BranchChoicer
    {
        [SerializeField] private GameObject answerPrefab;
        [SerializeField] private string answersRootKey;

        [InputPort("Answers")]
        [HideInInspector]
        public List<TextContainer> answers;

        [SerializeField] private bool late;
        
        [SerializeField] private float pivot;
        [SerializeField] private Rect margin;

        private UnityAction<Dialogue> _listener;
        private List<Answer> _answers;
        private bool _isManual;
        private CursorLockMode _modeBuff;
        private bool _visibilityBuff;
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
            if (!answerPrefab.TryGetComponent(typeof(RectTransform), out _))
                throw new ArgumentException("Answer prefab not UI component!");
            if (!answerPrefab.TryGetComponent(typeof(Answer), out _))
                throw new ArgumentException("Answer prefab not contain Answer Behaviour!");
            
            
            
            var rootGameObject = dialogue.Data[answersRootKey] as GameObject;
            if (rootGameObject.IsUnityNull())
                throw new ArgumentException("Answer Root is Null!");
            if (!rootGameObject.TryGetComponent(typeof(RectTransform), out var transformComponent))
                throw new ArgumentException("Answer Root not UI component!");
            var answersRoot = transformComponent as RectTransform;

            if (dialogue.buffer.TryGetValue("answerBuff", out var value)) _answers = value as List<Answer>;
            else
            {
                _answers = new List<Answer>();
                dialogue.buffer["answerBuff"] = _answers;
            }

            _listener = Click;
            _isManual = dialogue.manual;
            dialogue.manual = true;
            while (_answers.Count < answers.Count)
            {
                var answerGameObject = Instantiate(answerPrefab);
                var transform = answerGameObject.GetComponent<RectTransform>();
                transform.SetParent(answersRoot);
                transform.pivot = new Vector2(pivot, 0);
                var pos = new Vector2(
                    (answersRoot.sizeDelta.x - margin.xMin * 2) * (pivot - 0.5f),
                    margin.yMin - answersRoot.sizeDelta.y * 0.5f
                );
                
                pos.y += (margin.min.y + margin.max.y + transform.sizeDelta.y) * _answers.Count;
                transform.anchoredPosition = pos;
                _answers.Add(answerGameObject.GetComponent<Answer>());
                answerGameObject.SetActive(false);
            }

            if (late) return;
            _modeBuff = Cursor.lockState;
            _visibilityBuff = Cursor.visible;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            
            for (var i = 0; i < answers.Count; i++)
            {
                _answers[i].gameObject.SetActive(true);
                _answers[i].Show(answers[i].GetText(), () => _listener.Invoke(dialogue));
            }
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            if (!late) return;
            _modeBuff = Cursor.lockState;
            _visibilityBuff = Cursor.visible;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            
            for (var i = 0; i < answers.Count; i++)
            {
                _answers[i].gameObject.SetActive(true);
                _answers[i].Show(answers[i].GetText(), () => _listener.Invoke(dialogue));
            }
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            Cursor.lockState = _modeBuff;
            Cursor.visible = _visibilityBuff;
            for (var i = 0; i < answers.Count; i++)
            {
                _answers[i].Hide();
                _answers[i].gameObject.SetActive(false);
            }
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            
        }

        private void Click(Dialogue dialogue)
        {
            SelectionIndex = -1;
            for (var i = 0; i < answers.Count; i++)
                if (_answers[i].IsSelected)
                {
                    SelectionIndex = i;
                    break;
                }

            dialogue.manual = _isManual;
            dialogue.ToNext();
        }

        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.answerPrefab = answerPrefab;
            node.answersRootKey = answersRootKey;
            node.late = late;
            node.pivot = pivot;
            node.margin = margin;
            return node;
        }
    }
}