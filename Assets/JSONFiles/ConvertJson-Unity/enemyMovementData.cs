using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovementData : MonoBehaviour
{
    public string enemy { get; }
    public float moveSpeed { get; }
    public float chargeSpeed { get; }
    public float idleTimer { get; }
    public float idleDuration { get; }
    public float idleDurationMax { get; }
    public float movespeed { get; }
    public float detectionRange { get; }
    public float chargeCoolDown { get; }
    public float attackRange { get; }
    public float attackDuration { get; }

    public enemyMovementData(string enemy, float moveSpeed, float chargeSpeed, float idleTimer, float idleDuration, float idleDurationMax, float movespeed, float detectionRange, float chargeCoolDown, float attackRange, float attackDuration)
    {
        this.enemy = enemy;
        this.moveSpeed = moveSpeed;
        this.chargeSpeed = chargeSpeed;
        this.idleTimer = idleTimer;
        this.idleDuration = idleDuration;
        this.idleDurationMax = idleDurationMax;
        this.movespeed = movespeed;
        this.detectionRange = detectionRange;
        this.chargeCoolDown = chargeCoolDown;
        this.attackRange = attackRange;
        this.attackDuration = attackDuration;
    }
}
