using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public GameObject content;

    public GameObject prefabText;
    public InputField inputField;
    public int textCount;
    private List<string> texts = new List<string>();
    
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

                GameObject textObj = Instantiate(prefabText, content.transform);
                textObj.GetComponent<Text>().text = text;

                // Clear the message
                inputField.text = "";
            }

            // Reactivate the input field
            inputField.ActivateInputField();
        }
    }
}
