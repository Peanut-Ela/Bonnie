using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct DialogueData
{
    public string dialogueId;
    public string text;
    public string dialogueName;
    public Sprite speakerSprite;
    public string choices;
    public string onChoicesSelected;
    public string showChoiceBox;
    public string hideChoiceBox;
    public string choiceresponses;
    public string nextLine;
    public string choiceBox;
    public int choiceIndex;
    public string currentDialogue;
}
