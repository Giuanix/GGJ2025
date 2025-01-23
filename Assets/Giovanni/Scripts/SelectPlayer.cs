using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class SelectPlayer : MonoBehaviour
{
    public static SelectPlayer instance;
    public SwitcherPlayer switcherManager;

    [Header("Prefab Player in Game Object")]
    [Tooltip("Should be the same amount of character in the same order: \n\n Duck\nWhale\n3pg")]
    [SerializeField] private GameObject[] prefabPlayer;
    private int[] selectionIndex = { -1, -1 };
    [HideInInspector] public GameObject currentPrefab1; 
    [HideInInspector] public GameObject currentPrefab2;
    [SerializeField] private Sprite[] portraits;
    [Header("Prefab Player1")]
    public Image iconPlayer1;

    [Header("Prefab Player2")]
    public Image iconPlayer2;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        switcherManager = SwitcherPlayer.instance;
    }

    void Update()
    {
        if(Input.GetKeyDown("1") && switcherManager.index == 0)
        {
            selectionIndex[0]++;
            if (selectionIndex[0] == prefabPlayer.Length)
                selectionIndex[0] = 0;


            iconPlayer1.sprite = portraits[selectionIndex[0]];
            currentPrefab1 = prefabPlayer[selectionIndex[0]];
        }
        else if(Input.GetKeyDown("2") && switcherManager.index == 1)
        {
            selectionIndex[1]++;
            if (selectionIndex[1] == prefabPlayer.Length)
                selectionIndex[1] = 0;


            iconPlayer2.sprite = portraits[selectionIndex[1]];
            currentPrefab2 = prefabPlayer[selectionIndex[1]];
        }
    }
}
