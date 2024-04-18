using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.DialogueSystem.Scripts;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.StorylineNodes;
using Plugins.DialogueSystem.Scripts.Utils;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Plugins.DialogueSystem.Editor.DialogueGraph
{
    public class NodeView : Node
    {
        public static readonly Color DialogueColor = new(0, 0.5f, 0);
        public static readonly Color PropertyColor = new(1, 1, 1);
        public static readonly Color ChoiserColor = new(0.3f, 0.5f, 1f);
        public static readonly Color DrawerColor = new(0.8f, 0.4f, 0);
        public static readonly Color ContentColor = new(0.75f, 0, 0);
        public static readonly Color ValueColor = new(1f, 1f, 0);
        
        public Action<NodeView> onNodeSelected;
        public Action onGraphViewUpdate;
        
        public readonly AbstractNode node;
        
        public FieldInfo[] InputFields { get; private set; }
        public Port[] Inputs { get; private set; }
        public  Port[] Outputs { get; private set; }
        public NodeView(AbstractNode node)
        {
            this.node = node;
            title = this.node.name;
            viewDataKey = this.node.guid;

            style.left = this.node.nodePos.x;
            style.top = this.node.nodePos.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            switch (node)
            {
                case Storyline dialogue:
                    Inputs = new Port[4];

                    Inputs[0] = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(Drawer));
                    Inputs[0].portColor = DrawerColor;
                    inputContainer.Add(Inputs[0]);

                    if (dialogue is not DialogueRoot)
                    {
                        Inputs[1] = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
                            typeof(Storyline));
                        Inputs[1].portColor = DialogueColor;
                        inputContainer.Add(Inputs[1]);
                    }

                    Inputs[2] = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single,
                        typeof(BranchChoiser));
                    Inputs[2].portColor = ChoiserColor;
                    inputContainer.Add(Inputs[2]);

                    Inputs[3] = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
                        typeof(Property));
                    Inputs[3].portColor = PropertyColor;
                    inputContainer.Add(Inputs[3]);
                    return;
                case Drawer:
                    InputFields = node.GetType().GetFields().Where(field => field.HasAttribute(typeof(InputPort))).ToArray();

                    Inputs = new Port[InputFields.Length + 1];
                    Inputs[0] = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextContainer));
                    Inputs[0].portColor = ContentColor;
                    inputContainer.Add(Inputs[0]);
                    
                    for (var i = 1; i <= InputFields.Length; i++)
                    {
                        var inputPort = InputFields[i - 1].GetAttribute<InputPort>();

                        Port.Capacity capacity;
                        var type = InputFields[i - 1].GetType();
                        if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
                        {
                            capacity = Port.Capacity.Multi;
                            type = type.GetGenericArguments()[0];
                        }
                        else capacity = Port.Capacity.Single;
                        
                        Inputs[i] = InstantiatePort(Orientation.Horizontal,Direction.Input, capacity, type);
                        Inputs[i].portColor = new Color(
                            (float) (inputPort.color & 255) / 255,
                            (float) ((inputPort.color >> 8) & 255) / 255,
                            (float) ((inputPort.color >> 16) & 255) / 255
                        );
                        inputContainer.Add(Inputs[i]);
                    }
                    return;
                default:
                    InputFields = node.GetType().GetFields().Where(field => field.HasAttribute(typeof(InputPort))).ToArray();
                    Inputs = new Port[InputFields.Length];
                    for (var i = 0; i < InputFields.Length; i++)
                    {
                        var inputPort = InputFields[i].GetAttribute<InputPort>();

                        Port.Capacity capacity;
                        var type = InputFields[i].FieldType;
                        if (type.IsGenericType && type.GetInterface(nameof(IList)) != null)
                        {
                            capacity = Port.Capacity.Multi;
                            type = type.GetGenericArguments()[0];
                        }
                        else capacity = Port.Capacity.Single;

                        Inputs[i] = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, type);
                        Inputs[i].portColor = new Color(
                            (float)(inputPort.color & 255) / 255,
                            (float)((inputPort.color >> 8) & 255) / 255,
                            (float)((inputPort.color >> 16) & 255) / 255
                        );
                        if (inputPort.name != null) Inputs[i].portName = inputPort.name;
                        inputContainer.Add(Inputs[i]);
                    }

                    return;
            }
        }

        private void CreateOutputPorts()
        {
            switch (node)
            {
                case Storyline dialogueNode:
                    Outputs = new Port[dialogueNode.next.Count];
                    for (var i = 0; i < Outputs.Length; i++)
                    {
                        Outputs[i] = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Storyline));
                        Outputs[i].portColor = DialogueColor;
                        outputContainer.Add(Outputs[i]);
                    }
                    return;
                case Property:
                    Outputs = new[] {
                        InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Property))
                    };
                    Outputs[0].portColor = PropertyColor;
                    outputContainer.Add(Outputs[0]);
                    return;
                case BranchChoiser:
                    Outputs = new[] {
                        InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(BranchChoiser))
                    };
                    Outputs[0].portColor = ChoiserColor;
                    outputContainer.Add(Outputs[0]);
                    return;
                case Drawer:
                    Outputs = new[] {
                        InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Drawer))
                    };
                    Outputs[0].portColor = DrawerColor;
                    outputContainer.Add(Outputs[0]);
                    return;
                case TextContainer:
                    Outputs = new[] {
                        InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(TextContainer))
                    };
                    Outputs[0].portColor = ContentColor;
                    outputContainer.Add(Outputs[0]);
                    return;
                case ValueNode:
                    Outputs = new[] {
                        InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(ValueNode))
                    };
                    Outputs[0].portColor = ValueColor;
                    outputContainer.Add(Outputs[0]);
                    return;
            }

        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            node.BuildContextualMenu(evt, onGraphViewUpdate);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.nodePos.x = newPos.xMin;
            node.nodePos.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            onNodeSelected?.Invoke(this);
        }
    }
}