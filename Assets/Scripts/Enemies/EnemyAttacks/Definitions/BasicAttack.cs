using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBasicAttack", menuName = "Scriptable Objects/BasicAttack", order = 69)]
public class BasicAttack : EnemyAttack
{
    public override IEnumerator Attack(float attackWindup, float attackWarning, int attackDamage, AudioClip attackClip)
    {
        PlayerCombatStates randomState = playerDodgeStates[Random.Range(0, playerDodgeStates.Length)];
        EnemyCombatHandler.OnAttackWindingUp(randomState);
        yield return new WaitForSeconds(attackWindup);
        //Debug.LogWarning($"Enemy about to attack. Requires: {randomState}");
        EnemyCombatHandler.OnStartingAttackWarning(randomState);
        yield return new WaitForSeconds(attackWarning);
        EnemyCombatHandler.OnEnemyAttacked(attackDamage, randomState);
        if (attackClip != null)
        {
            AudioManager.Instance.PlaySoundEffect2D(attackClip, 1, Random.Range(0.9f, 1.1f));
        }
        yield return null;
    }
}
