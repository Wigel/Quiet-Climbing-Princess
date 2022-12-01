using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class OldManController : MonoBehaviour
{
    private Character player;
    public Weapon grapplingHook;

    private AnimatedDialogueZone animatedDialogueZone;

    // Start is called before the first frame update
    void Start()
    {
        animatedDialogueZone = GetComponentInChildren<AnimatedDialogueZone>();
    }

    // After the first dialogue we want to give the player the grappling hook.
    public void GiveGrapplingHook()
    {
        if (!animatedDialogueZone._playing)
            return;

        player = LevelManager.Current.Players[0];

        // Update the players weapon to the grappling hook
        player.GetComponent<CharacterHandleWeapon>().ChangeWeapon(grapplingHook, null);
    }
}
