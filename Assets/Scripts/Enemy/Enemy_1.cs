using EnemyStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    //public GameObject bulletPrefab;
    //public Enemy enemy; // Reference to the Enemy component

    public override BaseState AttackState => new ChargeState(this);
    //public void DoAttack()
    //{
    //    StartCoroutine(DoAttackSequence());
    //}

    //private IEnumerator DoAttackSequence()
    //{
    //    yield return new WaitForSeconds(1f);

    //    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, moveDirection));
    //    Bullet bulletComponent = bullet.GetComponent<Bullet>();
    //    bulletComponent.target = Player.instance.transform; // singleton - access player by using Player.instance
    //}
}
