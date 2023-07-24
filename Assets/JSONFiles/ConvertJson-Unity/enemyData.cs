using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyData : MonoBehaviour
{
    public int enemy { get; }
    public int detectionrange { get; }
    public int health { get; }
    public int damage { get; }
    public int fadeDuration { get; }
    public int fadeIterations { get; }
    public float idleDurationMin { get; }
    public float idleDurationMax { get; }
    public float moveSpeed { get; }
    public float chargeCooldown { get; }
    public bool canCharge { get; }
    public int attackRange { get; }
    public int attackDur { get; }
    public string currentState { get; }
    public string bufferedState { get; }

    public enemyData(string dialogueId, string dialogueName, Sprite speakerSprite, string choices, string onChoicesSelected, string showChoiceBox, string hideChoiceBox, string choiceresponses, string nextLine, string choiceBox, int choiceIndex, string currentDialogue)
    {
        this.enemy = enemy;
        this.detectionrange = detectionrange;
        this.health = health;
        this.damage = damage;
        this.fadeDuration = fadeDuration;
        this.fadeIterations = fadeIterations;
        this.idleDurationMin = idleDurationMax;
        this.moveSpeed = moveSpeed;
        this.chargeCooldown = chargeCooldown;
        this.canCharge = canCharge;
        this.attackRange = attackRange;
        this.attackDur = attackDur;
        this.currentState = currentState;
        this.bufferedState = bufferedState;
    }
}


