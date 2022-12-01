using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class GrappableObjectLifetime : FallingPlatform
{
    [Tooltip("the feedbacks to play when the object starts falling")]
    public MMFeedbacks StartFallingFeedbacks;

    [Tooltip("the character that is grappled to this object")]
    public CharacterHangToGrappableObject characterHangToGrappableObject;

    public bool canBreak = false;

    private GrappableObject grappableObject;

    private bool shouldFall = false;

    protected override void Initialization()
    {
        base.Initialization();

        grappableObject = GetComponent<GrappableObject>();
    }

    protected override void FixedUpdate()
    {
        if (canBreak)
        {
            // we send our various states to the animator.		
            UpdateAnimator();

            if (shouldFall)
            {
                FallingLogic();
            }
        }
    }

    private void FallingLogic()
    {
        _newPosition = new Vector2(0, -FallSpeed * Time.deltaTime);

        transform.Translate(_newPosition, Space.World);

        // TODO: Currently this is fine, but we might want to change it to when the object collides with something disable it.
        if (transform.position.y < _bounds.min.y)
        {
            DisableFallingPlatform();
        }
    }

    public override void OnTriggerStay2D(Collider2D collider)
    {
        if (!canBreak)
            return;

        characterHangToGrappableObject = collider.GetComponent<CharacterHangToGrappableObject>();
        if (characterHangToGrappableObject == null)
            return;

        if (TimeBeforeFall > 0)
        {
            _timer -= Time.deltaTime;
            _shaking = true;
            if (_timer < 0)
            {
                StartFalling();
            }
        }
        else
        {
            _shaking = false;
        }
    }

    private void StartFalling()
    {

        // Detach the character from the grappable object when it starts falling
        if (!shouldFall)
            characterHangToGrappableObject.StopGrabbingLedge();

        grappableObject.canGrabLedge = false;
        shouldFall = true;
        StartFallingFeedbacks?.PlayFeedbacks();
    }
}

