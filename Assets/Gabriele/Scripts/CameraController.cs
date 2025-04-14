using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private float distanceFromCenter;
    private float defaultDistanceFromCenter;
    private Camera cam;
    [SerializeField] private float defaultSize = 5f;
    [SerializeField] private float maxSize = 10f;
    float aspectRatio;
    void Start()
    {
        cam = GetComponent<Camera>();


        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
        Vector3 worldEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, Camera.main.nearClipPlane));

        defaultDistanceFromCenter = Vector3.Distance(worldCenter, worldEdge);
        cam.orthographicSize = defaultSize;
        aspectRatio = cam.aspect;
    }

    void Update()
    {
        distanceFromCenter = GetFartherCharacter();
        if (distanceFromCenter > defaultDistanceFromCenter)
        {
            cam.orthographicSize = distanceFromCenter / aspectRatio;

        }
        else
        {
            cam.orthographicSize = defaultSize;
        }

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, defaultSize, maxSize);
    }

    private float GetFartherCharacter()
    {
        float maxDistance = 0;
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));

        foreach (PlayerController player in FindObjectsOfType<PlayerController>())
        {
            float dir = player.transform.position.x > 0 ? -1 : 1;
            float currDist = Mathf.Abs(worldCenter.x+3f*dir - player.transform.position.x);
            if (currDist > maxDistance)
            {
                maxDistance = currDist;
            }
        }

        return maxDistance;
    }
}
