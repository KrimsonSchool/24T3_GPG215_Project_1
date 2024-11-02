using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFadeOutHPBar : FadeCanvasGroup
{
    private void OnEnable()
    {
        EnemyStats.EnemyDied += StartFade;
    }

    private void OnDisable()
    {
        EnemyStats.EnemyDied -= StartFade;
    }
}
