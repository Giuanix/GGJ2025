using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private GameObject[] screen;
    [SerializeField] private GameObject[] stage;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private float waitFrame = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<ManagerTry>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard Control
        if(Input.GetKeyDown(KeyCode.D))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x+200, pointer.anchoredPosition.y);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x-200, pointer.anchoredPosition.y);
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            Press();
        }

        //Gamepad Control
        foreach(var gamepad in Gamepad.all)
        {
            if(gamepad.dpad.right.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x+200, pointer.anchoredPosition.y);
            }
            if(gamepad.dpad.left.wasPressedThisFrame)
            {
                pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x-200, pointer.anchoredPosition.y);
            }
            if(gamepad.buttonSouth.wasPressedThisFrame)
            {
                Press();
            }
        }

        if(pointer.anchoredPosition.x > 200)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x-200, pointer.anchoredPosition.y);
        }
        if(pointer.anchoredPosition.x < -100)
        {
            pointer.anchoredPosition = new Vector2(pointer.anchoredPosition.x +200, pointer.anchoredPosition.y);
        }
    }
    public void Press()
    {
        switch (pointer.anchoredPosition.x)
        {
            case -100:
                Invoke("SelectStage1",waitFrame);
                break;

            case 100:
                Invoke("SelectStage2",waitFrame);
                break;
        } 
    }

    public void SelectStage1()
    {
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        stage[0].SetActive(true);
        FindObjectOfType<ManagerTry>().enabled = true;
        enabled = false;
    }
    public void SelectStage2()
    {
        screen[0].SetActive(false);
        screen[1].SetActive(true);
        stage[1].SetActive(true);
        FindObjectOfType<ManagerTry>().enabled = true;
        enabled = false;
    }
}
