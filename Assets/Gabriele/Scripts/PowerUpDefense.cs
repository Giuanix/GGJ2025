using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FloatingObject))]
[RequireComponent(typeof(SpriteRenderer))]
public class PowerUpDefense : MonoBehaviour
{
    [SerializeField] private int defenseAmount = 10;
    [SerializeField] private float time = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<BubbleCounter>(out BubbleCounter pl))
        {
            pl.SetDefense(time,defenseAmount,GetComponent<SpriteRenderer>().sprite);
            Destroy(gameObject);
        }
    }
}
