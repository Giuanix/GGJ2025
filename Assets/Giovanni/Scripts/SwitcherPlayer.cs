using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitcherPlayer : MonoBehaviour
{
    public SelectPlayer managerPlayer;
    [SerializeField] PlayerInputManager manager;
    int index = 0;
    [SerializeField] List<GameObject> fighters = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        managerPlayer = SelectPlayer.instance;
        manager = GetComponent<PlayerInputManager>();
    }
    void Update()
    {
        fighters[0] = managerPlayer.currentPrefab1;
        fighters[1] = managerPlayer.currentPrefab2;

        manager.playerPrefab = fighters[index];
    }
    public void  SwitchNextSpawnCharacter(PlayerInput input)
    {
        index++;
        manager.playerPrefab = fighters[index];
    }
}
