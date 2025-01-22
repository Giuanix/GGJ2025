using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class PlayerShoot : MonoBehaviour
{
    [Header("Shared Player Shoot var")]

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private float shotDelay;
    [Header("Only Duck")]

    [SerializeField] private int shotMaxCounter = 4;
    [Space(5)]
    [Header("Reload Bar")]
    private Transform canvas;

    [SerializeField] private Vector3 offset = new Vector3(0,1,0);
    [SerializeField] private Color fillColor;
    [SerializeField] private ShotType type;
    private float shotTimeCounter;
    private Slider slider;
    
    enum ShotType
    {
        MultipleShot,
        SingleShot
    }

    private void Start()
    {
        shotTimeCounter = shotDelay;
        canvas = FindObjectOfType<Canvas>().transform;
    }

    void Update()
    {
        shotTimeCounter += Time.deltaTime;
        if(shotTimeCounter < shotDelay)
        {
            if (!slider) {
                slider = Instantiate(Resources.Load<GameObject>("Prefabs/ReloadBarPrefab"),canvas).GetComponent<Slider>();
                slider.maxValue = shotDelay;
                slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = fillColor;
            }
            slider.value = shotTimeCounter;

           
        }
        
        else if (slider && shotTimeCounter > shotDelay + 0.5f)
        {
            Destroy(slider.gameObject);
            slider = null;
        }

        if (slider)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
            slider.transform.position = screenPos;
        }
    }
    public void Shoot(InputAction.CallbackContext context)
    {

        if (context.performed && shotTimeCounter > shotDelay && Time.timeScale > 0)
        {
            shotTimeCounter = 0.0f;
            PlayerController.instance.animator.SetTrigger("Shoot");
            switch (type)
            {
                case ShotType.MultipleShot:
                    StartCoroutine("Raffica");
                    break;
                case ShotType.SingleShot:
                    Instantiate(bullet, BulletSpawnPoint.position, transform.rotation);
                    break;
            }

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
