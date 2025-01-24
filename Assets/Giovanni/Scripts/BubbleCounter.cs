using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BubbleCounter : MonoBehaviour, IDamageable
{
    public static BubbleCounter instance;
    [Tooltip("Make sure that Prefabs/IncapsulateBubble.prefab exist! ")]

    [Header("Settings")]
    [SerializeField] private int maxDamageCounter = 100;
    public int MaxDamageCounter { get => maxDamageCounter;}
    private int defense = 0;
    PlayerController pl;
    private int damageCounter = 0;
    [HideInInspector] public AudioManager managerAudio;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        managerAudio = AudioManager.instance;
        pl = GetComponent<PlayerController>();
    }

    public void TakeDamage(int amount,bool damageFromProjectile)
    {
        managerAudio.PlayDannoSubito();
        damageCounter += amount - defense;

        UpdateText();

        if (damageCounter >= maxDamageCounter && !pl.IsInState<BubbleState>())
        {
            managerAudio.PlayFluttuamentoBolla();
            Incapsulate();
        }
    }

    public void UpdateText()
    {
        damageCounter = Mathf.Clamp(damageCounter,0,maxDamageCounter+5);

        if (pl.uiManager)
        {
            pl.uiManager.amount = damageCounter;
            pl.uiManager.UpdateText(maxDamageCounter);
        }
    }
    public void Incapsulate()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/IncapsulateBubble");
        prefab = Instantiate(prefab, transform.position, Quaternion.identity);
        
        transform.SetParent(prefab.transform, true);
        pl.ChangeState(pl.bubbleState);
    }

    public void Heal(int amount)
    {
        damageCounter -= amount;
        UpdateText();

    }

    public void SetDefense( float duration, int defense,Sprite icon)
    {
        this.defense = defense;
        pl.uiManager.SetupPowerUpImage(icon);
        StartCoroutine(DefensePowerUp(duration));

    }

    IEnumerator DefensePowerUp(float duration)
    {
        float elapsedTime = 0f;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer)
        {
            while (elapsedTime < duration)
            {
                spriteRenderer.color = Color.gray;
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            spriteRenderer.color = Color.white;
            pl.uiManager.SetupPowerUpImage(null);

            defense = 0;
        }

    }





    public void SetPercentage(int amount)
    {
        damageCounter -= amount;
        UpdateText();
    }
}