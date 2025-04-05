using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class StartMenu : MonoBehaviour
{
    [Header("Componenti")]
    [SerializeField] private GameObject bottonAudioSource;
    [SerializeField] private RectTransform pointer;
    [Header("Schermate")]
    
    [SerializeField] private GameObject comandScreen;
    [SerializeField] private GameObject creditScreen;
    

    [Header("Pulsanti")]
    [SerializeField] private Image inizia;
    [SerializeField] private Image comandi;
    [SerializeField] private Image credits;
    [SerializeField] private Image exit;

    [Header("sprite pulsanti")]
    //Sprite del pulsante "Inizia"
    [SerializeField] private Sprite iniziaClicked;
    [SerializeField] private Sprite iniziaUnclicked;

    //Sprite del pulsante "Comandi"
    [SerializeField] private Sprite comandiClicked;
    [SerializeField] private Sprite comandiUnclicked;

    //Sprite del pulsante "Credits"
    [SerializeField] private Sprite creditsClicked;
    [SerializeField] private Sprite creditsUnclicked;

    //Sprite del pulsante "Esci"
    [SerializeField] private Sprite exitClicked;
    [SerializeField] private Sprite exitUnclicked;
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
 
        //Cambia la posizione del cursore in base alla schermata e disattiva l'input audio
        if(comandScreen.activeSelf)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, -120);
            bottonAudioSource.SetActive(false);
            
        }
        else bottonAudioSource.SetActive(true);

        if(creditScreen.activeSelf)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, -280);
            bottonAudioSource.SetActive(false);            
        }
        else bottonAudioSource.SetActive(true);
    }
    public void Press()
    {
        switch (pointer.anchoredPosition.y)
        {
            case 40:
            Debug.Log("Inizia Gioco");
            AudioManager.instance.PlayBottonPressed();
            inizia.sprite = iniziaClicked;
            Invoke("SelectInizia",waitFrame); 
                break;

            case -120:
            Debug.Log("Schermata Comandi");
            AudioManager.instance.PlayBottonPressed();
            comandi.sprite = comandiClicked;
            Invoke("SelectComandi",waitFrame); 
                break;

            case -280:
            Debug.Log("Credits");
            AudioManager.instance.PlayBottonPressed();
            credits.sprite = creditsClicked;
            Invoke("SelectCredit",waitFrame); 
                break;
            
            case -440:
            Debug.Log("Esci");
            AudioManager.instance.PlayBottonPressed();
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
