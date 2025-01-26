using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class KO : MonoBehaviour
{
    public static KO instance;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject draw;
    [SerializeField] private Image p1;

    bool started = false;
    void Start()
    {
        draw.SetActive(false);

        instance = this;
        gameObject.SetActive(false);
        overlay.SetActive(false);
    }
    int deathCounter = 0;
    public void AnimateKO(bool force = false)
    {

        deathCounter++;
        if (deathCounter == ManagerTry.instance.maxPlayer-1 || force)
        {
            if (!started)
            {
                started = true;
                gameObject.SetActive(true);
                GetComponent<Image>().color = Color.clear;
                GameTimer.instance.StopTimer();
                StartCoroutine(CoroutineKO(force));
            }
        }
    }

    IEnumerator CoroutineKO(bool force)
    {
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
        foreach (PlayerController pl in playerControllers){
            if(force)
                Destroy(p1.gameObject);
   
        }
        if(force) GameTimer.instance.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(0.5f);
        
        AudioManager.instance.StopAll();

        GetComponent<Image>().color = Color.white;
        GetComponent<Animator>().Play("KOAnimation");
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.PlayKO();
        gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlayTheWinnerIs();
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlaySchermataVittoria();
        GetComponent<Image>().color = Color.clear;
        if(force){
            draw.SetActive(true);
        }else{
            GameTimer.instance.gameObject.SetActive(false);
            overlay.SetActive(true);

            p1.sprite = playerControllers[0].GetComponent<SpriteRenderer>().sprite;
        }
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene(0);
        yield return null;
    }
}
