using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class ResourceBox : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 10;
    [SerializeField] private GameObject spawnItem;
    [SerializeField] private Sprite[] randomSprite;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = randomSprite[Random.Range(0, randomSprite.Length)];
    }


    public void TakeDamage(int amount,bool damageFromProjectile)
    {
        if (damageFromProjectile)
        {
            health -= amount;
            if (health <= 0)
            {
                Instantiate(spawnItem, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
