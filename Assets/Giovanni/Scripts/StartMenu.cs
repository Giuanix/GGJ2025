using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class StartMenu : MonoBehaviour
{
    public RectTransform pointer;
    public GameObject comandScreen;
    void Start()
    {
        comandScreen.SetActive(false);
    }
    void Update()
    {
        foreach(var gamepad in Gamepad.all)
        {
            if(Input.GetKeyDown("i")|| gamepad.buttonEast.wasPressedThisFrame)//KeyCode.Escape
            {
                comandScreen.SetActive(false);
            }
            //Movement in a screen
            if(Input.GetKeyDown("s") || gamepad.dpad.down.wasPressedThisFrame)
            {
                Debug.Log("Vai Giu");
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-170);
            }
            if(Input.GetKeyDown("w") || gamepad.dpad.up.wasPressedThisFrame)
            {
                Debug.Log("Vai Su");
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+170);
            }

            if(pointer.anchoredPosition.y > -80)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-170);
            }
            if(pointer.anchoredPosition.y < -420)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x , pointer.anchoredPosition.y+170);
            }

            if(Input.GetKeyDown("j") || gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }
    }
    public void Press()
    {
        switch (pointer.anchoredPosition.y)
        {
            case -80:
                Debug.Log("Inizia");
                SceneManager.LoadScene(1);
                break;

            case -250:
                Debug.Log("Comandi");
                comandScreen.SetActive(true);
                break;

            case -420:
                Debug.Log("Esci");
                Application.Quit();
                break;
        }
    }
}
