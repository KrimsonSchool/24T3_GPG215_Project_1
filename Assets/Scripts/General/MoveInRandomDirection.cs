using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveInRandomDirection : MonoBehaviour
{
    private Vector3 destination;
    private float speed = 1.0f;

    private void Start()
    {
        destination = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }
}
