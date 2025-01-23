using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FloatingObject))]
public class PowerUpDelay : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float delayReducer;

    private void Start()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerShoot>(out PlayerShoot pl))
        {
            pl.StarPowerup(duration, delayReducer,GetComponent<SpriteRenderer>().sprite);
            Destroy(gameObject);
        }
    }
}
