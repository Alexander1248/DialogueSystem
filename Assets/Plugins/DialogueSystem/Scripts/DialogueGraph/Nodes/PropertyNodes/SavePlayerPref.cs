using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.PropertyNodes
{
    public class SavePlayerPref : Property
    {
        [SerializeField] private Stage stage;
        [SerializeField] private string key;
        [InputPort] [HideInInspector] public Value value;
        
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
            if (stage != Stage.OnDrawStart) return;
            SavePref();
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDrawEnd) return;
            SavePref();
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayStart) return;
            SavePref();
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayEnd) return;
            SavePref();
        }
        public override AbstractNode Clone()
        {
            var clone = Instantiate(this);
            clone.key = key;
            clone.stage = stage;
            return clone;
        }

        private void SavePref()
        {
            var val = value.GetValue();
            switch (val)
            {
                case Boolean:
                    PlayerPrefs.SetInt(key, val.Get() is true ? 1 : 0);
                    return;
                case Integer:
                    PlayerPrefs.SetInt(key, (int)val.Get());
                    return;
                case Float:
                    PlayerPrefs.SetFloat(key, (float)val.Get());
                    return;
                case String:
                    PlayerPrefs.SetString(key, val.Get() as string);
                    return;
            }
        }
    }

    public enum Stage
    {
        OnDrawStart,
        OnDrawEnd,
        OnDelayStart,
        OnDelayEnd
    }
}