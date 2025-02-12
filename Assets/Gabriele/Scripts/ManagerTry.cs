using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;

public class ManagerTry : MonoBehaviour
{
    public static ManagerTry instance;
    public PlayerInputManager playerInputManager;
    private Dictionary<InputDevice, int> joinedDevices = new Dictionary<InputDevice, int>();
    private List<InputDevice> lockedDevices = new List<InputDevice>();

    public AudioManager managerAudio;
    [SerializeField] private GameObject[] prefabPlayers;
    [SerializeField] private List<GameObject> objectToActiveOnJoin = new List<GameObject>();
    private GameObject[] currentPrefabs = { null, null,null,null };

    private int[] selectionIndex = { -1, -1,-1,-1 };

    [SerializeField] private Sprite[] portraits;
    [SerializeField] private Image[] iconPlayers;
    [SerializeField] private Transform[] selectionFrame;
    [SerializeField] private Transform[] selectionPosition;
    [SerializeField] private bool[] selectionFlipping;
    [SerializeField] private Animator[] previews;
    [SerializeField] private string[] animationsName = { "Duck", "Whale" };

    [SerializeField] private GameObject selectionScreen;
    public int maxPlayer = 2;
    [Space(5)]
    public UI_Manager uiPlayer1;
    public UI_Manager uiPlayer2;
    public UI_Manager uiPlayer3;
    public UI_Manager uiPlayer4;
    [Space(5)]
    public Transform spawnPointPlayer1;
    public Transform spawnPointPlayer2;
    public Transform spawnPointPlayer3;
    public Transform spawnPointPlayer4;
    [Space(5)]
    [SerializeField] private List<GameObject> fighters = new List<GameObject>();

    int joinIndex = 0;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        managerAudio = AudioManager.instance;

        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;

        fighters.Add(null);
        fighters.Add(null);
        fighters.Add(null);
        fighters.Add(null);

        foreach (Transform t in selectionFrame)
            t.gameObject.SetActive(false);

        foreach (Animator a in previews)
            a.gameObject.SetActive(false);


        foreach (GameObject g in objectToActiveOnJoin)
            g.gameObject.SetActive(false);


        
        selectionScreen.SetActive(true);

        uiPlayer1 = GameObject.FindGameObjectWithTag("UiPlayer1").GetComponent<UI_Manager>();
        uiPlayer2 = GameObject.FindGameObjectWithTag("UiPlayer2").GetComponent<UI_Manager>();
        uiPlayer3 = GameObject.FindGameObjectWithTag("UiPlayer3").GetComponent<UI_Manager>();
        uiPlayer4 = GameObject.FindGameObjectWithTag("UiPlayer4").GetComponent<UI_Manager>();
        uiPlayer3.gameObject.SetActive(false);
        uiPlayer4.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            TryJoinDevice(Keyboard.current);
        }

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.startButton.wasPressedThisFrame)
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
        fighters[2] = currentPrefabs[2];
        fighters[3] = currentPrefabs[3];

        playerInputManager.playerPrefab = fighters[joinIndex];

    }

private void SwitchIcon(int n)
{
    if (lockedDevices.Contains(joinedDevices.Keys.ElementAt(n))) return; // Prevent switching after locking

    // Correct the selectionIndex overflow
    selectionIndex[n] = Mathf.Clamp(selectionIndex[n], 0, portraits.Length - 1);

    int sel = selectionIndex[n];
    iconPlayers[n].sprite = portraits[sel];
    currentPrefabs[n] = prefabPlayers[sel];

    selectionFrame[n].position = selectionPosition[sel].position;
    selectionFrame[n].transform.localScale = new Vector3((selectionFlipping[sel] ? -1 : 1), 1, 1);
    selectionFrame[n].transform.GetChild(0).localScale = new Vector3((selectionFlipping[sel] ? -1 : 1), 1, 1);

    previews[n].Play(animationsName[sel]);
}

private void HandlePlayerSelection()
{
    foreach (var device in joinedDevices)
    {
        if (lockedDevices.Contains(device.Key)) continue;

        if (device.Key is Keyboard keyboard)
        {
            if (keyboard.aKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame)
            {
                selectionIndex[device.Value] = Mathf.Clamp(selectionIndex[device.Value] + (keyboard.dKey.wasPressedThisFrame ? 1 : -1), 0, portraits.Length - 1);
                SwitchIcon(device.Value);
            }
        }
        else if (device.Key is Gamepad gamepad)
        {
            if (gamepad.dpad.right.wasPressedThisFrame || gamepad.dpad.left.wasPressedThisFrame)
            {
                selectionIndex[device.Value] = Mathf.Clamp(selectionIndex[device.Value] + (gamepad.dpad.right.wasPressedThisFrame ? 1 : -1), 0, portraits.Length - 1);
                SwitchIcon(device.Value);
            }
        }
    }
}

private void TryJoinDevice(InputDevice inputDevice)
{
    if (joinedDevices.ContainsKey(inputDevice))
    {
        Debug.Log("This input device has already been joined!");
        return;
    }

    if (joinedDevices.Count < maxPlayer)
    {
        int playerIndex = joinedDevices.Count;
        joinedDevices[inputDevice] = playerIndex;

        selectionFrame[playerIndex].gameObject.SetActive(true);
        previews[playerIndex].gameObject.SetActive(true);
        selectionIndex[playerIndex] = 0;
        SwitchIcon(playerIndex);

        Debug.Log($"Player {playerIndex + 1} joined with {inputDevice.displayName}");
    }
    else
    {
        Debug.Log("Maximum number of players already joined!");
    }
}

public void OnPlayerJoined(PlayerInput playerInput)
{

    /*foreach(PlayerInput pl in lockedDevices){

    }

    if(!lockedDevices.Contains(playerInput.devices[0]))*/
        lockedDevices.Add(playerInput.devices[0]); // Lock player input after selection

    PlayerController pl = playerInput.GetComponent<PlayerController>();

    if (joinIndex == 0)
    {
        pl.uiManager = uiPlayer1;
        uiPlayer1.targetPlayer = playerInput.transform;
        playerInput.gameObject.transform.position = spawnPointPlayer1.transform.position;
        joinIndex++;
    }
    else if (joinIndex == 1)
    {
        pl.uiManager = uiPlayer2;
        uiPlayer2.targetPlayer = playerInput.transform;
        playerInput.gameObject.transform.position = spawnPointPlayer2.transform.position;

        if (maxPlayer == 2)
            StartGame();
        else
            joinIndex++;
    }
    else if (joinIndex == 2)
    {
        pl.uiManager = uiPlayer3;
        uiPlayer3.gameObject.SetActive(true);
        uiPlayer3.targetPlayer = playerInput.transform;
        playerInput.gameObject.transform.position = spawnPointPlayer3.transform.position;
        joinIndex++;
    }
    else if (joinIndex == 3)
    {
        pl.uiManager = uiPlayer4;
        uiPlayer4.gameObject.SetActive(true);

        uiPlayer4.targetPlayer = playerInput.transform;
        playerInput.gameObject.transform.position = spawnPointPlayer4.transform.position;
        joinIndex++;
        StartGame();
    }
}

    private void StartGame()
    {
        foreach (GameObject g in objectToActiveOnJoin)
            g.gameObject.SetActive(true);


        managerAudio.PlayStage(FindObjectOfType<SelectLevel>().selectedStage);
        managerAudio.StopPlaySchermataSelezionePersonaggio();

        selectionScreen.SetActive(false);

        GameTimer.instance.StartGame();
        GetComponent<PlayerInputManager>().enabled = false;
        enabled = false;
    }


}













