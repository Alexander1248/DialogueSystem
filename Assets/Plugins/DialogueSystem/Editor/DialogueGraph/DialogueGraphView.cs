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

            var choisers = new List<Type>();
            var drawers = new List<Type>();
            var properties = new List<Type>();
            var storylines = new List<Type>();
            var containers = new List<Type>();
            var values = new List<Type>();
            
            var types = TypeCache.GetTypesDerivedFrom<AbstractNode>();
            foreach (var type in types.Where(type => !type.IsAbstract))
            {
                var root = type;
                while (root != null) {
                    if (root == typeof(Storyline))
                    {
                        storylines.Add(type);
                        break;
                    }
                    if (root == typeof(Drawer))
                    {
                        drawers.Add(type);
                        break;
                    }
                    if (root == typeof(TextContainer))
                    {
                        containers.Add(type);
                        break;
                    }
                    if (root == typeof(BranchChoiser))
                    {
                        choisers.Add(type);
                        break;
                    }
                    if (root == typeof(Property))
                    {
                        properties.Add(type);
                        break;
                    }
                    if (root == typeof(ValueNode))
                    {
                        values.Add(type);
                        break;
                    }
                    root = root.BaseType;
                }
            }
            
            
            evt.menu.AppendSeparator();
            storylines.ForEach(type => evt.menu.AppendAction($"{type.Name}", _ => CreateNode(type, worldMousePosition)));
            
            evt.menu.AppendSeparator();
            drawers.ForEach(type => evt.menu.AppendAction($"{type.Name}", _ => CreateNode(type, worldMousePosition)));
           
            evt.menu.AppendSeparator();
            containers.ForEach(type => evt.menu.AppendAction($"{type.Name}", _ => CreateNode(type, worldMousePosition)));
            
            evt.menu.AppendSeparator();
            choisers.ForEach(type => evt.menu.AppendAction($"{type.Name}", _ => CreateNode(type, worldMousePosition)));
            
            evt.menu.AppendSeparator();
            properties.ForEach(type => evt.menu.AppendAction($"{type.Name}", _ => CreateNode(type, worldMousePosition)));
            
            evt.menu.AppendSeparator();
            values.ForEach(type => evt.menu.AppendAction($"{type.Name}", _ => CreateNode(type, worldMousePosition)));
                
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

                        if (storyline.branchChoiser != null)
                            AddElement(FindNodeView(storyline.branchChoiser).Outputs[0].ConnectTo(view.Inputs[2]));

                        foreach (var property in storyline.properties)
                            AddElement(FindNodeView(property).Outputs[0].ConnectTo(view.Inputs[3]));
                        return;

                    case Drawer drawer:
                        if (drawer.container != null)
                            AddElement(FindNodeView(drawer.container).Outputs[0].ConnectTo(view.Inputs[0]));
                        for (var i = 0; i < view.InputFields.Length; i++)
                        {
                            var type = view.InputFields[i].FieldType;
                            if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
                            {
                                var value = view.InputFields[i].GetValue(drawer) as IList<AbstractNode>;
                                foreach (var val in value)
                                    AddElement(FindNodeView(val).Outputs[0].ConnectTo(view.Inputs[i + 1]));
                            }
                            else
                            {
                                var value = view.InputFields[i].GetValue(drawer) as AbstractNode;
                                AddElement(FindNodeView(value).Outputs[0].ConnectTo(view.Inputs[i + 1]));
                            }
                        }
                        return;

                    default:
                        for (var i = 0; i < view.InputFields.Length; i++)
                        {
                            var type = view.InputFields[i].FieldType;
                            if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
                            {
                                var value = view.InputFields[i].GetValue(n) as IList<AbstractNode>;
                                foreach (var val in value)
                                    AddElement(FindNodeView(val).Outputs[0].ConnectTo(view.Inputs[i + 1]));
                            }
                            else
                            {
                                var value = view.InputFields[i].GetValue(n) as AbstractNode;
                                AddElement(FindNodeView(value).Outputs[0].ConnectTo(view.Inputs[i + 1]));
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
                                        Debug.LogError("To node strange type!");
                                        return;
                                }
                            case BranchChoiser:
                                switch (to.node)
                                {
                                    case Storyline toStoryline:
                                        toStoryline.branchChoiser = null;
                                        return;
                                    default:
                                        Debug.LogError("To node strange type!");
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
                                        Debug.LogError("To node strange type!");
                                        return;
                                }
                            case TextContainer:
                                switch (to.node)
                                {
                                    case Drawer toDrawer:
                                        toDrawer.container = null;
                                        return;
                                    default:
                                        Debug.LogError("To node strange type!");
                                        return;
                                }
                            case Storyline fromStoryline:
                                switch (to.node)
                                {
                                    case Property toProperty:
                                        RemoveProperty(fromStoryline, toProperty);
                                        return;
                                    case BranchChoiser:
                                        fromStoryline.branchChoiser = null;
                                        return;
                                    case Drawer:
                                        fromStoryline.drawer = null;
                                        return;
                                    case Storyline toDialogue:
                                        RemoveLink(fromStoryline, toDialogue);
                                        return;
                                    default:
                                        Debug.LogError("To node strange type!");
                                        return;
                                }
                            default:
                                Debug.LogError("From node strange type!");
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
                                Debug.LogError("To node strange type!");
                                return;
                        }
                    case BranchChoiser fromChoiser:
                        switch (to.node)
                        {
                            case Storyline toStoryline:
                                toStoryline.branchChoiser = fromChoiser;
                                return;
                            default:
                                Debug.LogError("To node strange type!");
                                return;
                        }
                    case Drawer fromDrawer:
                        switch (to.node)
                        {
                            case Storyline toStoryline:
                                toStoryline.drawer = fromDrawer;
                                return;
                            case TextContainer toHandler:
                                fromDrawer.container = toHandler;
                                return;
                            default:
                                Debug.LogError("To node strange type!");
                                return;
                        }
                    case TextContainer fromHandler:
                        switch (to.node)
                        {
                            case Drawer toDrawer:
                                toDrawer.container = fromHandler;
                                return;
                            default:
                                Debug.LogError("To node strange type!");
                                return;
                        }
                    case Storyline fromStoryline:
                        switch (to.node)
                        {
                            case Property toProperty:
                                AddProperty(fromStoryline, toProperty);
                                return;
                            case BranchChoiser toChoiser:
                                fromStoryline.branchChoiser = toChoiser;
                                return;
                            case Drawer toDrawer:
                                fromStoryline.drawer = toDrawer;
                                return;
                            case Storyline toDialogue:
                                var index = 0;
                                for (var i = 0; i < from.Outputs.Length; i++)
                                    if (from.Outputs[i] == edge.output)
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
                        Debug.LogError("From node strange type!");
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
        private static void Add(NodeView from, int index, AbstractNode to)
        {
            var type = from.InputFields[index].FieldType;
            if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
            {
                var value = from.InputFields[index].GetValue(from.node) as IList<AbstractNode>;
                value.Add(to);
                    
            }
            else from.InputFields[index].SetValue(from.node, to);

        }
        private static void Remove(NodeView from, int index, AbstractNode to)
        {
            var type = from.InputFields[index].FieldType;
            if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
            {
                var value = from.InputFields[index].GetValue(from.node) as IList<AbstractNode>;
                value.Remove(to);
                    
            }
            else from.InputFields[index].SetValue(from.node, null);
        }
    }
}