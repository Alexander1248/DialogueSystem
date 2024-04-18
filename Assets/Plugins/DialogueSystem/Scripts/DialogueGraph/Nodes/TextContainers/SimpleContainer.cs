using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.TextContainers
{
    public class SimpleContainer : TextContainer
    {
        [SerializeField] private string text;
        public override string GetText()
        {
            return text;
        }

        public override AbstractNode Clone()
        {
            var clone = base.Clone() as SimpleContainer;
            clone.text = text;
            return clone;
        }
    }
}