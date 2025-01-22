using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WhaleShoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform BulletSpawnPoint;
    [SerializeField] float shotDelay;
    float shotTimeCounter;
    private void Start()
    {
        shotTimeCounter = shotDelay;
    }
    void Update()
    {
        shotTimeCounter += Time.deltaTime;
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if(context.performed && shotTimeCounter > shotDelay && Time.timeScale > 0)
        {
            Debug.Log("Sparo");
            Instantiate(bullet, BulletSpawnPoint.position, transform.rotation);
            shotTimeCounter = 0.0f;
        }
    }
}
