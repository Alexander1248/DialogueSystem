using System.Collections.Generic;
using System.Linq;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.StorylineNodes;
using Unity.VisualScripting;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph
{
    [CreateAssetMenu(fileName = "Dialogue Graph")]
    public class DialogueGraph : ScriptableObject
    {
        [HideInInspector] public List<DialogueRoot> roots = new();
        [HideInInspector] public List<AbstractNode> nodes = new();

        public static Storyline Clone(Storyline node)
        {
            // TODO: Clone graph
            var clones = new Dictionary<AbstractNode, AbstractNode>();
            var completed = new List<AbstractNode>();
            var queue = new Queue<AbstractNode>();
            
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var n = queue.Dequeue();
                if (completed.Contains(n)) continue;
                completed.Add(n);
                clones[n] = n.Clone();
                switch (n)
                {
                    case Storyline dialogueNode:
                        foreach (var key in dialogueNode.next.Keys.Where(key => !dialogueNode.next[key].IsUnityNull()))
                            queue.Enqueue(dialogueNode.next[key]);
                        foreach (var property in dialogueNode.properties)
                            queue.Enqueue(property);
                        
                        if (dialogueNode.drawer != null)
                            queue.Enqueue(dialogueNode.drawer);
                        
                        if (dialogueNode.branchChoicer != null)
                            queue.Enqueue(dialogueNode.branchChoicer);
                        break;
                    default:
                        foreach (var field in n.GetType().GetFields())
                        {
                            if (!field.HasAttribute(typeof(InputPort))) continue;
                            if (field.GetValue(n) is AbstractNode abstractNode)
                                queue.Enqueue(abstractNode);
                        }
                        
                        break;
                }
            }
            
            completed.Clear();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var n = queue.Dequeue();
                if (completed.Contains(n)) continue;
                completed.Add(n);

                var clone = clones[n];
                if (clone == null) continue;
                if (n is not Storyline storyline)
                {
                    foreach (var field in n.GetType().GetFields())
                    {
                        if (!field.HasAttribute(typeof(InputPort))) continue;
                        var value = field.GetValue(n) as AbstractNode;
                        field.SetValue(clone, value == null ? null : clones[value]);
                    }
                    continue;
                }

                var storylineClone = clone as Storyline;
                foreach (var key in storyline.next.Keys)
                {
                    storylineClone!.next[key] = storyline.next[key].IsUnityNull() ? null : clones[storyline.next[key]] as Storyline;
                    if (storyline.next[key].IsUnityNull()) continue;
                    queue.Enqueue(storyline.next[key]);
                }

                foreach (var property in storyline.properties)
                    storylineClone!.properties.Add(clones[property] as Property);

                storylineClone!.drawer = storyline.drawer == null ? null : clones[storyline.drawer] as Drawer;
                storylineClone!.branchChoicer = storyline.branchChoicer == null ? null : clones[storyline.branchChoicer] as BranchChoicer;
            }

            return clones[node] as Storyline;
        }
    }
}