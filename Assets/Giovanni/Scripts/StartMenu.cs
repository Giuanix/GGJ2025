using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class StartMenu : MonoBehaviour
{
    [Header("Schermate")]
    public RectTransform pointer;
    public GameObject comandScreen;
    public GameObject creditScreen;
    [Header("Pulsanti")]
    public Image inizia;
    public Image comandi;
    public Image credits;
    public Image exit;

    [Header("sprite pulsanti")]
    //Sprite del pulsante "Inizia"
    public Sprite iniziaClicked;
    public Sprite iniziaUnclicked;

    //Sprite del pulsante "Comandi"
    public Sprite comandiClicked;
    public Sprite comandiUnclicked;

    //Sprite del pulsante "Credits"
    public Sprite creditsClicked;
    public Sprite creditsUnclicked;

    //Sprite del pulsante "Esci"
    public Sprite exitClicked;
    public Sprite exitUnclicked;
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
            Debug.Log("Esci dalle schermate");
            comandScreen.SetActive(false);
            creditScreen.SetActive(false);
        }
        //Scorri nella schermata
        if(Input.GetKeyDown(KeyCode.S))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-160);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+160);
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
                Debug.Log("Esci dalle schermate");
                comandScreen.SetActive(false);
                creditScreen.SetActive(false);
            }
            //Scorri nella schermata
            if(gamepad.dpad.down.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-160);
            }
            if(gamepad.dpad.up.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+160);
            }
            //Conferma la tua scelta
            if(gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }

        //Limita il movimento del cursore
        if(pointer.anchoredPosition.y > 40)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-160);
        }
        if(pointer.anchoredPosition.y < -440)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x , pointer.anchoredPosition.y+160);
        }
 
        //Cambia la posizione del cursore in base alla schermata
        if(comandScreen.activeSelf)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, -120);
        }

        if(creditScreen.activeSelf)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, -280);
        }
    }
    public void Press()
    {
        switch (pointer.anchoredPosition.y)
        {
            case 40:
            Debug.Log("Inizia Gioco");
            inizia.sprite = iniziaClicked;
            Invoke("SelectInizia",waitFrame); 
                break;

            case -120:
            Debug.Log("Schermata Comandi");
            comandi.sprite = comandiClicked;
            Invoke("SelectComandi",waitFrame); 
                break;

            case -280:
            Debug.Log("Credits");
            credits.sprite = creditsClicked;
            Invoke("SelectCredit",waitFrame); 
                break;
            
            case -440:
            Debug.Log("Esci");
            exit.sprite = exitClicked;
            Invoke("Exit",waitFrame); 
                break;
        }
    }

    public void SelectInizia()
    {
        SceneManager.LoadScene(1);
        inizia.sprite = iniziaUnclicked;
    }
    public void SelectComandi()
    {
        comandi.sprite = comandiUnclicked;
        comandScreen.SetActive(true);
    }

    public void SelectCredit()
    {
        credits.sprite = creditsUnclicked;
        creditScreen.SetActive(true);
    }
    public void Exit()
    {
        exit.sprite = exitUnclicked;
        Application.Quit();
    }
}
