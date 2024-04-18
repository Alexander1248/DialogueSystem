using System;
using System.Collections.Generic;
using Plugins.DialogueSystem.Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    public class Storyline : AbstractNode
    {
        [HideInInspector] public UDictionary<int, Storyline> next = new();
        [HideInInspector] public BranchChoiser branchChoiser;
        [FormerlySerializedAs("contextHandler")] [HideInInspector] public Drawer drawer;
        [HideInInspector] public List<Property> properties = new();
        public string tag;
        public float delay = 1;


        public Storyline()
        {
            next[0] = null;
        }

        public virtual Storyline GetNext() 
        {
            if (branchChoiser.IsUnityNull()) return next[0];
            if (branchChoiser.SelectionIndex < 0 || branchChoiser.SelectionIndex >= next.Count)
            {
                Debug.LogError("BranchChoisers selection index out of bounds!");
                return null;
            }
            return next[branchChoiser.SelectionIndex];
        }


        public virtual void OnDrawStart(Dialogue dialogue)
        {
            properties.ForEach(property => property.OnDrawStart(dialogue, this));
            
            if (!drawer.IsUnityNull())
                drawer.OnDrawStart(dialogue, this);
            
            if (!branchChoiser.IsUnityNull())
                branchChoiser.OnDrawStart(dialogue, this);
        }

        public virtual void OnDrawEnd(Dialogue dialogue)
        {
            properties.ForEach(property => property.OnDrawEnd(dialogue, this));
            
            if (!drawer.IsUnityNull())
                drawer.OnDrawEnd(dialogue, this);
            
            if (!branchChoiser.IsUnityNull())
                branchChoiser.OnDrawEnd(dialogue, this);
        } 
        public virtual void OnDelayStart(Dialogue dialogue)
        {
            
            properties.ForEach(property => property.OnDelayStart(dialogue, this));
            
            if (!drawer.IsUnityNull())
                drawer.OnDelayStart(dialogue, this);
            
            if (!branchChoiser.IsUnityNull())
                branchChoiser.OnDelayStart(dialogue, this);
        } 
        public virtual void OnDelayEnd(Dialogue dialogue)
        {
            
            properties.ForEach(property => property.OnDelayEnd(dialogue, this));
            
            if (!drawer.IsUnityNull())
                drawer.OnDelayEnd(dialogue, this);
            
            if (!branchChoiser.IsUnityNull())
                branchChoiser.OnDelayEnd(dialogue, this);
        } 
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt, Action onGraphViewUpdate)
        {
            evt.menu.AppendAction("Add Branch", _ =>
            {
                next[next.Count] = null;
                onGraphViewUpdate.Invoke();
            });
            evt.menu.AppendAction("Remove Branch", _ =>
            {
                next.Remove(next.Count - 1);
                onGraphViewUpdate.Invoke();
            });
        }
    }
}