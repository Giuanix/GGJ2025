using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndex : MonoBehaviour
{
    [SerializeField] private int IndexPlayer;
    
    public int GetPlayerIndex()
    {
        return IndexPlayer;
    }
}
