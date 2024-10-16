using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.TextContainers
{
    [EditorPath("TextContainers")]
    public class UnityLocalizedContainer : TextContainer
    {
        [SerializeField] private LocalizedString localizedString;
        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.localizedString = new LocalizedString(localizedString.TableReference, localizedString.TableEntryReference);
            return node;
        }
        public override string GetText()
        {
            localizedString.RefreshString();
            return localizedString.GetLocalizedString();
        }
    }
}