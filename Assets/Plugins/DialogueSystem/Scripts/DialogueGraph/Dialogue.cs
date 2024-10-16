using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
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
        
        [SerializeField] private bool lazy = true;

        private Storyline _current;
        private bool _wait = true;
        private readonly Queue<string> _fastSwap = new();
        private readonly Queue<string> _queue = new();
        private readonly Dictionary<AbstractNode, AbstractNode> _cloneBuffer = new();
        private readonly Queue<AbstractNode> _cloningQueue = new();

        public bool IsStarted => _current != null;
        public bool IsPlaying { get; private set; }
        
        
        public UDictionary<string, Object> Data => data;
        public readonly Dictionary<string, object> buffer = new();

        public void StartDialogueNow(string rootName)
        {
            Storyline root = graph.roots.Find(r => r.RootName == rootName);
            if (root.IsUnityNull()) return;

            if (lazy)
            {
                _current = root.Clone() as Storyline;
                _cloneBuffer[root] = _current;
            }
            else _current = DialogueGraph.Clone(root);
            SwitchUpdate();
            PlayDialogue();
        }
        public void StartDialogue(string rootName)
        {
            _fastSwap.Enqueue(rootName);
            PlayDialogue();
        }
        public void QueueDialogue(string rootName)
        {
            _queue.Enqueue(rootName);
            PlayDialogue();
        }

        public void PlayDialogue()
        {
            if (IsPlaying) return;
            IsPlaying = true;
            if (!_current.textPlayer.IsUnityNull())
                _current.textPlayer.PlayDraw(this);
        }
        public void PauseDialogue()
        {
            if (!IsPlaying) return;
            IsPlaying = false;
            if (!_current.textPlayer.IsUnityNull())
                _current.textPlayer.PauseDraw(this);
        }

        public void StopDialogue()
        {
            _current = null;
        }
        public void ClearFastSwap()
        {
            _fastSwap.Clear();
        }
        public void ClearQueue()
        {
            _queue.Clear();
        }

        public void StopAll()
        {
            ClearFastSwap();
            ClearQueue();
            StopDialogue();
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
            if (!IsPlaying) return;
            if (_current.IsUnityNull())
            {
                if (_fastSwap.TryDequeue(out var fastRoot))
                {
                    StartDialogueNow(fastRoot);
                    return;
                }
                if (_queue.TryDequeue(out var root))
                {
                    StartDialogueNow(root);
                    return;
                }
                return;
            }
            if (canSkip && Input.GetKeyDown(skipKey))
            {
                CancelInvoke(nameof(GoToNext));
                _current.OnDrawEnd(this);
                _current.OnDelayStart(this);
                GoToNext();
                return;
            }

            if (_wait) return;
            if (!_current.textPlayer.IsUnityNull())
                _current.textPlayer.Draw(this);
            
            if (_current.textPlayer.IsUnityNull() 
                || _current.textPlayer.IsCompleted())
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
            if (!_current.textPlayer.IsUnityNull())
                _current.textPlayer.PauseDraw(this);
            if (_fastSwap.TryDequeue(out var root))
            {
                StartDialogueNow(root);
                return;
            }
            _current = _current.GetNext();
            if (lazy && _current != null)
            {
                if (_cloneBuffer.TryGetValue(_current, out var c)) _current = c as Storyline;
                else
                {
                    _cloneBuffer[_current] = _current.Clone();
                    _cloningQueue.Enqueue(_cloneBuffer[_current]);
                    while (_cloningQueue.Count > 0)
                    {
                        var clone = _cloningQueue.Dequeue();

                        foreach (var field in clone.GetType().GetFields())
                        {
                            if (!field.HasAttribute(typeof(InputPort))) continue;
                            if (field.FieldType.IsGenericType && field.FieldType.GetInterface(nameof(IList)) != null)
                            {
                                if (field.GetValue(clone) is not IList values) continue;
                                var list = (IList)Activator.CreateInstance(field.FieldType);
                                foreach (var value in values)
                                    if (value is AbstractNode abstractNode)
                                    {
                                        if (abstractNode.IsUnityNull())
                                        {
                                            list.Add(null);
                                            return;
                                        }
                                        if (!_cloneBuffer.ContainsKey(abstractNode))
                                            _cloneBuffer[abstractNode] = abstractNode.Clone();
                                        _cloningQueue.Enqueue(_cloneBuffer[abstractNode]);
                                        list.Add(_cloneBuffer[abstractNode]);
                                    }

                                field.SetValue(clone, list);
                            }
                            else if (field.GetValue(clone) is AbstractNode abstractNode)
                            {
                                if (abstractNode.IsUnityNull()) return;
                                if (!_cloneBuffer.ContainsKey(abstractNode))
                                    _cloneBuffer[abstractNode] = abstractNode.Clone();
                                _cloningQueue.Enqueue(_cloneBuffer[abstractNode]);
                                field.SetValue(clone, _cloneBuffer[abstractNode]);
                            }
                        }
                    }

                    _current = _cloneBuffer[_current] as Storyline;
                }

            }
            SwitchUpdate();
        }
        private void SwitchUpdate()
        {
            if (_current.IsUnityNull())
            {
                onDialogueEnd.Invoke();
                
                if (_fastSwap.TryDequeue(out var fastRoot))
                {
                    StartDialogueNow(fastRoot);
                    return;
                }
                if (_queue.TryDequeue(out var root))
                {
                    StartDialogueNow(root);
                    return;
                }
                
                if (lazy) _cloneBuffer.Clear();
                return;
            }
            _current.OnDrawStart(this);
            _wait = false;
            onSentenceStart.Invoke(_current.tag);
        }
    }
}