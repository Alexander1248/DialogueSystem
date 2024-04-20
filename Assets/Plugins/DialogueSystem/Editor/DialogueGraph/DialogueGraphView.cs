using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.SEIDInfo;
using Plugins.DialogueSystem.Scripts;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.StorylineNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using IList = System.Collections.IList;

namespace Plugins.DialogueSystem.Editor.DialogueGraph
{
    public class DialogueGraphView : GraphView
    {
        public Action<NodeView> onNodeSelected;
        private Scripts.DialogueGraph.DialogueGraph _graph;

        public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }

        public DialogueGraphView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator( new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/DialogueSystem/Editor/DialogueGraph/DialogueGraphEditor.uss");
            styleSheets.Add(styleSheet);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var container = ElementAt(1);
            Vector3 screenMousePosition = evt.localMousePosition;
            Vector2 worldMousePosition = screenMousePosition - container.transform.position;
            worldMousePosition *= 1 / container.transform.scale.x;
            
            // base.BuildContextualMenu(evt);
            evt.menu.AppendAction("Update", _ => PopulateView(_graph));
            evt.menu.AppendSeparator();
            
            var types = TypeCache.GetTypesDerivedFrom<AbstractNode>();
            foreach (var type in types.Where(type => !type.IsAbstract))
            {
                var root = type;
                while (root != null) {
                    if (root == typeof(Storyline))
                    {
                        evt.menu.AppendAction($"Storylines/{type.Name}", 
                            _ => CreateNode(type, worldMousePosition));
                        break;
                    }
                    if (root == typeof(Drawer))
                    {
                        evt.menu.AppendAction($"Drawers/{type.Name}", 
                            _ => CreateNode(type, worldMousePosition));
                        break;
                    }
                    if (root == typeof(TextContainer))
                    {
                        evt.menu.AppendAction($"TextContainers/{type.Name}", 
                            _ => CreateNode(type, worldMousePosition));
                        break;
                    }
                    if (root == typeof(BranchChoicer))
                    {
                        evt.menu.AppendAction($"BranchChoicers/{type.Name}", 
                            _ => CreateNode(type, worldMousePosition));
                        break;
                    }
                    if (root == typeof(Property))
                    {
                        evt.menu.AppendAction($"Properties/{type.Name}", 
                            _ => CreateNode(type, worldMousePosition));
                        break;
                    }
                    if (root == typeof(Value))
                    {
                        evt.menu.AppendAction($"Values/{type.Name}", 
                            _ => CreateNode(type, worldMousePosition));
                        break;
                    }
                    root = root.BaseType;
                }
            }
        }

        private void CreateNode(Type type, Vector2 position)
        {
            AbstractNode node = CreateNode(type);
            node.nodePos = position;
            CreateNodeView(node);
        }

        private NodeView FindNodeView(AbstractNode sentence)
        {
            return GetNodeByGuid(sentence.guid) as NodeView;
            
        }

        public void PopulateView(Scripts.DialogueGraph.DialogueGraph graph)
        {
            _graph = graph;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
            
            _graph.nodes.ForEach(CreateNodeView);
            
            _graph.nodes.ForEach(n =>
            {
                var view = FindNodeView(n);
                switch (n)
                {
                    case Storyline storyline:
                        if (storyline.drawer != null)
                            AddElement(FindNodeView(storyline.drawer).Outputs[0].ConnectTo(view.Inputs[0]));

                        foreach (var key in storyline.next.Keys.Where(key => storyline.next[key] != null))
                            AddElement(view.Outputs[key].ConnectTo(FindNodeView(storyline.next[key]).Inputs[1]));

                        if (storyline.branchChoicer != null)
                            AddElement(FindNodeView(storyline.branchChoicer).Outputs[0].ConnectTo(view.Inputs[2]));

                        foreach (var property in storyline.properties)
                            AddElement(FindNodeView(property).Outputs[0].ConnectTo(view.Inputs[3]));
                        return;
                    default:
                        for (var i = 0; i < view.InputFields.Length; i++)
                        {
                            var type = view.InputFields[i].FieldType;
                            if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
                            {
                                var values = view.InputFields[i].GetValue(n) as IList;
                                if (values == null) continue;
                                foreach (var value in values)
                                    if (value is AbstractNode node) 
                                        AddElement(FindNodeView(node).Outputs[0].ConnectTo(view.Inputs[i]));
                            }
                            else
                            {
                                var value = view.InputFields[i].GetValue(n) as AbstractNode;
                                if (value) AddElement(FindNodeView(value).Outputs[0].ConnectTo(view.Inputs[i]));
                            }
                        }
                        return;
                }
            });
            
        }

