using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitcherPlayer : MonoBehaviour
{
    public static SwitcherPlayer instance;
    public UI_Manager uiPlayer1;
    public UI_Manager uiPlayer2;
    public Transform spawnPointPlayer1;
    public Transform spawnPointPlayer2;
    public SelectPlayer managerPlayer;
    [SerializeField] PlayerInputManager manager;
    [HideInInspector] public int index = 0;
    [SerializeField] List<GameObject> fighters = new List<GameObject>();
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        uiPlayer1 = GameObject.FindGameObjectWithTag("UiPlayer1").GetComponent<UI_Manager>();
        uiPlayer2 = GameObject.FindGameObjectWithTag("UiPlayer2").GetComponent<UI_Manager>();
        managerPlayer = SelectPlayer.instance;
        manager = GetComponent<PlayerInputManager>();
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }
    void Update()
    {
        fighters[0] = managerPlayer.currentPrefab1;
        fighters[1] = managerPlayer.currentPrefab2;

        manager.playerPrefab = fighters[index];
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined: " + playerInput.playerIndex);

        // Set position only if it's a new player
        if (playerInput.playerIndex == 0)
        {
            playerInput.GetComponent<PlayerController>().uiManager = uiPlayer1;
            uiPlayer1.targetPlayer = playerInput.transform;
            playerInput.gameObject.transform.position = spawnPointPlayer1.transform.position;
        }
        else if (playerInput.playerIndex == 1)
        {
            playerInput.GetComponent<PlayerController>().uiManager = uiPlayer2;
            uiPlayer2.targetPlayer = playerInput.transform;
            playerInput.gameObject.transform.position = spawnPointPlayer2.transform.position;
        }
    }


    public void SwitchNextSpawnCharacter(PlayerInput input)
    {
        if (fighters[index] != null)
            index++;
    }
}
