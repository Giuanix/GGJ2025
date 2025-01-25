using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class SwitchBotton : MonoBehaviour
{
    Image button;
    [SerializeField] Sprite unclicked;
    [SerializeField] Sprite clicked;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
