using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine.Events;

public class AnimatedDialogueZone : DialogueZone
{
    [System.Serializable]
    public class DialogueAnimation
    {
        [Tooltip("the parameter name to occurr when dialog index happens")]
        public string animationParameter;
        public int dialogueLineIndex;
        public bool animationParameterValue;

        public DialogueAnimation(string _animationParameter, int _dialogueLineIndex, bool _animationParameterValue)
        {
            animationParameter = _animationParameter;
            dialogueLineIndex = _dialogueLineIndex;
            animationParameterValue = _animationParameterValue;
        }
    }

    [MMInspectorGroup("Dialogue Animation", true, 22)]

    [Tooltip("the animator to use for the dialogue box")]
    public Animator Animator;

    public List<DialogueAnimation> DialogueAnimations = new List<DialogueAnimation>();

    [MMInspectorGroup("After dialogue events", true, 22)]
    [Tooltip("an action to trigger after the dialogue")]
    public UnityEvent AfterDialogue;

    // Update is called once per frame
    void Update()
    {
        PerformDialogueAnimations();
    }

    private void PerformDialogueAnimations()
    {
        foreach (DialogueAnimation dialogueAnimation in DialogueAnimations)
        {
            if (_currentIndex == dialogueAnimation.dialogueLineIndex)
            {
                Animator.SetBool(dialogueAnimation.animationParameter, dialogueAnimation.animationParameterValue);
            }
        }
    }

    public void TriggerAfterDialogue()
    {
        // After the dialogue we trigger the after dialogue events
        if (_playing)
        {
            AfterDialogue.Invoke();
        }
    }
}
