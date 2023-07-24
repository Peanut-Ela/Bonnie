using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NPCStates;
using System;

[System.Serializable]
public struct DialogueData
{
    //Convert string into list, json doesnt support list or sprite

    public string lineId;
    public string nextLineId;
    public string text;
    public string dialogueName;

    public string speakerSpriteStr;
    public Sprite speakerSprite; //use addressables

    public string choicesStr;
    public string choiceNextLineIdsStr;
    public string choiceColorsStr;
    public List<string> choices; // List of choices for the dialogue line
    public List<string> choiceNextLineIds; // List of nextLineIds corresponding to each choice
    public List<Color> choiceColors; // List of colors for each choice

    public void Parse()
    {

        if (!string.IsNullOrEmpty(choicesStr))
        {
            choices = new List<string>();
            string[] choiceSplit = choicesStr.Split('@');
            for (int i = 0; i < choiceSplit.Length; i++)
            {
                choices.Add(choiceSplit[i]);
            }
        }

        if (!string.IsNullOrEmpty(choiceNextLineIdsStr))
        {
            choiceNextLineIds = new List<string>();
            string[] choiceNextLineIdsSplit = choiceNextLineIdsStr.Split('@');
            for (int i = 0; i < choiceNextLineIdsSplit.Length; i++)
            {
                choiceNextLineIds.Add(choiceNextLineIdsSplit[i]);
            }
        }

        if (!string.IsNullOrEmpty(choiceColorsStr))
        {
            choiceColors = new List<Color>();
            string[] coloursSplit = choiceColorsStr.Split('@');
            for (int i = 0; i < coloursSplit.Length; i++)
            {
                Color colour;
                ColorUtility.TryParseHtmlString(choiceColorsStr, out colour);
                choiceColors.Add(colour);
            }
        }
    }
}


public class DialogueManager : MonoBehaviour
{

    // Singleton instance to access the DialogueManager from other scripts
    public static DialogueManager instance;
    private NPC npc;
    internal DialogueData currentDialogue;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogueName;
    public Image speakerImage;
    public float wordSpeed;


    public GameObject contButton;
    public GameObject choiceBox; // New choice box object


    public AudioSource typingSound;
    public float pitchIncreaseInterval = 3;
    public float pitchVariance = 0.1f;

    //show dialogue

    [SerializeField]
    [Header("Dialogue Data")]
    private Coroutine typingCoroutine;

    // New DialogueData struct with [System.Serializable] attribute

    public static DialogueData GetDialogueID(string id) => GameAssets.instance.dialogueList.Find(a => a.lineId == id);

    private void Awake()
    {
        // Ensure there is only one instance of the DialogueManager in the scene
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    public void StartTyping(NPC npc)
    {

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        this.npc = npc;
        dialogueText.text = "";
        dialogueName.text = currentDialogue.dialogueName;
        speakerImage.sprite = currentDialogue.speakerSprite;
        typingCoroutine = StartCoroutine(Typing());

    }

    IEnumerator Typing()
    {
        int letterCount = 0;
        bool skip = false;
        foreach (char letter in currentDialogue.text.ToCharArray())
        {
            if (letter == '<')
                skip = true;
            if (letter == '>')
                skip = false;

            dialogueText.text += letter;

            if (skip)
            {
                continue;
            }
            else
            {
                typingSound.pitch = UnityEngine.Random.Range(1f - pitchVariance, 1f + pitchVariance);
                letterCount++;
                if (letterCount % pitchIncreaseInterval == 0)
                {
                    typingSound.pitch = 1f;
                }

                typingSound.Play();
                // Wait for wordSpeed seconds, but check for spacebar input to skip
                float elapsedTime = 0f;
                while (elapsedTime < wordSpeed)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        // Skip the typing animation by instantly showing the entire text
                        dialogueText.text = currentDialogue.text;
                        break;
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
            //Skip when it starts at < until >
        }

        // Check if the current dialogue line has choices
        if (currentDialogue.choices != null && currentDialogue.choices.Count > 0)
        {
            ShowChoiceBox();
        }
        else
        {
            contButton.SetActive(true);
        }

        yield return null;
    }

    public void NextLine()
    {
        contButton.SetActive(false);
        string nextLineid = currentDialogue.nextLineId;

        if (string.IsNullOrEmpty(nextLineid))
        {
            zeroText();
            dialoguePanel.SetActive(false);
        }
        else
        {
            currentDialogue = GetDialogueID(currentDialogue.nextLineId); //incrementing
            StartTyping(npc);
        }

    }

    private void ShowChoiceBox()
    {
        if (choiceBox != null)
        {
            choiceBox.SetActive(true);
            // Set choice button texts and callbacks
            ChoiceBox choiceBoxComponent = choiceBox.GetComponent<ChoiceBox>();
            if (choiceBoxComponent != null)
            {
                choiceBoxComponent.SetChoices(currentDialogue.choices, OnChoiceSelected);
            }
        }
    }

    private void HideChoiceBox()
    {
        if (choiceBox != null)
        {
            choiceBox.SetActive(false);
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        Debug.Log("Choice selected. Choice index: " + choiceIndex);
        HideChoiceBox();

        //string nextLineid = currentDialogue.choiceNextLineIds[choiceIndex];

        currentDialogue = GetDialogueID(currentDialogue.choiceNextLineIds[choiceIndex]);
        // Continue with the next line after processing the choice
        StartTyping(npc);
    }

    public void zeroText()
    {

        dialogueText.text = "";
        dialogueName.text = "";
        speakerImage.sprite = null;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingSound.Stop();
        }
        dialoguePanel.SetActive(false);
        HideChoiceBox(); // Hide the choice box when resetting the dialogue

        npc.QueueState(npc.DefaultState);
        npc = null;
        StopAllCoroutines();
    }
}
