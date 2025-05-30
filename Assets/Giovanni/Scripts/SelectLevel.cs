using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class SelectLevel : MonoBehaviour
{
    public static SelectLevel instance;
    [Header("Game Menu screen")]
    [SerializeField] private GameObject[] screen;

    [Header("Stage")]
    [SerializeField] private GameObject[] stage;

    [Header("Components")]
    [SerializeField] private RectTransform pointer;
    [SerializeField] private float waitFrame = 0.2f;
    [HideInInspector] public int selectedStage;

    [Header("Botton Image")]
    public Image back;

    [Header("Botton sprite")]
    public Sprite backClicked;
    public Sprite backUnclicked;

    [Header("Water Color")]
    public Color waterColorBeach = new Color(0f, 0.769f, 1f, 1f);
    public Color waterColorBath = new Color(0.31f, 0.863f, 0.878f, 1f);
    public Color waterColorPool = new Color(0.31f, 0.403f, 0.878f, 1f);
    public Color waterColorSewer = new Color(0.427f, 0.6f, 0.184f, 1f);
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.visible = false;
        FindObjectOfType<ManagerTry>().enabled = false;
        FindObjectOfType<SelectNumberPlayer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard Control
        //Torna alla schermata di selezione Player
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            back.sprite = backClicked;
            AudioManager.instance.PlayBottonPressed();
            Invoke("SchermataPrecedente",waitFrame);
        }

        //Scorri nella schermata in orizzontale
        if(Input.GetKeyDown(KeyCode.D))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x+200, pointer.anchoredPosition.y);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x-200, pointer.anchoredPosition.y);
        }

        //Scorri nella schermata in verticale
        if(Input.GetKeyDown(KeyCode.W))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-140);
        }
        //Conferma Scelta
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Press();
        }

        //Gamepad Control
        foreach(var gamepad in Gamepad.all)
        {
            //Torna alla schermata di selezione Player
             if(gamepad.buttonEast.wasPressedThisFrame)
            {
                back.sprite = backClicked;
                AudioManager.instance.PlayBottonPressed();
                Invoke("SchermataPrecedente",waitFrame);
            }
            //Scorri nella schermata orizzontale
            if(gamepad.dpad.right.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x+200, pointer.anchoredPosition.y);
            }
            if(gamepad.dpad.left.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x-200, pointer.anchoredPosition.y);
            }
            //Scorri la Schermata in verticale
            if(gamepad.dpad.up.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
            }
            if(gamepad.dpad.down.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-140);
            }
            //Conferma Scelta
            if(gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }

        //Controllo per non uscire dalla schermata
        if(pointer.anchoredPosition.x > 200)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x-400, pointer.anchoredPosition.y);
        }
        if(pointer.anchoredPosition.x < -100)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x +400, pointer.anchoredPosition.y);
        }
        if(pointer.anchoredPosition.y > 140)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-280);
        }
        if(pointer.anchoredPosition.y < 0)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+280);
        }
    }
    public void Press()
    {
        switch (pointer.anchoredPosition.x, pointer.anchoredPosition.y)
        {
            case (-100, 140):
                Debug.Log("Stage 1 Selezionato");
                AudioManager.instance.PlayBottonPressed();
                Invoke("SelectStage1",waitFrame);
                break;

            case (100, 140):
                Debug.Log("Stage 2 Selezionato");
                AudioManager.instance.PlayBottonPressed();
                Invoke("SelectStage2",waitFrame);
                break;

            case (-100, 0):
                Debug.Log("Stage 3 Selezionato");
                AudioManager.instance.PlayBottonPressed();
                Invoke("SelectStage3",waitFrame);
                break;
            
            case (100, 0):
                Debug.Log("Stage 4 Selezionato");
                AudioManager.instance.PlayBottonPressed();
                Invoke("SelectStage4",waitFrame);
                break;
        } 
    }

    //Seleziona lo Stage 1
    public void SelectStage1()
    {
        Debug.Log("Stage 1 Selezionato");
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        stage[0].SetActive(true);
        selectedStage = 1;
        FindObjectOfType<WaterLevel>().fillColor = waterColorBeach;
        FindObjectOfType<ManagerTry>().enabled = true;

        enabled = false;
    }
    //Seleziona lo Stage 2
    public void SelectStage2()
    {
        Debug.Log("Stage 2 Selezionato");
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        stage[1].SetActive(true);
        selectedStage = 2;
        FindObjectOfType<WaterLevel>().fillColor = waterColorBath;
        FindObjectOfType<ManagerTry>().enabled = true;

        enabled = false;
    }

    public void SelectStage3()
    {
        Debug.Log("Stage 3 Selezionato");
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        stage[2].SetActive(true);
        selectedStage = 3;
        FindObjectOfType<WaterLevel>().fillColor = waterColorPool;
        FindObjectOfType<ManagerTry>().enabled = true;

        enabled = false;
    }
    public void SelectStage4()
    {
        Debug.Log("Stage 4 Selezionato");
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        stage[3].SetActive(true);
        selectedStage = 4;
        FindObjectOfType<WaterLevel>().fillColor = waterColorSewer;
        FindObjectOfType<ManagerTry>().enabled = true;

        enabled = false;
    }
    public void SchermataPrecedente()
    {
        Debug.Log("Torna alla schermata di selezione Num Player");
        back.sprite = backUnclicked;
        FindObjectOfType<SelectNumberPlayer>().enabled = true;
        FindObjectOfType<SelectNumberPlayer>().screen[0].SetActive(true);
        FindObjectOfType<SelectNumberPlayer>().screen[1].SetActive(false);
    }
}
