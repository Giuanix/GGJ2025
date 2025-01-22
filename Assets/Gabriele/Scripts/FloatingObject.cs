using UnityEngine;
public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float buoyancyForce = 10f;  // Force pushing the object upwards
    [SerializeField] private float dragCoefficient = 0.5f;  // Drag force for horizontal motion
    [SerializeField] private float angularDragCoefficient = 0.1f;  // Drag force for rotation

    [SerializeField] private Rigidbody2D rb; 

    private bool isInWater;
    public bool IsInWater() { return isInWater; }

    private void FixedUpdate()
    {
        float waterHeight = WaterLevel.Instance.GetHeightAtPosition(transform.position.x);

    
        float currentDistanceFromWater = transform.position.y - waterHeight;

       
        float buoyancy = 0f;
        if (currentDistanceFromWater < 0) 
        {
            buoyancy = buoyancyForce * Mathf.Abs(currentDistanceFromWater);  // Increase buoyancy with depth

            // Apply the buoyancy force
            rb.AddForce(Vector2.up * buoyancy, ForceMode2D.Force);

            Vector2 dragForce = -rb.velocity * dragCoefficient;
            rb.AddForce(dragForce, ForceMode2D.Force);

            rb.angularDrag = angularDragCoefficient;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<WaterLevel>(out WaterLevel w)) isInWater = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<WaterLevel>(out WaterLevel w)) isInWater = false;

    }

}
