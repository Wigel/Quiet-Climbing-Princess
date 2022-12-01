using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class GrapplingHookWeapon : ProjectileWeapon
{

    [MMInspectorGroup("Hook logic", true, 66)]

    public float hookSpeed = 10f;
    public float returnHookSpeed = 20f;
    public float hookMaxDistance = 10f;
    public int maxNumberOfHooks = 1;

    private GrapplingHookProjectile currentHookProjectile;

    protected override void WeaponUse()
    {
        if ((ObjectPooler as MMSimpleObjectPooler).GetActivePooledGameObjectsCount() < maxNumberOfHooks)
        {
            base.WeaponUse();
        }
        else if ((ObjectPooler as MMSimpleObjectPooler).GetActivePooledGameObjectsCount() == maxNumberOfHooks)
        {
            currentHookProjectile.ReturnToPlayer();
        }
    }

    public override GameObject SpawnProjectile(Vector3 spawnPosition, int projectileIndex, int totalProjectiles, bool triggerObjectActivation = true)
    {
        // Spawn the projectile with base logic
        GameObject spawnedProjectile = base.SpawnProjectile(spawnPosition, projectileIndex, totalProjectiles, triggerObjectActivation);

        currentHookProjectile = spawnedProjectile.GetComponent<GrapplingHookProjectile>();

        // Initialize the projectile
        currentHookProjectile.InitializeHook(hookSpeed, returnHookSpeed, hookMaxDistance);

        return spawnedProjectile;
    }

    public GameObject GetReticle()
    {
        return _aimableWeapon._reticle;
    }
}
