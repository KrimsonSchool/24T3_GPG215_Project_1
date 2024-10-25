using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenFader : FadeCanvasGroup
{
    protected void OnEnable()
    {
        GameManager.StartRoomTransition += OnRoomLevelChanging;
    }

    protected void OnDisable()
    {
        GameManager.StartRoomTransition -= OnRoomLevelChanging;
    }

    protected virtual void OnRoomLevelChanging()
    {
        StartFade(FadeOptions.FadeIn, 0.4f, 0.4f);
    }
}
