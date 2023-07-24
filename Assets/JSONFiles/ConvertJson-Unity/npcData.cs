using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcData : MonoBehaviour
{

    public int npcId { get; }
    public string dialogueText { get; }
    public string dialogueName { get; }
    public string speakerImage { get; }
    public float wordSpeed { get; }
    public string playerIsClose { get; }
    public string dialogueRange { get; }
    public string contButton { get; }
    public string choiceBox { get; }
    public string pitchIncreaseInterval { get; }
    public string pitchVanriance { get; }
    public string idleDuratonMinl { get; }
    public string idleDurationMix { get; }
    public float moveSpeed { get; }
    public float nextDialogueId { get; }

    public npcData(int npcId, string dialoguetext, string dialogueName, string speakerImage, float wordSpeed, string playerIsClose, string dialogueRange, string contButton, string choiceBox, string pitchIncreaseInterval, string pitchVanriance, string idleDuratonMinl, string idleDurationMix, float moveSpeed, float nextDialogueId)
    {
        this.npcId = npcId;
        this.dialogueText = dialoguetext;
        this.dialogueName = dialogueName;
        this.speakerImage = speakerImage;
        this.wordSpeed = wordSpeed;
        this.playerIsClose = playerIsClose;
        this.dialogueRange = dialogueRange;
        this.contButton = contButton;
        this.choiceBox = choiceBox;
        this.pitchIncreaseInterval = pitchIncreaseInterval;
        this.pitchVanriance = pitchVanriance;
        this.idleDuratonMinl = idleDuratonMinl; 
        this.idleDurationMix = idleDurationMix;
        this.moveSpeed = moveSpeed;
        this.nextDialogueId = nextDialogueId;

    }
}
