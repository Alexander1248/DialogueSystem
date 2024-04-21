using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.Value;
using UnityEngine;
using UnityEngine.Serialization;

namespace Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.PropertyNodes
{
    public class PlayAnimation : Property
    {
        [SerializeField] private string animationObjectKey;
        [SerializeField] private Stage playStage;
        [SerializeField] private Stage stopStage;
        [SerializeField] private string stateName;
        [SerializeField] private float crossFade = -1;
        [SerializeField] private bool fixedTime;
        
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        {
            if (stopStage == Stage.OnDrawStart)
            {
                switch (dialogue.Data[animationObjectKey])
                {
                    case Animation animation:
                        AnimationStop(animation);
                        return;
                    case Animator animator:
                        AnimatorStop(animator);
                        return;
                }
            }

            if (playStage != Stage.OnDrawStart) return;
            switch (dialogue.Data[animationObjectKey])
            {
                case Animation animation:
                    AnimationPlay(animation);
                    return;
                case Animator animator:
                    AnimatorPlay(animator);
                    return;
            }
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stopStage == Stage.OnDrawEnd)
            {
                switch (dialogue.Data[animationObjectKey])
                {
                    case Animation animation:
                        AnimationStop(animation);
                        return;
                    case Animator animator:
                        AnimatorStop(animator);
                        return;
                }
            }

            if (playStage != Stage.OnDrawEnd) return;
            switch (dialogue.Data[animationObjectKey])
            {
                case Animation animation:
                    AnimationPlay(animation);
                    return;
                case Animator animator:
                    AnimatorPlay(animator);
                    return;
            }
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            if (stopStage == Stage.OnDelayStart)
            {
                switch (dialogue.Data[animationObjectKey])
                {
                    case Animation animation:
                        AnimationStop(animation);
                        return;
                    case Animator animator:
                        AnimatorStop(animator);
                        return;
                }
            }

            if (playStage != Stage.OnDelayStart) return;
            switch (dialogue.Data[animationObjectKey])
            {
                case Animation animation:
                    AnimationPlay(animation);
                    return;
                case Animator animator:
                    AnimatorPlay(animator);
                    return;
            }
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stopStage == Stage.OnDelayEnd)
            {
                switch (dialogue.Data[animationObjectKey])
                {
                    case Animation animation:
                        AnimationStop(animation);
                        return;
                    case Animator animator:
                        AnimatorStop(animator);
                        return;
                }
            }

            if (playStage != Stage.OnDelayEnd) return;
            switch (dialogue.Data[animationObjectKey])
            {
                case Animation animation:
                    AnimationPlay(animation);
                    return;
                case Animator animator:
                    AnimatorPlay(animator);
                    return;
            }
        }

        private static void AnimationPlay(Animation animation)
        {
            animation.Play();
        }
        private static void AnimationStop(Animation animation)
        {
            animation.Stop();
        }
        private void AnimatorPlay(Animator animator)
        {
            animator.StartPlayback();
            if (fixedTime)
            {
                if (crossFade > 0) animator.CrossFadeInFixedTime(stateName, crossFade);
                else animator.PlayInFixedTime(stateName);
                return;
            }
            if (crossFade > 0) animator.CrossFade(stateName, crossFade);
            else animator.Play(stateName);
        }
        private static void AnimatorStop(Animator animator)
        {
            animator.StopPlayback();
        }
        public override AbstractNode Clone()
        {
            var clone = Instantiate(this);
            clone.animationObjectKey = animationObjectKey;
            clone.playStage = playStage;
            clone.stopStage = stopStage;
            clone.stateName = stateName;
            clone.crossFade = crossFade;
            clone.fixedTime = fixedTime;
            return clone;
        }
    }
}