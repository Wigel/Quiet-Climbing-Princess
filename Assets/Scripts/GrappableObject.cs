using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class GrappableObject : Ledge
{
    protected const string _grapplingProjectileTag = "GrapplingProjectile";
    public bool isOnLedge = false;
    public bool canGrabLedge = false;

    [SerializeField]
    private float verticalHangingOffset = -0.7f;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(_grapplingProjectileTag) && !isOnLedge)
        {
            // if a grappling projectile hits a ledge, we want to update the ledge hang offset to the projectile's position
            HangOffset = collider.transform.position - transform.position;

            // We also want to make it possible to hang on the ledge.
            canGrabLedge = true;
        }

        if (!canGrabLedge)
            return;

        if (collider.CompareTag(_playerTag) && !isOnLedge)
        {
            // When the player collides with the ledge, we want to make sure the ledge hang offset is updated to fit the hanging animation
            HangOffset = new Vector3(HangOffset.x, HangOffset.y + verticalHangingOffset, HangOffset.z);
            LedgeEvent.Trigger(collider, this);
            isOnLedge = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(_playerTag))
        {
            //LedgeEvent.Trigger(collider, null);
            isOnLedge = false;
            canGrabLedge = false;
        }
    }
}
