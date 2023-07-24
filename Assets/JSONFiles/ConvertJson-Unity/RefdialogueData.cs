using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RefdialogueData : MonoBehaviour
{
    public string dialogueId;
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
