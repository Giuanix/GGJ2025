using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private GameObject[] screen;
    [SerializeField] private GameObject[] stage;
    [SerializeField] private RectTransform pointer;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<ManagerTry>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var gamepad in Gamepad.all)
        {
            //Movement in a screen
            if(Input.GetKeyDown("d") || gamepad.dpad.right.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x+200, pointer.anchoredPosition.y);
            }
            if(Input.GetKeyDown("a") || gamepad.dpad.left.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x-200, pointer.anchoredPosition.y);
            }

            if(pointer.anchoredPosition.x > 200)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x-200, pointer.anchoredPosition.y);
            }
            if(pointer.anchoredPosition.x < -100)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x +200, pointer.anchoredPosition.y);
            }

            if(Input.GetKeyDown("j") || gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }
    }
    void Press()
    {
        switch (pointer.anchoredPosition.x)
        {
            case -100:
                screen[0].SetActive(false);
                screen[1].SetActive(true);
                stage[0].SetActive(true);
                FindObjectOfType<ManagerTry>().enabled = true;
                enabled = false;
                break;

            case 100:
                screen[0].SetActive(false);
                screen[1].SetActive(true);
                stage[1].SetActive(true);
                FindObjectOfType<ManagerTry>().enabled = true;
                enabled = false;
                break;
        } 
    }
}
