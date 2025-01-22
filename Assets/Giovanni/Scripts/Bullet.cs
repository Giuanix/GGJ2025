using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public PlayerController manager;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float TimerDespawn = 10f;
    private Rigidbody2D rb;
    void Start()
    {
        manager = PlayerController.instance;
        rb = GetComponent<Rigidbody2D>();
        
        if(manager.isFacingRight == true)
        {
            rb.velocity = transform.right * bulletSpeed;
        }
        else
        {
            rb.velocity = -transform.right * bulletSpeed;
        }
    }

    void Update()
    {
        TimerDespawn -= Time.deltaTime;
        
        if(TimerDespawn <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
