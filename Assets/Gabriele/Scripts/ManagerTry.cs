using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

public class ManagerTry : MonoBehaviour
{
    #region Variables
    public static ManagerTry instance;
    public PlayerInputManager playerInputManager;
    private Dictionary<InputDevice, int> joinedDevices = new Dictionary<InputDevice, int>();
    private List<InputDevice> lockedDevices = new List<InputDevice>();

    public AudioManager managerAudio;
    [SerializeField] private GameObject[] prefabPlayers;
    [SerializeField] private List<GameObject> objectToActiveOnJoin = new List<GameObject>();
    private GameObject[] currentPrefabs = { null, null, null, null };
    private int[] selectionIndex = { -1, -1, -1, -1 };

    [SerializeField] private Sprite[] portraits;
    [SerializeField] private Image[] iconPlayers;
    [SerializeField] private Transform[] selectionFrame;
    [SerializeField] private Transform[] selectionPosition;
    [SerializeField] private bool[] selectionFlipping;

    [SerializeField] private GameObject[] previews; // GameObjects with Image and Animator
    [SerializeField] private Sprite[] previewSprites; // Static character sprites
    [SerializeField] private string[] animationsName = { "Duck", "Whale", "Rita", "Pina" };
    private Image[] previewImages;
    private Animator[] previewAnimators;

    [SerializeField] private GameObject selectionScreen;
    [SerializeField] private GameObject loadingScreen;
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
    private bool loading = false;
    public Image back;
    public Sprite backClicked;
    public Sprite backUnclicked;
    #endregion

