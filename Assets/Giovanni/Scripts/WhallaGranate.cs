using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WhallaGranate : MonoBehaviour
{
    [Header("Whalla Granate Properties")]
    public int maxBounces = 2;
    private int groundHitCount = 0;
    public GameObject bubbleExplosion;
    public AudioManager audioManager;
    void Start()
    {
        audioManager = AudioManager.instance;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundHitCount++;

            if (groundHitCount >= maxBounces)
            {
                BubbleExplosion();
            }
        }
    }

    void BubbleExplosion()
    {
        if (bubbleExplosion != null)
        {
            audioManager.PlaySparoBalena();
            Instantiate(bubbleExplosion, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
