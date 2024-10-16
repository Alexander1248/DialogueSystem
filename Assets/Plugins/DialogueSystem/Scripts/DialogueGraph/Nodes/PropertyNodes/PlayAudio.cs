using System;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.AudioContainers;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.PropertyNodes
{
    [EditorPath("Property")]
    public class PlayAudio : Property
    { 
        [SerializeField] private Stage stage;
        [SerializeField] private string dataName = "source";
        [InputPort("AudioContainer")] 
        [HideInInspector] 
        public AudioContainer container;
        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.stage = stage;
            return node;
        }
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        { 
            if (stage != Stage.OnDrawStart) return;
            if (dialogue.Data[dataName] is not AudioSource source)
                throw new ArgumentException($"Type of field \"{dataName}\" is not AudioSource");
            source.clip = container.GetClip();
            source.Play();
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDrawEnd) return;
            if (dialogue.Data[dataName] is not AudioSource source)
                throw new ArgumentException($"Type of field \"{dataName}\" is not AudioSource");
            source.clip = container.GetClip();
            source.Play();
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayStart) return;
            if (dialogue.Data[dataName] is not AudioSource source)
                throw new ArgumentException($"Type of field \"{dataName}\" is not AudioSource");
            source.clip = container.GetClip();
            source.Play();
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayEnd) return;
            if (dialogue.Data[dataName] is not AudioSource source)
                throw new ArgumentException($"Type of field \"{dataName}\" is not AudioSource");
            source.clip = container.GetClip();
            source.Play();
        }
    }
}