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

    public DamageText damageText;

    PlayerController pl;

    private int damageCounter = 0;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        pl = GetComponent<PlayerController>();
    }

    public void TakeDamage(int amount,bool damageFromProjectile)
    {
        damageCounter += amount;

        UpdateText();

        if (damageCounter >= maxDamageCounter && !pl.IsInState<BubbleState>())
        {
            Incapsulate();
        }
    }

    public void UpdateText()
    {
        if (damageText)
        {
            damageText.amount = damageCounter;
            damageText.UpdateText(maxDamageCounter);
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

    public void SetPercentage(int amount)
    {
        damageCounter -= amount;
        UpdateText();
    }
}