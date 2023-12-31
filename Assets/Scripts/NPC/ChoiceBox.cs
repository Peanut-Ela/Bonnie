using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceBox : MonoBehaviour
{
    public GameObject choiceButtonPrefab;
    public Transform buttonContainer;

    private List<Button> choiceButtons = new List<Button>();
    private Action<int> choiceCallback;

    public void SetChoices(List<string> choices, Action<int> callback)
    {
        choiceCallback = callback;

        // Clear existing buttons
        foreach (Button button in choiceButtons)
        {
            Destroy(button.gameObject);
        }
        choiceButtons.Clear();

        // Create new buttons for each choice
        for (int i = 0; i < choices.Count; i++)
        {
            string choice = choices[i];
            GameObject buttonObj = Instantiate(choiceButtonPrefab, buttonContainer);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI choiceText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = choice;

            // Add a click listener to each button
            int choiceIndex = i; // Store the choice index in a local variable
            button.onClick.AddListener(() => OnChoiceClicked(choiceIndex));

            choiceButtons.Add(button);
        }
    }


    private void OnChoiceClicked(int choiceIndex)
    {
        Debug.Log("Choice button clicked. Choice index: " + choiceIndex);
        if (choiceCallback != null)
        {
            choiceCallback.Invoke(choiceIndex);
        }
    }
}
