using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
public class SelectNumberPlayer : MonoBehaviour
{
    public GameObject[] screen;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private GameObject headSprite;
    public ManagerTry manager;
    [SerializeField] private float waitFrame = 0.2f;
    public Image select2Player;
    public Image select4Player;
    public Sprite clicked;
    public Sprite Uncliked;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySchermataSelezionePersonaggio();
        Cursor.visible = false;
        FindObjectOfType<ManagerTry>().enabled = false;
        FindObjectOfType<SelectLevel>().enabled = false;
        manager = ManagerTry.instance;
    }

    // Update is called once per frame
    void Update()
    {
        #region Controlli Tastiera
        //Torna al Menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SchermataPrecedente();
        }
        //Scorri nella schermata
        if(Input.GetKeyDown(KeyCode.S))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y -140);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
        }
        //Conferma la tua scelta
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Press();
        }
        #endregion

        #region Controlli Gamepad
        foreach(var gamepad in Gamepad.all)
        {
            //Torna al Menu
            if(gamepad.buttonEast.wasPressedThisFrame)
            {
                SchermataPrecedente();
            }
            //Scorri nella schermata
            if(gamepad.dpad.down.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y -140);
            }
            if(gamepad.dpad.up.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
            }
            //Conferma la tua scelta
            if(gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }
        #endregion

        if(pointer.anchoredPosition.y > 70)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-140);
        }
        if(pointer.anchoredPosition.y < -70)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
        }
    }

    #region Funzioni
    void Press()
    {
        switch (pointer.anchoredPosition.y)
        {
            case 70:
                select2Player.sprite = clicked;
                Invoke("Select2Player",waitFrame);
                break;

            case -70:
                select4Player.sprite = clicked;
                Invoke("Select4Player",waitFrame);
                break;
        } 
    }
    public void Select2Player()
    {
        Debug.Log("Partita a 2 Giocatori");
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        headSprite.SetActive(true);
        select2Player.sprite = Uncliked;
        manager.maxPlayer = 2;
        FindObjectOfType<SelectLevel>().enabled = true;
        FindObjectOfType<ManagerTry>().enabled = false;
        enabled = false;
    }
    public void Select4Player()
    {
        Debug.Log("Partita a 4 Giocatori");
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        headSprite.SetActive(false);
        select4Player.sprite = Uncliked;
        manager.maxPlayer = 4;
        FindObjectOfType<SelectLevel>().enabled = true;
        FindObjectOfType<ManagerTry>().enabled = false;
        enabled = false;
    }

    private void SchermataPrecedente()
    {
        Debug.Log("Torna al menu pricipale");
        SceneManager.LoadScene(0);
    }
    #endregion
}
