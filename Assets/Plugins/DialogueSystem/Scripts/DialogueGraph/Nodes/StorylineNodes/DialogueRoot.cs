using System;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.StorylineNodes
{
    [Serializable]
    public class DialogueRoot : Storyline
    {
        [SerializeField] private string rootName;

        public string RootName => rootName;
    }
}