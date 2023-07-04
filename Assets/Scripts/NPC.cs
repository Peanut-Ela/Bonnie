using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Image speakerImage;
    public float wordSpeed;
    public bool playerIsClose;
    public GameObject contButton;

    public AudioSource typingSound; // Reference to the audio source for the typing sound effect
    public float pitchIncreaseInterval = 3; // Number of letters after which the pitch should increase
    public float pitchVariance = 0.1f; // Maximum amount by which the pitch can vary

    [SerializeField]
    [Header("Dialogue Data")]
    private List<DialogueData> dialogueDataList = new List<DialogueData>();
    private int index;
    private Coroutine typingCoroutine;

    [System.Serializable]
    public class DialogueData
    {
        public string text;
        public Sprite speakerSprite;
    }

    // Add any additional functions or variables you need here

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartTyping();
            }
        }

        if (dialogueText.text == dialogueDataList[index].text)
        {
            contButton.SetActive(true);
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        speakerImage.sprite = null; // Clear the speaker image when dialogue is not active
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingSound.Stop(); // Stop the typing sound
        }
        dialoguePanel.SetActive(false);
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueText.text = "";
        speakerImage.sprite = dialogueDataList[index].speakerSprite; // Set the speaker image for this dialogue element
        typingCoroutine = StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        int letterCount = 0; // Count of letters typed
        foreach (char letter in dialogueDataList[index].text.ToCharArray())
        {
            dialogueText.text += letter;
            typingSound.pitch = Random.Range(1f - pitchVariance, 1f + pitchVariance); // Randomize the pitch
            letterCount++;

            // Check if the letter count is a multiple of the pitch increase interval
            if (letterCount % pitchIncreaseInterval == 0)
            {
                typingSound.pitch = 1f; // Reset the pitch to normal
            }

            typingSound.Play(); // Play the typing sound effect
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);

        if (index < dialogueDataList.Count - 1)
        {
            index++;
            StartTyping();
        }
        else
        {
            zeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
