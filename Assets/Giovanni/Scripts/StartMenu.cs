using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class StartMenu : MonoBehaviour
{
    public RectTransform pointer;
    public GameObject comandScreen;
    public Image inizia;
    public Image comandi;
    public Sprite clicked;
    public Sprite unclicked;
    [SerializeField] private float waitFrame = 0.2f;
    void Start()
    {
        Cursor.visible = false;
        comandScreen.SetActive(false);
    }
    void Update()
    {
        //Keyboard Control
        //Torna alla schermata principale
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esci da Schermata Comandi");
            comandScreen.SetActive(false);
        }
        //Scorri nella schermata
        if(Input.GetKeyDown(KeyCode.S))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-170);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+170);
        }
        //Conferma la tua scelta
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Press();
        }

        //Gamepad Control
        foreach(var gamepad in Gamepad.all)
        {
            //Torna alla schermata principale
            if(gamepad.buttonEast.wasPressedThisFrame)
            {
                comandScreen.SetActive(false);
            }
            //Scorri nella schermata
            if(gamepad.dpad.down.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-170);
            }
            if(gamepad.dpad.up.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+170);
            }
            //Conferma la tua scelta
            if(gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }

        if(pointer.anchoredPosition.y > -80)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-170);
        }
        if(pointer.anchoredPosition.y < -420)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x , pointer.anchoredPosition.y+170);
        }
    }
    public void Press()
    {
        switch (pointer.anchoredPosition.y)
        {
            case -80:
            Debug.Log("Inizia Gioco");
            inizia.sprite = clicked;
            Invoke("SelectInizia",waitFrame); 
                break;

            case -250:
            Debug.Log("Schermata Comandi");
            comandi.sprite = clicked;
            Invoke("SelectComandi",waitFrame); 
                break;

            case -420:
                Debug.Log("Esci");
                Application.Quit();
                break;
        }
    }

    public void SelectInizia()
    {
        SceneManager.LoadScene(1);
        inizia.sprite = unclicked;
    }
    public void SelectComandi()
    {
        comandi.sprite = unclicked;
        comandScreen.SetActive(true);
    }
}
