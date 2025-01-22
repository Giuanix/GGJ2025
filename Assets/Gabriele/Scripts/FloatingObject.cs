using UnityEngine;
[RequireComponent (typeof(Rigidbody2D))]
public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float buoyancyForce = 10f;  // Force pushing the object upwards
    [SerializeField] private float dragCoefficient = 0.5f;  // Drag force for horizontal motion
    [SerializeField] private float angularDragCoefficient = 0.1f;  // Drag force for rotation

    private Rigidbody2D rb;  // Rigidbody2D for applying physics

    private bool isInWater;
    public bool IsInWater() { return isInWater; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get the current water height at the object's X position
        float waterHeight = WaterLevel.Instance.GetHeightAtPosition(transform.position.x);

        // Calculate the current distance from the water level
        float currentDistanceFromWater = transform.position.y - waterHeight;

        // Apply the buoyancy force only if the object is below the water surface
        float buoyancy = 0f;
        if (currentDistanceFromWater < 0) // Object is below water surface
        {
            buoyancy = buoyancyForce * Mathf.Abs(currentDistanceFromWater);  // Increase buoyancy with depth

            // Apply the buoyancy force
            rb.AddForce(Vector2.up * buoyancy, ForceMode2D.Force);

            // Apply drag force for horizontal motion
            Vector2 dragForce = -rb.velocity * dragCoefficient;
            rb.AddForce(dragForce, ForceMode2D.Force);

            // Apply angular drag
            rb.angularDrag = angularDragCoefficient;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Water") isInWater = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Water") isInWater = false;

    }

}
