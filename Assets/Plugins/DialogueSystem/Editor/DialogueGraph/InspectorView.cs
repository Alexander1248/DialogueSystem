﻿using UnityEngine.UIElements;

namespace Plugins.DialogueSystem.Editor.DialogueGraph
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

        private UnityEditor.Editor _editor;

        public void UpdateSelection(NodeView view)
        {
            Clear();
            _editor = UnityEditor.Editor.CreateEditor(view.node);
            Add(new IMGUIContainer(() => _editor.OnInspectorGUI()));
        }
    }
}