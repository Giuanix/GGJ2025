using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FloatingObject))]
[RequireComponent(typeof(SpriteRenderer))]
public class PowerUpDamage : MonoBehaviour
{
    [SerializeField] private int damage = 15;
    [SerializeField] private float time = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerShoot>(out PlayerShoot pl))
        {
            pl.SetDamage(time, damage, GetComponent<SpriteRenderer>().sprite);
            Destroy(gameObject);
        }
    }
}
