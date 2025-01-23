using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBox : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 10;
    [SerializeField] private GameObject spawnItem;
    
    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Instantiate(spawnItem, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
