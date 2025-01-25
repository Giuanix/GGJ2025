using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private Text timerText;
    [SerializeField] private Text timerText2;
    [SerializeField] private Text countdownText;
    [SerializeField] private Text countdownText2;

    public static GameTimer instance;

    bool canCountdown = false;

    private void Start()
    {
        instance = this;    
        gameObject.SetActive(false);

        countdownText.text = "";
        countdownText2.text = "";
        timerText.text = "0:00";
        timerText2.text = "0:00";

    }

    private void Update()
    {
        if (canCountdown) {
            countdown -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(countdown / 60);
            int seconds = Mathf.FloorToInt(countdown % 60);

            string formattedTime = $"{minutes:D2}:{seconds:D2}";

            timerText.text = formattedTime;
            timerText2.text = formattedTime;

            if (countdown <= 0)
            {
                KO.instance.AnimateKO(true);
            }
        }
    }

    public void StartGame()
    {
        gameObject.SetActive(true);
        StartCoroutine(CountdownToPlay());
    }
    public void StopTimer()
    {
        canCountdown = false;
    }
    IEnumerator CountdownToPlay()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlayCountdown();
        countdownText.text = "3";
        countdownText2.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        countdownText2.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        countdownText2.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "FIGHT!";
        countdownText2.text = "FIGHT!";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";
        countdownText2.text = "";

        canCountdown = true;

        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();

        // Activate all PlayerControllers
        foreach (var controller in playerControllers)
        {
            controller.ChangeState(controller.locomotionState); // Set each player controller's GameObject as active
        }

        countdownText.gameObject.SetActive(false);
    }
}
