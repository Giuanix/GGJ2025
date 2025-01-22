using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
public class SelectPlayer : MonoBehaviour
{
    public static SelectPlayer instance;
    [SerializeField] private GameObject prefabDuck;
    [SerializeField] private GameObject prefabWhale;
    public GameObject currentPrefab1; 
    public GameObject currentPrefab2;

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            currentPrefab1 = prefabDuck;
            currentPrefab2 = prefabDuck;
        }
        else if(Input.GetKeyDown("2"))
        {
            currentPrefab1 = prefabWhale;
            currentPrefab2 = prefabWhale;
        }
    }
}
