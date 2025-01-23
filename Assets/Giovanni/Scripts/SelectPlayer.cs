using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
public class SelectPlayer : MonoBehaviour
{
    public static SelectPlayer instance;
    public SwitcherPlayer switcherManager;

    [Header("Prefab Player in Game Object")]
    [SerializeField] private GameObject prefabDuck;
    [SerializeField] private GameObject prefabWhale;
    [HideInInspector] public GameObject currentPrefab1; 
    [HideInInspector] public GameObject currentPrefab2;

    [Header("Prefab Player1")]
    public GameObject iconDuckPlayer1;
    public GameObject iconWhalePlayer1;

    [Header("Prefab Player2")]
    public GameObject iconDuckPlayer2;
    public GameObject iconWhalePlayer2;
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
        if(Input.GetKeyDown("1"))
        {
            currentPrefab1 = prefabDuck;
            if(currentPrefab1 = prefabDuck)
            {
                if(switcherManager.index == 0)
                {
                    iconDuckPlayer1.SetActive(true);
                    iconWhalePlayer1.SetActive(false);
                }
            }

            currentPrefab2 = prefabDuck;
            if(currentPrefab2 = prefabDuck)
            {
                if(switcherManager.index == 1)
                {
                    iconDuckPlayer2.SetActive(true);
                    iconWhalePlayer2.SetActive(false);
                }
            }
        }
        else if(Input.GetKeyDown("2"))
        {
            currentPrefab1 = prefabWhale;
            if(currentPrefab1 = prefabWhale)
            {
                if(switcherManager.index == 0)
                {
                    iconDuckPlayer1.SetActive(false);
                    iconWhalePlayer1.SetActive(true);
                }
            }
            currentPrefab2 = prefabWhale;
            if(currentPrefab2 = prefabWhale)
            {
                if(switcherManager.index == 1)
                {
                    iconDuckPlayer2.SetActive(false);
                    iconWhalePlayer2.SetActive(true);
                }
            }
        }
    }
}
