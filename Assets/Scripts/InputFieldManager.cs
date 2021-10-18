using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Text placeHolderText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) Application.Quit();

        if (Input.GetButtonDown("Submit"))
        {
            if (playerNameText.text.Length < 3)
            {
                inputField.Select();
                inputField.text = "";
                placeHolderText.text = "Le nom doit avoir au moins 3 caractères";
            }
            else
            {
                GameManager.instance.SetPlayerName(playerNameText.text);
                GameManager.instance.StartNextlevel(0.2f);
            }
        }
    }
}
