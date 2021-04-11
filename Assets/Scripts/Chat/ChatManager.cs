using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;

    public GameObject content;

    public GameObject prefabText;
    public InputField inputField;
    public int textCount;
    private List<string> texts = new List<string>();

    public void Init()
    {
        Instance = this;
    }

    public void OnEndEdit()
    {
        // If clicked to enter or keypad enter
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // If text is not empty
            if (inputField.text.Trim() != "")
            {
                string text = inputField.text.Trim();

                texts.Add(text);
                if (texts.Count > textCount)
                {
                    texts.RemoveAt(0);
                    Destroy(content.transform.GetChild(0).gameObject);
                }

                // Show the text in the chat box
                ShowText(text);

                // Clear the message
                inputField.text = "";
            }

            // Reactivate the input field
            inputField.ActivateInputField();
        }
    }

    public void ShowText(string _text)
    {
        GameObject textObj = Instantiate(prefabText, content.transform);
        textObj.GetComponent<Text>().text = _text;
    }

    public void ShowText(string _text, Color _color)
    {
        GameObject textObj = Instantiate(prefabText, content.transform);
        Text textComponent = textObj.GetComponent<Text>();
        textComponent.text = _text;
        textComponent.color = _color;
    }
}
