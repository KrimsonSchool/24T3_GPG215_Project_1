using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : FadeCanvasGroup
{
    protected void OnEnable()
    {
        GameManager.RoomTransitionStarting += OnRoomLevelChanging;
    }

    protected void OnDisable()
    {
        GameManager.RoomTransitionStarting -= OnRoomLevelChanging;
    }

    protected virtual void OnRoomLevelChanging()
    {
        StartFade(FadeOptions.FadeIn, 0.4f, 0.4f);
        canvasGroup.blocksRaycasts = true;
    }
}
