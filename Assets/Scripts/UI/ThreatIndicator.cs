using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatIndicator : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroupRight;
    [SerializeField] private CanvasGroup canvasGroupLeft;
    [SerializeField] private CanvasGroup canvasGroupBelow;
    [SerializeField] private float fadeInSpeed = 4f;
    [SerializeField] private float fadeOutSpeed = 6f;

    private void OnEnable()
    {
        EnemyCombatHandler.EnemyAttackWarningEvent += TriggerThreatIndicator;
        EnemyCombatHandler.EnemyAttackEvent += RemoveIndicator;
        EnemyCombatHandler.EnemyDeadEvent += RemoveAllIndicators;
    }

    private void OnDisable()
    {
        EnemyCombatHandler.EnemyAttackWarningEvent -= TriggerThreatIndicator;
        EnemyCombatHandler.EnemyAttackEvent -= RemoveIndicator;
        EnemyCombatHandler.EnemyDeadEvent -= RemoveAllIndicators;
    }

    private void TriggerThreatIndicator(PlayerCombatStates requiredState)
    {
        StopAllCoroutines();
        RemoveAllIndicators();
        StartCoroutine(StartThreatIdication(requiredState));
    }

    private IEnumerator StartThreatIdication(PlayerCombatStates requiredState)
    {
        switch (requiredState)
        {
            case PlayerCombatStates.DodgingRight:
                while (canvasGroupLeft.alpha < 1f)
                {
                    canvasGroupLeft.alpha += Time.deltaTime * fadeInSpeed;
                    yield return null;
                }
                break;
            case PlayerCombatStates.DodgingLeft:
                while (canvasGroupRight.alpha < 1f)
                {
                    canvasGroupRight.alpha += Time.deltaTime * fadeInSpeed;
                    yield return null;
                }
                break;
            case PlayerCombatStates.DodgingUp:
                while (canvasGroupBelow.alpha < 1f)
                {
                    canvasGroupBelow.alpha += Time.deltaTime * fadeInSpeed;
                    yield return null;
                }
                break;
        }
        yield return null;
    }

    private void RemoveAllIndicators()
    {
        StopAllCoroutines();
        canvasGroupRight.alpha = 0f;
        canvasGroupLeft.alpha = 0f;
        canvasGroupBelow.alpha = 0f;
    }

    private void RemoveIndicator(int damage, PlayerCombatStates requiredState)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutIndicator(requiredState));
    }

    private IEnumerator FadeOutIndicator(PlayerCombatStates requiredState)
    {
        switch (requiredState)
        {
            case PlayerCombatStates.DodgingRight:
                while (canvasGroupLeft.alpha > 0f)
                {
                    canvasGroupLeft.alpha -= Time.unscaledDeltaTime * fadeOutSpeed;
                    yield return null;
                }
                break;
            case PlayerCombatStates.DodgingLeft:
                while (canvasGroupRight.alpha > 0f)
                {
                    canvasGroupRight.alpha -= Time.unscaledDeltaTime * fadeOutSpeed;
                    yield return null;
                }
                break;
            case PlayerCombatStates.DodgingUp:
                while (canvasGroupBelow.alpha > 0f)
                {
                    canvasGroupBelow.alpha -= Time.unscaledDeltaTime * fadeOutSpeed;
                    yield return null;
                }
                break;
        }
        yield return null;
    }
}
