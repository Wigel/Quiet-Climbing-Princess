using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class CharacterGrappling : CharacterAbility
{
    public override string HelpBoxText() { return "This component will allow the character to fly to a designated target."; }

    [Tooltip("the current target that we are flying towards")]
    public float minDistanceToReachTarget = 0.5f;

    [Tooltip("the current target that we are flying towards")]
    private Vector3 grapplingTargetPoint;
    private Transform grapplingTargetTransform;
    private GrapplingHookProjectile currentGrapplingHookProjectile;

    public bool hasReachedTarget = true;
    private CharacterFly _characterFly;
    private CharacterLedgeHang _characterLedgeHang;

    [Tooltip("the feedbacks to play when a grappable object has been set")]
    public MMFeedbacks GrappableObjectHitFeedbacks;

    protected override void Initialization()
    {
        base.Initialization();

        _characterFly = GetComponent<CharacterFly>();
        _characterLedgeHang = GetComponent<CharacterLedgeHang>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grapplingTargetTransform != null)
        {
            FlyToTarget();
        }
    }


    /// <summary>
    /// Move the player transform towards the grappling target
    /// </summary>
    private void FlyToTarget()
    {
        if (grapplingTargetTransform == null) return;

        if (_movement.CurrentState != CharacterStates.MovementStates.Flying)
            _characterFly.StartFlight();

        MoveTowardsTarget();

        if (hasReachedTarget)
        {
            grapplingTargetTransform = null;
            _characterFly.StopFlight();
            _characterFly.SetHorizontalMove(0);
            _characterFly.SetVerticalMove(0);
            currentGrapplingHookProjectile.Destroy();
            StopStartFeedbacks();
        }
    }

    /// <summary>
    /// Moves the character towards the target
    /// </summary>
    protected virtual void MoveTowardsTarget()
    {
        Vector3 direction = (grapplingTargetPoint - transform.position).normalized;
        _characterFly.SetHorizontalMove(direction.x);
        _characterFly.SetVerticalMove(direction.y);
        //Play the feedbacks for flying towards target
        PlayAbilityStartFeedbacks();
    }

    public void SetGrapplingTarget(Transform targetTransform, Vector3 _grapplingTargetPoint, GrapplingHookProjectile targettGrapplingHookProjectile)
    {
        _characterLedgeHang.DetachFromLedge();

        currentGrapplingHookProjectile = targettGrapplingHookProjectile;
        grapplingTargetTransform = targetTransform;
        grapplingTargetPoint = _grapplingTargetPoint;
        hasReachedTarget = false;
        _characterFly.StartFlight();

        // play feedbacks
        GrappableObjectHitFeedbacks?.PlayFeedbacks();
    }

    public void StopFlyingToTarget()
    {
        hasReachedTarget = true;
        grapplingTargetTransform = null;
        _characterFly.StopFlight();
        _characterFly.SetHorizontalMove(0);
        _characterFly.SetVerticalMove(0);
    }
}
