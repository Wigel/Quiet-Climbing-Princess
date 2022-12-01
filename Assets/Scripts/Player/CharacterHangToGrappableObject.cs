using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class CharacterHangToGrappableObject : CharacterLedgeHang
{
    [Tooltip("the jump height for the jump that happens when character loose its ledge")]
    [SerializeField]
    private float stopLedgeJumpHeight = 1f;

    private CharacterGrappling _characterGrappling;

    protected override void Initialization()
    {
        base.Initialization();

        _characterGrappling = GetComponent<CharacterGrappling>();
    }

    public override void StartGrabbingLedge(Ledge ledge)
    {
        // we make sure we can grab the ledge
        if (!AbilityAuthorized
            || (_movement.CurrentState == CharacterStates.MovementStates.Jetpacking))
        {
            return;
        }

        _characterGrappling.hasReachedTarget = true;

        // we start hanging from the ledge
        _ledgeHangingStartedTimestamp = Time.time;
        _ledge = ledge;
        _controller.CollisionsOff();
        PlayAbilityStartFeedbacks();
        _movement.ChangeState(CharacterStates.MovementStates.LedgeHanging);
        MMCharacterEvent.Trigger(_character, MMCharacterEventTypes.LedgeHang, MMCharacterEvent.Moments.Start);
    }

    public void StopGrabbingLedge()
    {
        float jumpHeight = _characterJump.JumpHeight;
        _characterJump.JumpHeight = stopLedgeJumpHeight;
        _characterJump.JumpStart();
        _characterJump.JumpHeight = jumpHeight;
    }
}
