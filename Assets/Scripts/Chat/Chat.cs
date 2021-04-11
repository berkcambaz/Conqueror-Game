using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public GameObject content;

    public GameObject prefabText;

    public InputField inputField;
    
    public void OnEndEdit()
    {
        // If clicked to enter or keypad enter, send the message, clear and activate the inputfield
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string text = inputField.text;

            GameObject textObj = Instantiate(prefabText, content.transform);
            textObj.GetComponent<Text>().text = text;

            // Clear the message and activate input field 
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }
}
