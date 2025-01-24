using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("SFX")]
    [SerializeField] private AudioSource SparoBalena;
    [SerializeField] private AudioSource SparoPapera;
    [SerializeField] private AudioSource DannoSubito;
    [SerializeField] private AudioSource FluttuamentoBolla;
    [SerializeField] private AudioSource PlayerSconfitto;

    [Header("OST")]
    [SerializeField] private AudioSource Stage1;
    [SerializeField] private AudioSource SchermataSelezionePersonaggio;
    void Awake()
    {
        instance = this;
    }

    //SFX
    public void PlaySparoBalena()
    {
        SparoBalena.PlayOneShot(SparoBalena.clip,0.5f);
    }  
    public void PlaySparoPapera()
    {
        SparoPapera.PlayOneShot(SparoPapera.clip,0.5f);
    }  
    public void PlayDannoSubito()
    {
        DannoSubito.PlayOneShot(DannoSubito.clip,0.5f);
    }  
    public void PlayFluttuamentoBolla()
    {
        FluttuamentoBolla.PlayOneShot(FluttuamentoBolla.clip,0.5f);
    }  
    public void PlayPlayerSconfitto()
    {
        PlayerSconfitto.PlayOneShot(PlayerSconfitto.clip,1f);
    }  
    
    //OST
    public void PlayStage1()
    {
        Stage1.PlayOneShot(Stage1.clip,0.5f);
    }  
    public void StopPlayStage1()
    {
        Stage1.Stop();
    }  
    public void PlaySchermataSelezionePersonaggio()
    {
        SchermataSelezionePersonaggio.PlayOneShot(SchermataSelezionePersonaggio.clip,0.5f);
    }
    public void StopPlaySchermataSelezionePersonaggio()
    {
        SchermataSelezionePersonaggio.Stop();
    }
    

}
