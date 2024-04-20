using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.PropertyNodes
{
    public class ExecuteScript : Property
    {
        [SerializeField] private PropertyScript script;
        
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
            script.OnDrawStart(dialogue, node);
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            script.OnDrawEnd(dialogue, storyline);
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            script.OnDelayStart(dialogue, storyline);
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            script.OnDelayEnd(dialogue, storyline);
        }
        public override AbstractNode Clone()
        {
            var clone = Instantiate(this);
            clone.script = script;
            return clone;
        }
    }
}