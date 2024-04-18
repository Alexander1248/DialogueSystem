namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes
{
    public abstract class BranchChoiser : AbstractNode
    {
        public int SelectionIndex { get; protected set; }
        public abstract void OnDrawStart(Dialogue dialogue, Storyline node);
        public abstract void OnDrawEnd(Dialogue dialogue, Storyline storyline);
        public abstract void OnDelayStart(Dialogue dialogue, Storyline storyline);
        public abstract void OnDelayEnd(Dialogue dialogue, Storyline storyline);
    }
}