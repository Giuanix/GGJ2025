using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DuckShoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform BulletSpawnPoint;
    [SerializeField] float shotDelay;
    [SerializeField] int shotMaxCounter = 4;
    private float shotTimeCounter;

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

        if (context.performed && shotTimeCounter > shotDelay && Time.timeScale > 0)
        {
            shotTimeCounter = 0.0f;
            StartCoroutine("Raffica");
        }
    }
    IEnumerator Raffica()
    {
        for(int i = 0; i < shotMaxCounter; i++)
        {
            Instantiate(bullet, BulletSpawnPoint.position, transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }

    }
}
