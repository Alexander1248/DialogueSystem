using System;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using String = Plugins.DialogueSystem.Scripts.Value.String;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.ValueNodes
{
    public class PlayerPref : Value
    {
        [SerializeField] private string key;
        [SerializeField] private PlayerPrefType type;
        public override IValue GetValue()
        {
            return type switch
            {
                PlayerPrefType.Integer => new Integer(PlayerPrefs.GetInt(key)),
                PlayerPrefType.Float => new Float(PlayerPrefs.GetFloat(key)),
                PlayerPrefType.String => new String(PlayerPrefs.GetString(key)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    [Serializable]
    public enum PlayerPrefType
    {
        Integer,
        Float,
        String
    }
}