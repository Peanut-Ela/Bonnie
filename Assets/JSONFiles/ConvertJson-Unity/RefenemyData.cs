using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefenemyData : MonoBehaviour
{
    public string enemy;
    public float detectionRange;
    public float health;

    public float damage;
    public float fadeDuration;

    public float fadeIterations;

    public int idleDurationMin;
    public int idleDurationMax;

    public float moveSpeed;
    public float chargeCooldown;

    public bool canCharge;

    public float attackRange;
    public float attackDuration;

    public string currentstate;

    public float bufferedState;
}
