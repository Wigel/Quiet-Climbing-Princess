using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class GrapplingHookProjectile : Projectile
{
    public LineRenderer lineRenderer;

    private float maxDistance;
    private bool isReturningToPlayer = false;
    private float returnHookSpeed;
    public float minDistanceToDestroy = 1f;

    [Tooltip("The tag representing objects that are grappable")]
    public string grappableObjectTag = "GrappableObject";

    [Tooltip("the feedback to play when the projectile hits a grappable object")]
    public MMFeedbacks WeaponStartMMFeedback;

    protected override void Initialization()
    {
        base.Initialization();
    }

    public void InitializeHook(float speed, float _returnHookSpeed, float _maxDistance)
    {
        isReturningToPlayer = false;
        Speed = speed;
        maxDistance = _maxDistance;
        returnHookSpeed = _returnHookSpeed;
    }

    protected override void Update()
    {
        base.Update();

        // To visualize the rope between the grappling hook projectile and the player we need to create a line renderer between them
        if (lineRenderer != null)
        {
            Vector3[] linePositions = new[] { transform.position, _weapon.GetReticle().transform.position };
            lineRenderer.SetPositions(linePositions);
        }

        if (!isReturningToPlayer && IsMaxDistanceReachead())
        {
            ReturnToPlayer();
        }

        if (isReturningToPlayer)
        {
            ReturnProjectileToPlayer();
        }
    }

    private bool IsMaxDistanceReachead()
    {
        return Vector2.Distance(transform.position, _owner.transform.position) > maxDistance;
    }

    private void ReturnProjectileToPlayer()
    {
        Speed = returnHookSpeed;
        Direction = (_owner.transform.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, _owner.transform.position) < minDistanceToDestroy)
        {
            Destroy();
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="collider">The Collider2D data associated with this collision.</param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == grappableObjectTag && !collider.GetComponent<GrappableObject>().isOnLedge)
        {
            // When the grapple projectile collide with a grappable object we need to stop the projectile and make the player stick to the object
            // Stop the projectile movement
            Speed = 0;

            // Make the player move towards the grappled object
            _owner.GetComponent<CharacterGrappling>().SetGrapplingTarget(collider.transform, collider.ClosestPoint(transform.position), this);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            // If the hook projectile collide with a platform we need it to start returning to the player
            ReturnToPlayer();
        }
    }

    public void ReturnToPlayer()
    {
        isReturningToPlayer = true;
        _owner.GetComponent<CharacterGrappling>().StopFlyingToTarget();
    }

}
