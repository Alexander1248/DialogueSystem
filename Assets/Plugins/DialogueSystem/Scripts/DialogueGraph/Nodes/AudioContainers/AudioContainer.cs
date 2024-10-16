using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.AudioContainers
{
    [OutputPort(typeof(AudioContainer), "AudioContainer")]
    public abstract class AudioContainer : AbstractNode
    {
        static AudioContainer()
        {
            NodeColors.Colors.Add(typeof(AudioContainer), new Color(0.5f, 0, 0.7f));
        }

        public abstract AudioClip GetClip();
    }
}