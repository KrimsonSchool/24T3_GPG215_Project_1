using System.Collections;
using UnityEngine;

public abstract class EnemyAttack : ScriptableObject
{
    protected static readonly PlayerCombatStates[] playerDodgeStates = { PlayerCombatStates.DodgingRight, PlayerCombatStates.DodgingLeft, PlayerCombatStates.DodgingUp };

    public abstract IEnumerator Attack(float attackWindup, float attackWarning, int attackDamage, AudioClip attackClip);
}
