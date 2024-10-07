using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeCanvasUI : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected enum FadeOptions { fadeIn, fadeOut }
    [SerializeField] protected FadeOptions fadeType = FadeOptions.fadeOut;
    [SerializeField, Range(0.01f, 10f)] protected float fadeSpeed = 1f;
    [SerializeField] protected float delayBeforeFade = 0f;
    [SerializeField] protected bool fadeOnStart = false;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        if (fadeOnStart)
        {
            StartFade();
        }
    }

    protected virtual void StartFade()
    {
        if (canvasGroup != null)
        {
            StartCoroutine(FadeCanvas());
        }
    }

    protected virtual IEnumerator FadeCanvas()
    {
        yield return new WaitForSecondsRealtime(delayBeforeFade);
        if (fadeType == FadeOptions.fadeOut)
        {
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime / fadeSpeed;
                yield return null;
            }
        }
        else
        {
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.unscaledDeltaTime / fadeSpeed;
                yield return null;
            }
        }
    }
}