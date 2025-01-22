using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DrawBubble : MonoBehaviour
{
    [Header("Bubble")]
    [SerializeField] private float horizontalSpeed = 1f;
    [SerializeField] private Vector2 horizontalAmplitude = new Vector2(0.5f, 1.5f);
    [SerializeField] private Vector2 floatingSpeed = new Vector2(1, 2);
    [SerializeField] private Vector2 increasingScaleSpeed = new Vector2(1, 2);

    [SerializeField] private float movementVariance = 0.5f; 
    [SerializeField] private bool scaleIncreasing = false;
    [SerializeField] private float livingTime = 10f;
    [SerializeField] private float generalSpeed = 0.65f;
    private float startHorizontalOffset;
    private void Start()
    {
        startHorizontalOffset = Random.Range(-50, 50);
    }

    private void Update()
    {
       

        if (scaleIncreasing)
            transform.localScale += Vector3.one * Time.deltaTime * Random.Range(increasingScaleSpeed.x, increasingScaleSpeed.y);

        float randomHorizontalMovement = Mathf.Sin(Time.time * horizontalSpeed + startHorizontalOffset) 
            * Random.Range(horizontalAmplitude.x, horizontalAmplitude.y);
     
        randomHorizontalMovement += Random.Range(-movementVariance, movementVariance);  // Add randomness

        float randomVerticalMovement = Random.Range(floatingSpeed.x, floatingSpeed.y)
            + Random.Range(-movementVariance, movementVariance); // Randomize floating speed


        transform.position += new Vector3(randomHorizontalMovement, randomVerticalMovement, 0) * Time.deltaTime * generalSpeed;



        if(livingTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