    #region Awake
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region Start
    private void Start()
    {
        managerAudio = AudioManager.instance;
        managerAudio.PlayChoose();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        loadingScreen.SetActive(false);
        loading = false;

        fighters.Add(null);
        fighters.Add(null);
        fighters.Add(null);
        fighters.Add(null);

        foreach (Transform t in selectionFrame)
            t.gameObject.SetActive(false);

        previewImages = new Image[previews.Length];
        previewAnimators = new Animator[previews.Length];

        for (int i = 0; i < previews.Length; i++)
        {
            previewImages[i] = previews[i].GetComponent<Image>();
            previewAnimators[i] = previews[i].GetComponent<Animator>();
            previews[i].SetActive(false);
        }

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
    #endregion

    #region Update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TryJoinDevice(Keyboard.current);
            managerAudio.PlayDeviceJoin();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!loading)
            {
                back.sprite = backClicked;
                managerAudio.PlayBottonPressed();
                Invoke("SchermataPrecedente", 0.2f);
            }
        }

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.startButton.wasPressedThisFrame)
            {
                TryJoinDevice(gamepad);
                managerAudio.PlayDeviceJoin();
            }

            if (gamepad.buttonEast.wasPressedThisFrame)
            {
                if (!loading)
                {
                    back.sprite = backClicked;
                    managerAudio.PlayBottonPressed();
                    Invoke("SchermataPrecedente", 0.2f);
                }
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
    #endregion

    #region TryJoinDevice
    private void TryJoinDevice(InputDevice inputDevice)
    {
        if (joinedDevices.ContainsKey(inputDevice)) return;

        if (joinedDevices.Count < maxPlayer)
        {
            int playerIndex = joinedDevices.Count;
            joinedDevices[inputDevice] = playerIndex;

            selectionFrame[playerIndex].gameObject.SetActive(true);
            previews[playerIndex].SetActive(true);
            selectionIndex[playerIndex] = 0;
            SwitchIcon(playerIndex);

            Debug.Log($"Player {playerIndex + 1} joined with {inputDevice.displayName}");
        }
    }
    #endregion

    #region SwitchIcon
    public void SwitchIcon(int n)
    {
        if (lockedDevices.Contains(joinedDevices.Keys.ElementAt(n))) return;

        if (selectionIndex[n] == 4)
            selectionIndex[n] = 0;

        int sel = selectionIndex[n];
        iconPlayers[n].sprite = portraits[sel];
        currentPrefabs[n] = prefabPlayers[sel];

        selectionFrame[n].position = selectionPosition[sel].position;
        selectionFrame[n].transform.localScale = new Vector3((selectionFlipping[sel] ? -1 : 1), 1, 1);
        selectionFrame[n].transform.GetChild(0).localScale = new Vector3((selectionFlipping[sel] ? -1 : 1), 1, 1);

        previews[n].SetActive(true);
        previewAnimators[n].enabled = false;
        previewImages[n].sprite = previewSprites[sel];
    }
    #endregion

    #region HandlePlayerSelection
    private void HandlePlayerSelection()
    {
        foreach (var device in joinedDevices)
        {
            if (lockedDevices.Contains(device.Key)) continue;

            int playerIndex = device.Value;
            int columns = 2; // due colonne: sinistra e destra
            int rows = 2;    // due righe: sopra e sotto

            if (device.Key is Keyboard keyboard)
            {
                if (keyboard.aKey.wasPressedThisFrame) // sinistra
                {
                    int current = selectionIndex[playerIndex];
                    int row = current / columns;
                    int col = (current % columns - 1 + columns) % columns;
                    selectionIndex[playerIndex] = row * columns + col;
                    SwitchIcon(playerIndex);
                }
                else if (keyboard.dKey.wasPressedThisFrame) // destra
                {
                    int current = selectionIndex[playerIndex];
                    int row = current / columns;
                    int col = (current % columns + 1) % columns;
                    selectionIndex[playerIndex] = row * columns + col;
                    SwitchIcon(playerIndex);
                }
                else if (keyboard.wKey.wasPressedThisFrame) // su
                {
                    int current = selectionIndex[playerIndex];
                    int row = (current / columns - 1 + rows) % rows;
                    int col = current % columns;
                    selectionIndex[playerIndex] = row * columns + col;
                    SwitchIcon(playerIndex);
                }
                else if (keyboard.sKey.wasPressedThisFrame) // giù
                {
                    int current = selectionIndex[playerIndex];
                    int row = (current / columns + 1) % rows;
                    int col = current % columns;
                    selectionIndex[playerIndex] = row * columns + col;
                    SwitchIcon(playerIndex);
                }
            }
            else if (device.Key is Gamepad gamepad)
            {
                if (gamepad.dpad.left.wasPressedThisFrame || gamepad.leftStick.left.wasPressedThisFrame) // sinistra
                {
                    int current = selectionIndex[playerIndex];
                    int row = current / columns;
                    int col = (current % columns - 1 + columns) % columns;
                    selectionIndex[playerIndex] = row * columns + col;
                    SwitchIcon(playerIndex);
                }
                else if (gamepad.dpad.right.wasPressedThisFrame || gamepad.leftStick.right.wasPressedThisFrame) // destra
                {
                    int current = selectionIndex[playerIndex];
                    int row = current / columns;
                    int col = (current % columns + 1) % columns;
                    selectionIndex[playerIndex] = row * columns + col;
                    SwitchIcon(playerIndex);
                }
                else if (gamepad.dpad.up.wasPressedThisFrame || gamepad.leftStick.up.wasPressedThisFrame) // su
                {
                    int current = selectionIndex[playerIndex];
                    int row = (current / columns - 1 + rows) % rows;
                    int col = current % columns;
                    selectionIndex[playerIndex] = row * columns + col;
                    SwitchIcon(playerIndex);
                }
                else if (gamepad.dpad.down.wasPressedThisFrame || gamepad.leftStick.down.wasPressedThisFrame) // giù
                {
                    int current = selectionIndex[playerIndex];
                    int row = (current / columns + 1) % rows;
                    int col = current % columns;
                    selectionIndex[playerIndex] = row * columns + col;
                    SwitchIcon(playerIndex);
                }
            }
        }
    }

    #endregion

    #region OnPlayerJoined
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        lockedDevices.Add(playerInput.devices[0]);

        int playerIndex = joinedDevices.Keys.ToList().IndexOf(playerInput.devices[0]);
        int selectedCharacter = selectionIndex[playerIndex];
        managerAudio.PlayPlayerConfirm();

        // Abilita animazione
        previewAnimators[playerIndex].enabled = true;
        previewAnimators[playerIndex].Play(animationsName[selectedCharacter]);

        PlayerController pl = playerInput.GetComponent<PlayerController>();

        if (joinIndex == 0)
        {
            pl.uiManager = uiPlayer1;
            uiPlayer1.targetPlayer = playerInput.transform;
            playerInput.gameObject.transform.position = spawnPointPlayer1.position;
            joinIndex++;
        }
        else if (joinIndex == 1)
        {
            pl.uiManager = uiPlayer2;
            uiPlayer2.targetPlayer = playerInput.transform;
            playerInput.gameObject.transform.position = spawnPointPlayer2.position;

            if (maxPlayer == 2)
                StartCoroutine(LoadingScreen());
            else
                joinIndex++;
        }
        else if (joinIndex == 2)
        {
            pl.uiManager = uiPlayer3;
            uiPlayer3.gameObject.SetActive(true);
            uiPlayer3.targetPlayer = playerInput.transform;
            playerInput.gameObject.transform.position = spawnPointPlayer3.position;
            joinIndex++;
        }
        else if (joinIndex == 3)
        {
            pl.uiManager = uiPlayer4;
            uiPlayer4.gameObject.SetActive(true);
            uiPlayer4.targetPlayer = playerInput.transform;
            playerInput.gameObject.transform.position = spawnPointPlayer4.position;
            StartCoroutine(LoadingScreen());
        }
    }
    #endregion

    #region LoadingScreen Coroutine
    IEnumerator LoadingScreen()
    {
        loading = true;
        yield return new WaitForSeconds(2f);
        managerAudio.StopPlaySchermataSelezionePersonaggio();
        selectionScreen.SetActive(false);
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(7f);
        loadingScreen.SetActive(false);
        StartGame();
    }
    #endregion

    #region StartGame
    void StartGame()
    {
        foreach (GameObject g in objectToActiveOnJoin)
            g.gameObject.SetActive(true);

        managerAudio.PlayStage(FindObjectOfType<SelectLevel>().selectedStage);
        GameTimer.instance.StartGame();

        enabled = false;
    }
    #endregion

    #region SchermataPrecedente
    public void SchermataPrecedente()
    {
        Debug.Log("Torna al menu di selezione stage");
        back.sprite = backUnclicked;
        SceneManager.LoadScene(1);
    }
    #endregion
}
