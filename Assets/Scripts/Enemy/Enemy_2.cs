using EnemyStates;
using System.Collections;
using UnityEngine;

public class Enemy_2 : Enemy
{
    public Enemy enemy; // Reference to the Enemy component
    public AudioSource audioSource;
    public AudioClip explosionSound;
    public static readonly int ExplodeKey = Animator.StringToHash("Enemy_Explode");

    public override BaseState AttackState => new ChargeExplosion(this);
    public void DoAttack()
    {
        StartCoroutine(DoAttackSequence());
    }

    private IEnumerator DoAttackSequence()
    {
        yield return new WaitForSeconds(1f);

        //GameObject explosion = GameObject.Instantiate(explosionPrefab, enemy.transform.position, Quaternion.identity);
        //ExplodeEffect explodeEffect = explosion.GetComponent<ExplodeEffect>();

        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
    }
}
