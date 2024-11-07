using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private float maxHorizontalDistance = 100.0f;
    private GameObject horizontalMovePointAnimationReplacementCuzImDumbSadFace;
    private TextMeshProUGUI damageText;
    private float horizontalSpeed;
    private Vector3 destination;

    public Vector3 WorldSpawnPoint { get; set; }
    public bool WasBlocking { get; set; } = false;
    public int Damage { get; set; } = 420;

    private void Awake()
    {
        horizontalMovePointAnimationReplacementCuzImDumbSadFace = transform.GetChild(0).gameObject;
        damageText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (WasBlocking)
        {
            damageText.color = Color.grey;
        }

        if (Damage == 0)
        {
            damageText.text = $"Blocked";
        }
        else
        {
            damageText.text = $"-{Damage}HP";
        }

        // Dunno if I can randomize keyframes so did this... If something better pls change!
        destination = transform.position + new Vector3(Random.Range(-maxHorizontalDistance, maxHorizontalDistance), 0, 0);
        horizontalSpeed = (transform.position - destination).magnitude / GetComponent<DestroyAfterLifetime>().Lifetime;
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(WorldSpawnPoint);

        // Same as previous comment :(
        horizontalMovePointAnimationReplacementCuzImDumbSadFace.transform.position = Vector3.MoveTowards(horizontalMovePointAnimationReplacementCuzImDumbSadFace.transform.position, destination, Time.deltaTime * horizontalSpeed);
    }
}