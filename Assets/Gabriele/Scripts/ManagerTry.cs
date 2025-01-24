using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class ManagerTry : MonoBehaviour
{
    public PlayerInputManager playerInputManager;

    private List<InputDevice> joinedDevices = new List<InputDevice>();


    [SerializeField] private GameObject[] prefabPlayers;
    private GameObject[] currentPrefabs = {null,null};

    private int[] selectionIndex = { -1, -1 };
    
    [SerializeField] private Sprite[] portraits;
    [SerializeField] private Image[] iconPlayers;
    [SerializeField] private Transform[] selectionFrame;
    [SerializeField] private Transform[] selectionPosition;
    [SerializeField] private bool[] selectionFlipping;
    [SerializeField] private Animator[] previews;
    [SerializeField] private string[] animationsName = { "Duck", "Whale"};

    [SerializeField] private GameObject selectionScreen;

    [Space(5)]
    public UI_Manager uiPlayer1;
    public UI_Manager uiPlayer2;
    [Space(5)]
    public Transform spawnPointPlayer1;
    public Transform spawnPointPlayer2;
    [Space(5)]
    [Tooltip("Debug")]
    [SerializeField] List<GameObject> fighters = new List<GameObject>();

    int joinIndex = 0;

    private void Start()
    {
        uiPlayer1 = GameObject.FindGameObjectWithTag("UiPlayer1").GetComponent<UI_Manager>();
        uiPlayer2 = GameObject.FindGameObjectWithTag("UiPlayer2").GetComponent<UI_Manager>();
      
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;

        fighters.Add(null);
        fighters.Add(null);

        foreach (Transform t in selectionFrame)
            t.gameObject.SetActive(false);

        foreach (Animator a in previews)
            a.gameObject.SetActive(false);
    }

    private void Update()
    {
     
        if (Keyboard.current.digit1Key.wasPressedThisFrame) // Example: Spacebar to join a player
        {
            TryJoinDevice(Keyboard.current);
        }

      
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame) // Gamepad 'A' button to join a player
            {
                TryJoinDevice(gamepad);
            }
        }


        if (joinedDevices.Count > 0)
        {
            HandlePlayerSelection();
        }

        fighters[0] = currentPrefabs[0];
        fighters[1] = currentPrefabs[1];

        playerInputManager.playerPrefab = fighters[joinIndex];
    }


    private void TryJoinDevice(InputDevice inputDevice)
    {
       
        if (joinedDevices.Contains(inputDevice))
        {
            Debug.Log("This input device has already been joined!");
            return;
        }

        // Join the player with the input device
        int playerIndex = joinedDevices.Count;
        if (playerIndex < 2) // Limit to 4 players (adjust based on your needs)
        {
            joinedDevices.Add(inputDevice);

            selectionFrame[playerIndex].gameObject.SetActive(true);
            previews[playerIndex].gameObject.SetActive(true);

            Debug.Log($"Player {playerIndex + 1} joined with {inputDevice.displayName}");
        }
        else
        {
            Debug.Log("Maximum number of players already joined!");
        }
    }
     
    private void SwitchIcon(int n)
    {
        selectionIndex[n]++;


        if (selectionIndex[n] == prefabPlayers.Length)
            selectionIndex[n] = 0;

        int sel = selectionIndex[n];

        iconPlayers[n].sprite = portraits[sel];
        currentPrefabs[n] = prefabPlayers[sel];

        selectionFrame[n].position = selectionPosition[sel].position;

        selectionFrame[n].transform.localScale = new Vector3( (selectionFlipping[sel] ? -1 : 1),1,1);
        selectionFrame[n].transform.GetChild(0).localScale = new Vector3((selectionFlipping[sel] ? -1 : 1),1,1);

        previews[n].Play(animationsName[sel]);

    }


    private void HandlePlayerSelection()
    {
        for (int i = 0; i < joinedDevices.Count; i++)
        {
            if (joinedDevices[i] is Keyboard)
            {
                var keyboard = (Keyboard)joinedDevices[i];

                if (keyboard.digit1Key.wasPressedThisFrame)
                {
                    SwitchIcon(i);
                }
            }
            else if (joinedDevices[i] is Gamepad)
            {
                var gamepad = (Gamepad)joinedDevices[i];

                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    SwitchIcon(i);
                }
            }
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined: " + playerInput.playerIndex);

        // Set position only if it's a new player
        if (joinIndex == 0)
        {
            playerInput.GetComponent<PlayerController>().uiManager = uiPlayer1;
            uiPlayer1.targetPlayer = playerInput.transform;
            playerInput.gameObject.transform.position = spawnPointPlayer1.transform.position;

            joinIndex += 1;

        }
        else if (joinIndex == 1)
        {
            playerInput.GetComponent<PlayerController>().uiManager = uiPlayer2;
            uiPlayer2.targetPlayer = playerInput.transform;
            playerInput.gameObject.transform.position = spawnPointPlayer2.transform.position;

        }
        else
        {
            selectionScreen.SetActive(false);
            enabled = false;
        }
    }
}
