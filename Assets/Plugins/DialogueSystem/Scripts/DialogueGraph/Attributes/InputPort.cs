using System;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InputPort : Attribute
    {
        public readonly int color;
        public readonly string name;
        public InputPort(int color, string name = null)
        {
            this.color = color;
            this.name = name;
        }
    }
}