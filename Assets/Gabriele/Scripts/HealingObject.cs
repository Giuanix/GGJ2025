using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FloatingObject))]
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
