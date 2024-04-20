using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    [OutputPort(typeof(TextContainer),"Text")]
    public abstract class TextContainer : AbstractNode
    {
        static TextContainer()
        {
            NodeColors.Colors.Add(typeof(TextContainer), new Color(0.75f, 0, 0));
        }
        public abstract string GetText();
    }
}