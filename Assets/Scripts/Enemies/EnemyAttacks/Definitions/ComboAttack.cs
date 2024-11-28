using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComboAttack", menuName = "Scriptable Objects/ComboAttack", order = 69)]
public class ComboAttack : EnemyAttack
{
    [SerializeField] private int amountOfAttacks = 2;
    [SerializeField] private float delayBetweenSuccessiveAttacks = 0.4f;

    public override IEnumerator Attack(float attackWindup, float attackWarning, int attackDamage, AudioClip attackClip)
    {
        PlayerCombatStates randomState = playerDodgeStates[Random.Range(0, playerDodgeStates.Length)];
        EnemyCombatHandler.OnAttackWindingUp(randomState);
        yield return new WaitForSeconds(attackWindup);
        for (int i = 0; i < amountOfAttacks; i++)
        {
            //Debug.LogWarning($"Enemy about to attack. Requires: {randomState}");
            EnemyCombatHandler.OnStartingAttackWarning(randomState);
            yield return new WaitForSeconds(attackWarning);
            EnemyCombatHandler.OnEnemyAttacked(attackDamage, randomState);
            if (attackClip != null)
            {
                AudioManager.Instance.PlaySoundEffect2D(attackClip, 1, Random.Range(0.9f, 1.1f));
            }
            if (amountOfAttacks > 1)
            {
                randomState = playerDodgeStates[Random.Range(0, playerDodgeStates.Length)];
                yield return new WaitForSeconds(delayBetweenSuccessiveAttacks);
            }
        }
        yield return null;
    }
}