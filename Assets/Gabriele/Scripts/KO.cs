using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KO : MonoBehaviour
{
    public static KO instance;
    bool started = false;
    void Start()
    {
        instance = this;
        gameObject.SetActive(false);

    }

    public void AnimateKO()
    {
        if(!started)
        {
            started = true;
            gameObject.SetActive(true);
            GetComponent<Image>().color = Color.clear;
            GameTimer.instance.StopTimer();
            StartCoroutine(CoroutineKO());
        }

    }

    IEnumerator CoroutineKO()
    {
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
        foreach (PlayerController pl in playerControllers)
            pl.GetComponent<PlayerInput>().enabled = false;

        yield return new WaitForSeconds(0.5f);
        
        AudioManager.instance.StopAll();
        GetComponent<Image>().color = Color.white;
        GetComponent<Animator>().Play("KOAnimation");
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.PlayKO();

        gameObject.SetActive(true);

        yield return null;
    }
}
