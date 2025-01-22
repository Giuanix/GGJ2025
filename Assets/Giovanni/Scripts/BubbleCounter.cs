using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BubbleCounter : MonoBehaviour, IDamageable
{
    [Tooltip("Make sure that Prefabs/IncapsulateBubble.prefab exist! ")]

    [Header("Settings")]
    [SerializeField] private int maxDamageCounter = 100;
    [Header("HUD")]
    [SerializeField] private Text damageText;
    
    private int damageCounter = 0;
    
    
    public void TakeDamage(int amount)
    {
        damageCounter += amount;
      
        if(damageText)
            damageText.text = damageCounter.ToString();

        if(damageCounter >= maxDamageCounter)
        {
            Incapsulate();
        }
    }


    public void Incapsulate()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/IncapsulateBubble");
        prefab = Instantiate(prefab, transform.position, Quaternion.identity);
        
        transform.SetParent(prefab.transform, true);



        if (transform.TryGetComponent<PlayerController>(out PlayerController pl))
            pl.ChangeState(pl.bubbleState);
        if (transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            rb.simulated = false;
    }

    
}