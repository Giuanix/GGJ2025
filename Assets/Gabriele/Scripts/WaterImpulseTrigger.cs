using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterImpulseTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.TryGetComponent<FloatingObject>(out FloatingObject fl))
        {
            if (!fl.IsInWater() && WaterLevel.Instance.CanSplash())
            {
                Debug.Log("Splash");
                transform.parent.GetComponent<WaterLevel>().StartImpulse(15f, 0.25f, this);
            }
        }
       
    }

}
