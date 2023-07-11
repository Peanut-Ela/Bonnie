using EnemyStates;
using System.Collections;
using UnityEngine;

public class Enemy_2 : Enemy
{
    public GameObject explosionPrefab;
    public Enemy enemy; // Reference to the Enemy component

    public override BaseState AttackState => new ExplodeState(this);
    public void DoAttack()
    {
        StartCoroutine(DoAttackSequence());
    }

    private IEnumerator DoAttackSequence()
    {
        yield return new WaitForSeconds(1f);

        GameObject explosion = GameObject.Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
        ExplodeEffect explodeEffect = explosion.GetComponent<ExplodeEffect>();

        // Check if the enemy has collided with the player during the attack sequence
        Collider2D collision = Physics2D.OverlapCircle(enemy.transform.position, enemy.detectionRange, LayerMask.GetMask("Player"));
        if (collision != null)
        {
            // Perform explode effect on collision with the player
            Player.instance.ApplyKnockback((Player.instance.transform.position - enemy.transform.position).normalized);
            explodeEffect.Explode();
            enemy.QueueState(new DeathState(enemy)); // Transition to the death state
        }
    }
}
