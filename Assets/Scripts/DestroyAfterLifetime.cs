using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
    [SerializeField] private float lifetimeInSeconds = 1f;

    private void Start()
    {
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(lifetimeInSeconds);
        Destroy(gameObject);
    }
}