        private void UpdateView()
        {
            PopulateView(_graph);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            int index;
            graphViewChange.elementsToRemove?.ForEach(elem =>
            {
                switch (elem)
                {
                    case NodeView nodeView:
                        DeleteNode(nodeView.node);
                        break;
                    case Edge edge:
                    {
                        if (edge.output.node is not NodeView from) return;
                        if (edge.input.node is not NodeView to) return;
                        
                        switch (from.node)
                        {
                            case Property fromProperty:
                                switch (to.node)
                                {
                                    case Storyline toStoryline:
                                        RemoveProperty(toStoryline, fromProperty);
                                        return;
                                    default:
                                        index = -1;
                                        for (var i = 0; i < to.Inputs.Length; i++)
                                            if (to.Inputs[i] == edge.input)
                                            {
                                                index = i;
                                                break;
                                            }
                                        Remove(fromProperty, to, index);
                                        return;
                                }
                            case BranchChoicer fromChoiser:
                                switch (to.node)
                                {
                                    case Storyline toStoryline:
                                        toStoryline.branchChoicer = null;
                                        return;
                                    default:
                                        index = -1;
                                        for (var i = 0; i < to.Inputs.Length; i++)
                                            if (to.Inputs[i] == edge.input)
                                            {
                                                index = i;
                                                break;
                                            }
                                        Remove(fromChoiser, to, index);
                                        return;
                                }
                            case Drawer fromDrawer:
                                switch (to.node)
                                {
                                    case Storyline toStoryline:
                                        toStoryline.drawer = null;
                                        return;
                                    case TextContainer:
                                        fromDrawer.container = null;
                                        return;
                                    default:
                                        index = -1;
                                        for (var i = 0; i < to.Inputs.Length; i++)
                                            if (to.Inputs[i] == edge.input)
                                            {
                                                index = i;
                                                break;
                                            }
                                        Remove(fromDrawer, to, index);
                                        return;
                                }
                            case TextContainer fromContainer:
                                switch (to.node)
                                {
                                    case Drawer toDrawer:
                                        toDrawer.container = null;
                                        return;
                                    default:
                                        index = -1;
                                        for (var i = 0; i < to.Inputs.Length; i++)
                                            if (to.Inputs[i] == edge.input)
                                            {
                                                index = i;
                                                break;
                                            }
                                        Remove(fromContainer, to, index);
                                        return;
                                }
                            case Storyline fromStoryline:
                                switch (to.node)
                                {
                                    case Storyline toDialogue:
                                        RemoveLink(fromStoryline, toDialogue);
                                        return;
                                    default:
                                        Debug.LogError("To node strange type!");
                                        return;
                                }
                            default:
                                index = -1;
                                for (var i = 0; i < to.Inputs.Length; i++)
                                    if (to.Inputs[i] == edge.input)
                                    {
                                        index = i;
                                        break;
                                    }
                                Remove(from.node, to, index);
                                return;
                        }
                    }
                }
            });

            graphViewChange.edgesToCreate?.ForEach(edge =>
            {
                if (edge.output.node is not NodeView from) return;
                if (edge.input.node is not NodeView to) return;
                
                switch (from.node)
                {
                    case Property fromProperty:
                        switch (to.node)
                        {
                            case Storyline toStoryline:
                                AddProperty(toStoryline, fromProperty);
                                return;
                            default:
                                index = -1;
                                for (var i = 0; i < to.Inputs.Length; i++)
                                    if (to.Inputs[i] == edge.input)
                                    {
                                        index = i;
                                        break;
                                    }
                                Add(fromProperty, to, index);
                                return;
                        }
                    case BranchChoicer fromChoiser:
                        switch (to.node)
                        {
                            case Storyline toStoryline:
                                toStoryline.branchChoicer = fromChoiser;
                                return;
                            default:
                                index = -1;
                                for (var i = 0; i < to.Inputs.Length; i++)
                                    if (to.Inputs[i] == edge.input)
                                    {
                                        index = i;
                                        break;
                                    }
                                Add(fromChoiser, to, index);
                                return;
                        }
                    case Drawer fromDrawer:
                        switch (to.node)
                        {
                            case Storyline toStoryline:
                                toStoryline.drawer = fromDrawer;
                                return;
                            default:
                                index = -1;
                                for (var i = 0; i < to.Inputs.Length; i++)
                                    if (to.Inputs[i] == edge.input)
                                    {
                                        index = i;
                                        break;
                                    }
                                Add(fromDrawer, to, index);
                                return;
                        }
                    case TextContainer fromContainer:
                        switch (to.node)
                        {
                            default:
                                index = -1;
                                for (var i = 0; i < to.Inputs.Length; i++)
                                    if (to.Inputs[i] == edge.input)
                                    {
                                        index = i;
                                        break;
                                    }
                                Add(fromContainer, to, index);
                                return;
                        }
                    case Storyline fromStoryline:
                        switch (to.node)
                        {
                            case Storyline toDialogue:
                                index = -1;
                                for (var i = 0; i < from.Outputs.Length; i++)
                                    if (from.Outputs[i] == edge.input)
                                    {
                                        index = i;
                                        break;
                                    }
                                AddLink(fromStoryline, index, toDialogue);
                                return;
                            default:
                                Debug.LogError("To node strange type!");
                                return;
                        }
                    default:
                        index = -1;
                        for (var i = 0; i < to.Inputs.Length; i++)
                            if (to.Inputs[i] == edge.input)
                            {
                                index = i;
                                break;
                            }
                        Add(from.node, to, index);
                        return;
                }
            });

            return graphViewChange;
        }

