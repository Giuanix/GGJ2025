using System.Collections.Generic;
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
    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = defaultSize;
    }
    void Start()
    {

        aspectRatio = cam.aspect;

        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
        Vector3 worldEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, Camera.main.nearClipPlane));

        defaultDistanceFromCenter = Vector3.Distance(worldCenter, worldEdge);
    }

    void Update()
    {
        distanceFromCenter = GetFartherCharacter();
        Debug.Log(distanceFromCenter);
        
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
    Vector2 oldDistance;
    private float GetFartherCharacter()
    {
        float maxDistance = 0;
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
        Vector2 thisDist = Vector2.zero;
        foreach (PlayerController player in FindObjectsOfType<PlayerController>())
        {
            float y = Mathf.Floor(player.transform.position.y / 4f) * 4f; //change 1 to use different snap!

            float xPos = Mathf.Abs(worldCenter.x + 1f * Mathf.Sign(-player.transform.position.x) - player.transform.position.x);
            float yPos = Mathf.Abs(worldCenter.y + 1.5f * Mathf.Sign(-player.transform.position.y) - y);
            Vector2 dist = new Vector2(xPos, yPos);
            float distance = Vector2.Distance(worldCenter, dist);


            
            if (distance > maxDistance)
            {
                maxDistance = distance;
                thisDist = dist;
            }
        }
        oldDistance.x = thisDist.x;
        oldDistance.y = Mathf.Lerp(oldDistance.y, thisDist.y, Time.deltaTime*3f);

        return Vector2.Distance(worldCenter, oldDistance);
    }
}
