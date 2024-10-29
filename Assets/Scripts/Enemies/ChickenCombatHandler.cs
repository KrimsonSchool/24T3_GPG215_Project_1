using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChickenCombatHandler : EnemyCombatHandler
{
    private ParticleSystem _particleSystem;

    protected override void FindReferences()
    {
        base.FindReferences();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected override IEnumerator Attack()
    {
        _particleSystem.Play();
        return base.Attack();
    }
}
