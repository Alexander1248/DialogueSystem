using System;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    public abstract class ValueNode : AbstractNode
    {
        public abstract object GetValue();
    }
}