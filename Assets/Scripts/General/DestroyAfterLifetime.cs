using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
    [SerializeField] private float lifetimeInSeconds = 1f;
    [SerializeField] private bool countdownOnStart = true;

    public float Lifetime { get { return lifetimeInSeconds; } }

    private void Start()
    {
        if (countdownOnStart)
        {
            StartLifetime();
        }
    }

    public void StartLifetime()
    {
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(lifetimeInSeconds);
        Destroy(gameObject);
    }
}
