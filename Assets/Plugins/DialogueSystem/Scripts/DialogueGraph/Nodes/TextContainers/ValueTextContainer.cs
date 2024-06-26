﻿using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.TextContainers
{
    [EditorPath("TextContainers")]
    public class ValueTextContainer : TextContainer
    {
        [InputPort]
        [HideInInspector]
        public Value text;
        public override string GetText()
        {
            return text.GetValue().Get().ToString();
        }
    }
}