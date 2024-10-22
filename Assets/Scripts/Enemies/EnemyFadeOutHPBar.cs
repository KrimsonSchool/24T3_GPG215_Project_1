using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFadeOutHPBar : FadeCanvasGroup
{
    private void OnEnable()
    {
        EnemyCombatHandler.EnemyDeadEvent += StartFade;
    }

    private void OnDisable()
    {
        EnemyCombatHandler.EnemyDeadEvent -= StartFade;
    }
}
