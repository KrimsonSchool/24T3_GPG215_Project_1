using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCombatHandler : EnemyCombatHandler
{
    private ParticleSystem _particleSystem;
    [SerializeField] private AudioClip featherClip;
    [SerializeField] private AudioClip cluckingClip;

    private void OnEnable()
    {
        EnemyStats.EnemyDied += RewardPlayer;
    }
    private void OnDisable()
    {
        EnemyStats.EnemyDied -= RewardPlayer;
    }

    protected override void FindReferences()
    {
        base.FindReferences();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected override IEnumerator Attack()
    {
        _particleSystem.Play();
        AudioManager.Instance.PlaySoundEffect2D(featherClip);
        yield return base.Attack();
        AudioManager.Instance.PlaySoundEffect2D(cluckingClip, 1, Random.Range(0.9f, 1.1f));
    }

    public void RewardPlayer()
    {
        print("Rewarding boss kill");
        FindObjectOfType<PlayerStats>().CurrentHealth += (FindObjectOfType<PlayerStats>().CurrentHealth / 100) * 30;
    }
}
