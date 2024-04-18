using System.Collections.Generic;
using System.Linq;
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
                        
                        if (dialogueNode.branchChoiser != null)
                            queue.Enqueue(dialogueNode.branchChoiser);
                        break;
                    case Drawer drawer:
                        if (drawer.container != null)
                            queue.Enqueue(drawer.container);
                        break;
                    case Property:
                    case BranchChoiser:
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
                if (n is not Storyline storyline)
                {
                    if (n is not Drawer drawer) continue;
                    var drawerClone = clone as Drawer;
                    drawerClone!.container = drawer.container == null ? null : clones[drawer.container] as TextContainer;
                    
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
                storylineClone!.branchChoiser = storyline.branchChoiser == null ? null : clones[storyline.branchChoiser] as BranchChoiser;
            }

            return clones[node] as Storyline;
        }
    }
}