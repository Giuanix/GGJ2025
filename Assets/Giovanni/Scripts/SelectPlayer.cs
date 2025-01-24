using System.Collections;
using System.Collections.Generic;
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

    private PlayerInput player1Input;
    private PlayerInput player2Input;

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
        if (selectionIndex[0] == -1 && player1Input != null && player1Input.actions["Select"].triggered)
        {
            selectionIndex[0]++;
            if (selectionIndex[0] == prefabPlayer.Length)
                selectionIndex[0] = 0;

            iconPlayer1.sprite = portraits[selectionIndex[0]];
            currentPrefab1 = prefabPlayer[selectionIndex[0]];

            // Dopo che il primo giocatore ha selezionato, abilitare l'input per il secondo giocatore
            EnablePlayerInput(1);
        }

        // Selezione del secondo giocatore, attivata solo dopo che il primo ha selezionato
        if (selectionIndex[1] == -1 && player2Input != null && player2Input.actions["Select"].triggered)
        {
            selectionIndex[1]++;
            if (selectionIndex[1] == prefabPlayer.Length)
                selectionIndex[1] = 0;

            iconPlayer2.sprite = portraits[selectionIndex[1]];
            currentPrefab2 = prefabPlayer[selectionIndex[1]];
        }
    }

    public void SetPlayerInputs(PlayerInput p1Input, PlayerInput p2Input)
    {
        player1Input = p1Input;
        player2Input = p2Input;

        // Bloccare inizialmente l'input del secondo giocatore
        DisablePlayerInput(1);
    }

    private void EnablePlayerInput(int playerIndex)
    {
        if (playerIndex == 1 && player2Input != null)
            player2Input.enabled = true;
    }

    private void DisablePlayerInput(int playerIndex)
    {
        if (playerIndex == 1 && player2Input != null)
            player2Input.enabled = false;
    }
}