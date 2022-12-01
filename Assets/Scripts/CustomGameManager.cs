using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class CustomGameManager : GameManager, MMEventListener<MMGameEvent>,
        MMEventListener<CorgiEngineEvent>,
        MMEventListener<CorgiEnginePointsEvent>
{
    /// <summary>
    /// Catches CorgiEngineEvents and acts on them, playing the corresponding sounds
    /// </summary>
    /// <param name="engineEvent">CorgiEngineEvent event.</param>
    public override void OnMMEvent(CorgiEngineEvent engineEvent)
    {
        switch (engineEvent.EventType)
        {
            case CorgiEngineEventTypes.TogglePause:
                if (Paused)
                {
                    CorgiEngineEvent.Trigger(CorgiEngineEventTypes.UnPause);
                }
                else
                {
                    CorgiEngineEvent.Trigger(CorgiEngineEventTypes.Pause);
                }
                break;

            case CorgiEngineEventTypes.Pause:
                Pause();
                break;

            case CorgiEngineEventTypes.UnPause:
                UnPause();
                break;

        }
    }
}
