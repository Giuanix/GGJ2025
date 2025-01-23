using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealingObject : MonoBehaviour
{
    [SerializeField] private int healAmount = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<BubbleCounter>(out BubbleCounter pl))
        {
            pl.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
