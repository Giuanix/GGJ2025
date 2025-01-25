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
        if(Input.GetKeyDown(KeyCode.Escape))
            {
                comandScreen.SetActive(false);
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-170);
            }
            if(Input.GetKeyDown(KeyCode.W))
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+170);
            }
            if(Input.GetKeyDown(KeyCode.J))
            {
                Press();
            }
        foreach(var gamepad in Gamepad.all)
        {
            if(Input.GetKeyDown(KeyCode.I)|| gamepad.buttonEast.wasPressedThisFrame)//KeyCode.Escape
            {
                comandScreen.SetActive(false);
            }
            if(gamepad.dpad.down.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-170);
            }
            if(gamepad.dpad.up.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+170);
            }
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
            inizia.sprite = clicked;
            Invoke("SelectInizia",waitFrame); 
                break;

            case -250:
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
    }
    public void SelectComandi()
    {
        comandi.sprite = unclicked;
        comandScreen.SetActive(true);
    }
}
