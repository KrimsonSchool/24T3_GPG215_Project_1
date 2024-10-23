using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeCanvasGroup : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    public enum FadeOptions { FadeIn, FadeOut }

    [SerializeField] protected FadeOptions fadeType = FadeOptions.FadeOut;
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
            if (fadeType == FadeOptions.FadeIn)
            {
                canvasGroup.alpha = 0f;
            }
            else
            {
                canvasGroup.alpha = 1f;
            }
            StartFade();
        }
    }

    protected virtual void StartFade()
    {
        if (canvasGroup != null)
        {
            StartCoroutine(FadeCanvas(fadeType, fadeSpeed, delayBeforeFade));
        }
    }

    public virtual void StartFade(FadeOptions type, float speed, float delay = 0f)
    {
        if (canvasGroup != null)
        {
            StartCoroutine(FadeCanvas(type, speed, delay));
        }
    }

    protected virtual IEnumerator FadeCanvas(FadeOptions type, float speed, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (type == FadeOptions.FadeOut)
        {
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime / speed;
                yield return null;
            }
        }
        else
        {
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.unscaledDeltaTime / speed;
                yield return null;
            }
        }
    }
}