using System;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OutputPort : Attribute
    {
        public readonly int color;
        public readonly string name;
        public OutputPort(int color, string name = null)
        {
            this.color = color;
            this.name = name;
        }
    }
}