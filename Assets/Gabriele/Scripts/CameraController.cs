using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private float defaultSize = 5f;
    [SerializeField] private float maxSize = 10f;

    private float aspectRatio;

    private float minX, maxX, minY, maxY;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = defaultSize;
    }

    private void Start()
    {
        aspectRatio = cam.aspect;

        // Temporarily set to max to calculate world bounds
        cam.orthographicSize = maxSize;

        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;

        cam.orthographicSize = defaultSize;
    }

    private void Update()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        if (players.Length == 0) return;

        Bounds bounds = GetPlayersBounds(players);
        Vector3 center = bounds.center;

        float requiredHeight = bounds.size.y / 2f;
        float requiredWidth = bounds.size.x / (2f * aspectRatio);

        float targetSize = Mathf.Max(requiredHeight, requiredWidth);

        cam.orthographicSize = Mathf.Clamp(targetSize, defaultSize, maxSize);

        ClampCameraPosition(center);
    }

    private Bounds GetPlayersBounds(PlayerController[] players)
    {
        Vector3 min = players[0].transform.position;
        Vector3 max = players[0].transform.position;

        foreach (var player in players)
        {
            Vector3 pos = player.transform.position;
            min = Vector3.Min(min, pos);
            max = Vector3.Max(max, pos);
        }

        Bounds bounds = new Bounds();

        //Evitare il resize quando un giocatore è oltro il bordo, così viene contata la posizione come se fosse SUL bordo.
        if (min.x < minX) min.x = minX;
        if (min.y < minY) min.y = minY;

        if (max.x > maxX) max.x = maxX;
        if (max.y > maxY) max.y = maxY;

        bounds.SetMinMax(min, max);
        return bounds;
    }

    private void ClampCameraPosition(Vector3 targetCenter)
    {
        float camSize = cam.orthographicSize;
        float camHeight = camSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float halfWidth = camWidth / 2f;
        float halfHeight = camHeight / 2f;

        float clampedX = Mathf.Clamp(targetCenter.x, minX + halfWidth, maxX - halfWidth);
        float clampedY = Mathf.Clamp(targetCenter.y, minY + halfHeight, maxY - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
