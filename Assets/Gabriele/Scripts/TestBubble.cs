using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBubble : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.GetComponent<BubbleCounter>().Incapsulate();
            Destroy(gameObject);
        }
    }
}
