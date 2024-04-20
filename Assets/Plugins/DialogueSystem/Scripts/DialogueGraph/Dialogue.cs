using System;
using System.Collections.Generic;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using Plugins.DialogueSystem.Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph
{
    public class Dialogue : MonoBehaviour
    {
        [SerializeField] private UDictionary<string, Narrator> narrators;
        [SerializeField] private DialogueGraph graph;
        public bool manual;
        public bool canSkip = true;
        public KeyCode skipKey = KeyCode.Return;
        [Space]
        [SerializeField] private UDictionary<string, Object> data;
        [Space]
        public UnityEvent<string> onSentenceStart;
        public UnityEvent<string> onSentenceEnd;
        public UnityEvent onDialogueEnd;

        private Storyline _current;
        private bool _wait = true;

        public bool IsStarted => _current != null;
        public bool IsPlaying { get; private set; }
        
        
        public UDictionary<string, Object> Data => data;
        public readonly Dictionary<string, object> buffer = new();

        public void StartDialogue(string rootName)
        {
            _current = DialogueGraph.Clone(graph.roots.Find(r => r.RootName == rootName));
            SwitchUpdate();
            PlayDialogue();
        }

        public void PlayDialogue()
        {
            IsPlaying = true;
            if (!_current.drawer.IsUnityNull())
                _current.drawer.PlayDraw(this);
        }
        public void PauseDialogue()
        {
            IsPlaying = false;
            if (!_current.drawer.IsUnityNull())
                _current.drawer.PauseDraw(this);
        }

        public void StopDialogue()
        {
            _current = null;
        }

        public void ToNext()
        {
            _current.OnDelayStart(this);
            Invoke(nameof(GoToNext), Mathf.Max(0, _current.delay));
        }

        public Narrator GetNarrator(string narratorName)
        {
            if (narrators.TryGetValue(narratorName, out var narrator)) return narrator;
            return narrators.Values.Count > 0 ? narrators.Values[0] : null;
        }
        
        private void Update()
        {
            if (_current.IsUnityNull() || !IsPlaying) return;
            if (canSkip && Input.GetKeyDown(skipKey))
            {
                CancelInvoke(nameof(GoToNext));
                GoToNext();
                return;
            }

            if (_wait) return;
            if (!_current.drawer.IsUnityNull())
                _current.drawer.Draw(this);
            
            if (_current.drawer.IsUnityNull() 
                || _current.drawer.IsCompleted())
            {
                _current.OnDrawEnd(this);
                if (!manual) ToNext();
                _wait = true;
            }
        }

        private void GoToNext()
        {
            _current.OnDelayEnd(this);
            onSentenceEnd.Invoke(_current.tag);
            if (!_current.drawer.IsUnityNull())
                _current.drawer.PauseDraw(this);
            _current = _current.GetNext();
            SwitchUpdate();
        }
        private void SwitchUpdate()
        {
            if (_current.IsUnityNull())
            {
                onDialogueEnd.Invoke();
                return;
            }
            _current.OnDrawStart(this);
            _wait = false;
            onSentenceStart.Invoke(_current.tag);
        }
    }
}