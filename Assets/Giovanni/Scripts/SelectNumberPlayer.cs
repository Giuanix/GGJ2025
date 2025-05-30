using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SelectNumberPlayer : MonoBehaviour
{
    public static SelectNumberPlayer instance;
    [Header("Game Menu screen")]
    public GameObject[] screen;

    [Header("Components")]
    [SerializeField] private RectTransform pointer;
    [SerializeField] private GameObject headSprite;
    public ManagerTry manager;
    [SerializeField] private float waitFrame = 0.2f;

    [Header("Botton Image")]
    public Image select2Player;
    public Image select4Player;
    public Image back;

    [Header("Botton sprite")]
    public Sprite player2Clicked;
    public Sprite player4Clicked;
    public Sprite player2Uncliked;
    public Sprite player4Uncliked;
    public Sprite backClicked;
    public Sprite backUnclicked;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        
        Cursor.visible = false;
        FindObjectOfType<ManagerTry>().enabled = false;
        FindObjectOfType<SelectLevel>().enabled = false;
        manager = ManagerTry.instance;
    }
    private void Start()
    {
        AudioManager.instance.PlaySchermataSelezionePersonaggio();
    }

    // Update is called once per frame
    void Update()
    {
        #region Controlli Tastiera
        //Torna al Menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            back.sprite = backClicked;
            AudioManager.instance.PlayBottonPressed();
            Invoke("SchermataPrecedente",waitFrame);
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
                back.sprite = backClicked;
                AudioManager.instance.PlayBottonPressed();
                Invoke("SchermataPrecedente",waitFrame);
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
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-280);
        }
        if(pointer.anchoredPosition.y < -70)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+280);
        }
    }

    #region Funzioni
    void Press()
    {
        switch (pointer.anchoredPosition.y)
        {
            case 70:
                AudioManager.instance.PlayBottonPressed();
                select2Player.sprite = player2Clicked;
                Invoke("Select2Player",waitFrame);
                break;

            case -70:
                AudioManager.instance.PlayBottonPressed();
                select4Player.sprite = player4Clicked;
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
        select2Player.sprite = player2Uncliked;
        ManagerTry.instance.maxPlayer = 2;
        FindObjectOfType<SelectLevel>().enabled = true;
        ManagerTry.instance.enabled = false;
        enabled = false;
    }
    public void Select4Player()
    {
        Debug.Log("Partita a 4 Giocatori");
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        headSprite.SetActive(false);
        select4Player.sprite = player4Uncliked;
        ManagerTry.instance.maxPlayer = 4;
        FindObjectOfType<SelectLevel>().enabled = true;
        ManagerTry.instance.enabled = false;
        enabled = false;
    }

    private void SchermataPrecedente()
    {
        Debug.Log("Torna al menu pricipale");
        back.sprite = backUnclicked;
        SceneManager.LoadScene(0);
    }
    #endregion
}
