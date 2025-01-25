using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectNumberPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] screen;
    [SerializeField] private RectTransform pointer;
    public ManagerTry manager;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SelectLevel>().enabled = false;
        manager = ManagerTry.instance;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var gamepad in Gamepad.all)
        {
            //Movement in a screen
            if(Input.GetKeyDown("s") || gamepad.dpad.down.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y -140);
            }
            if(Input.GetKeyDown("w") || gamepad.dpad.up.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
            }

            if(pointer.anchoredPosition.y > 70)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y-140);
            }
            if(pointer.anchoredPosition.y < -70)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x, pointer.anchoredPosition.y+140);
            }

            if(Input.GetKeyDown("j") || gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }
    }
    void Press()
    {
        switch (pointer.anchoredPosition.y)
        {
            case 70:
                screen[0].SetActive(false);
                screen[1].SetActive(true);
                manager.maxPlayer = 2;
                FindObjectOfType<SelectLevel>().enabled = true;
                enabled = false;
                break;

            case -70:
                screen[0].SetActive(false);
                screen[1].SetActive(true);
                manager.maxPlayer = 4;
                FindObjectOfType<SelectLevel>().enabled = true;
                enabled = false;
                break;
        } 
    }
}