        private void CreateNodeView(AbstractNode sentence)
        {
            var nodeView = new NodeView(sentence)
            {
                onNodeSelected = onNodeSelected,
                onGraphViewUpdate = UpdateView
            };
            AddElement(nodeView);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => 
                endPort.direction != startPort.direction
                && endPort.portType == startPort.portType
                && endPort.node != startPort.node).ToList();
        }
        

        public void Save()
        {
            if (_graph == null) throw new Exception("Graph not exists!");
            EditorUtility.SetDirty(_graph);
            AssetDatabase.SaveAssets();
        }
        

        private AbstractNode CreateNode(Type type)
        {
            if (_graph == null) throw new Exception("Graph not exists!");
            var node = ScriptableObject.CreateInstance(type) as AbstractNode;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            _graph.nodes.Add(node);
            if (node is DialogueRoot r)  _graph.roots.Add(r);
            
            AssetDatabase.AddObjectToAsset(node,  _graph);
            AssetDatabase.SaveAssets();
            
            return node;
        }

        private void DeleteNode(AbstractNode node)
        {
            if (_graph == null) throw new Exception("Graph not exists!");
            if (node is DialogueRoot r) _graph.roots.Remove(r);
            _graph.nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        private static void AddLink(Storyline from, int index, Storyline to)
        {
            from.next[index] = to;
        }

        private static void RemoveLink(Storyline from, Storyline to)
        {
            foreach (var key in from.next.Keys.Where(key => from.next[key] == to))
                from.next[key] = null;
        }
        private static void AddProperty(Storyline node, Property property)
        {
            node.properties.Add(property);
        }
        private static void RemoveProperty(Storyline node, Property property)
        {
            node.properties.Remove(property);
        }
        private static void Add(AbstractNode from, NodeView to, int index)
        {
            if (index < 0 || index >= to.InputFields.Length)
                throw new ArgumentException("Wrong argument index!");
            
            var type = to.InputFields[index].FieldType;
            if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
            {
                var value = to.InputFields[index].GetValue(to.node) as IList;
                value.Add(from);
                    
            }
            else to.InputFields[index].SetValue(to.node, from);

        }
        private static void Remove(AbstractNode from, NodeView to, int index)
        {
            var type = to.InputFields[index].FieldType;
            if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
            {
                var value = to.InputFields[index].GetValue(to.node) as IList;
                value.Remove(from);
                    
            }
            else to.InputFields[index].SetValue(to.node, null);
        }
    }
}