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
    [SerializeField] private AudioSource PlayerJump;
    [SerializeField] private AudioSource ExplosionBubble;
    [SerializeField] private AudioSource TryToBeFree;

    [Header("OST")]
    [SerializeField] private AudioSource[] stages;
    [SerializeField] private AudioSource SchermataSelezionePersonaggio;

    [Header("Voiceline")]
    [SerializeField] private AudioSource KO;
    [SerializeField] private AudioSource Countdown;
    [SerializeField] private AudioSource ChooseYourFighter;

    void Awake()
    {
        instance = this;
    }


    public void StopAll()
    {
        foreach(AudioSource a in  stages)
            a.Stop();
        SchermataSelezionePersonaggio.Stop();
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
    public void PlayJump()
    {
        PlayerSconfitto.PlayOneShot(PlayerJump.clip, 0.5f);
    }
    public void PlayExplosionBubble()
    {
        PlayerSconfitto.PlayOneShot(PlayerJump.clip, 0.5f);
    }
    public void PlayTryToBeFree()
    {
        PlayerSconfitto.PlayOneShot(PlayerJump.clip, 0.5f);
    }
    //OST
    
    public void PlaySchermataSelezionePersonaggio()
    {
        SchermataSelezionePersonaggio.PlayOneShot(SchermataSelezionePersonaggio.clip,0.5f);
    }
    public void StopPlaySchermataSelezionePersonaggio()
    {
        SchermataSelezionePersonaggio.Stop();
    }

    public void PlayStage(int index)
    {
        stages[index-1].PlayOneShot(stages[index-1].clip, 0.5f);
    }

    //VOICELINE

    public void PlayKO()
    {
        KO.PlayOneShot(KO.clip, 1f);
    }
    public void PlayCountdown()
    {
        Countdown.PlayOneShot(Countdown.clip, 1f);
    }
    public void PlayChoose()
    {
        ChooseYourFighter.PlayOneShot(ChooseYourFighter.clip, 1f);
    }
}
