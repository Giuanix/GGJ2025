using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectNumberPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] screen;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private GameObject headSprite;
    public ManagerTry manager;
    [SerializeField] private float waitFrame = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        FindObjectOfType<SelectLevel>().enabled = false;
        manager = ManagerTry.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard Control
        if(Input.GetKeyDown(KeyCode.S))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y -140);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            Press();
        }

        //Gamepad Control
        foreach(var gamepad in Gamepad.all)
        {
            if(gamepad.dpad.down.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y -140);
            }
            if(gamepad.dpad.up.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
            }
            if(gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }

        if(pointer.anchoredPosition.y > 70)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-140);
        }
        if(pointer.anchoredPosition.y < -70)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
        }
    }
    void Press()
    {
        switch (pointer.anchoredPosition.y)
        {
            case 70:
                Invoke("Select2Player",waitFrame);
                break;

            case -70:
                Invoke("Select4Player",waitFrame);
                break;
        } 
    }
    public void Select2Player()
    {
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        headSprite.SetActive(true);
        manager.maxPlayer = 2;
        FindObjectOfType<SelectLevel>().enabled = true;
        enabled = false;
    }
    public void Select4Player()
    {
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        headSprite.SetActive(false);
        manager.maxPlayer = 4;
        FindObjectOfType<SelectLevel>().enabled = true;
        enabled = false;
    }
}
