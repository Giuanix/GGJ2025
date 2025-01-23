using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PolygonCollider2D))]
public class WaterLevel : MonoBehaviour
{
    [SerializeField] private int textureWidth = 320;
    [SerializeField] private int textureHeight = 180;
    public float offset = 200f;
    
    
    [Range(2, 64)]
    [SerializeField] private int waterPointsCount = 6;
    [SerializeField] private int curveThickness = 3;
    public float speed = 3f;
    public float waterFrequency = 15f;
    public float waterHeight = 5;
    public float angularForce = 20; //<-- used in floatingObj

    [SerializeField] private Color curveColor;
    [SerializeField] private Color fillColor;
    [SerializeField] private GameObject waterHitPoint;


    private List<WaterImpulseTrigger> waterPoints = new List<WaterImpulseTrigger>();
    private Texture2D waterTexture;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polyCollider;



    private float impulseIntensity = 0f;
    private float impulsePosition = 0f;

    public bool canImpact;
    public bool CanSplash() { return canImpact; }

    [Space(5)]
    [SerializeField] private float impulseWidth = 30f;



    public static WaterLevel Instance { get; private set; }

    private void Start()
    {
        Instance = this;

        for (int i = 0; i < waterPointsCount; i++)
        {
            waterPoints.Add(Instantiate(waterHitPoint, transform).GetComponent<WaterImpulseTrigger>());
        }

        waterTexture = new Texture2D(textureWidth, textureHeight);
        waterTexture.filterMode = FilterMode.Point;
        waterTexture.wrapMode = TextureWrapMode.Repeat;

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        spriteRenderer.sprite = Sprite.Create(waterTexture, new Rect(0, 0, waterTexture.width, waterTexture.height), new Vector2(0.5f, 0.5f), 1f);

        polyCollider = GetComponent<PolygonCollider2D>();
        polyCollider.points = new Vector2[waterPointsCount + 2];
        DrawWaterCurve();

    }

    private void Update()
    {
        float time = Time.time * speed;

        for (int i = 0; i < waterPointsCount; i++)
        {
            float xPos = -textureWidth / 2 + (textureWidth / (waterPointsCount - 1)) * i;

            float yPos = waterHeight * Mathf.Sin(waterFrequency * (xPos * Mathf.PI / 180f) + time) + offset;

            float impulse = impulseIntensity * Mathf.Exp(-Mathf.Pow(xPos - impulsePosition, 2) / (2 * impulseWidth * impulseWidth));

            float downwardEffect = Mathf.Exp(-Mathf.Pow(xPos - impulsePosition, 2) / (2 * Mathf.Pow(impulseWidth, 2))) * Mathf.Cos(2f * time + xPos * 0.1f);



            yPos += downwardEffect * impulse;

            waterPoints[i].transform.localPosition = new Vector2(xPos, yPos);

        }
        Vector2[] colliderPoints = new Vector2[waterPointsCount + 2];
        for (int i = 0; i < waterPointsCount; i++)
        {
            colliderPoints[i] = waterPoints[i].transform.localPosition;
        }

        colliderPoints[waterPointsCount] = new Vector2(textureWidth / 2, -textureHeight);
        colliderPoints[waterPointsCount + 1] = new Vector2(-textureWidth / 2, -textureHeight);

        polyCollider.points = colliderPoints;
        // Redraw the water curve
        DrawWaterCurve();
    }



    public void StartImpulse(float impulseInitialIntensity, float impulseDuration, WaterImpulseTrigger waterImpulseTrigger)
    {
        impulseIntensity = impulseInitialIntensity;
        impulsePosition = waterImpulseTrigger.transform.localPosition.x;

        StartCoroutine(GradualImpulse(impulseInitialIntensity, impulseDuration));
    }

    IEnumerator GradualImpulse(float startIntensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            impulseIntensity = Mathf.Lerp(0f, startIntensity, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        impulseIntensity = startIntensity;

        float dampingDuration = 2f; // Damping time after the impact
        elapsed = 0f;

        while (elapsed < dampingDuration)
        {
            impulseIntensity = Mathf.Lerp(startIntensity, 0f, elapsed / dampingDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        impulseIntensity = 0f;  // Set it to 0 after the damping is complete
    }








    public float GetHeightAtPosition(float x)
    {
        float distance = 10000f;
        Transform closestPoint = null;

        foreach(WaterImpulseTrigger t in waterPoints)
        {
            float currentDistance = Mathf.Abs(t.transform.position.x - x);
            if(currentDistance < distance)
            {
                closestPoint = t.transform;
                distance = currentDistance;
            }
        }

        if (closestPoint != null)
        {
            return closestPoint.position.y;
        }

        return 0f;  
    }









    private void DrawWaterCurve()
    {
        Color[] texturePixels = new Color[textureWidth * textureHeight];

        for (int i = 0; i < texturePixels.Length; i++)
        {
            texturePixels[i] = Color.clear;
        }

        // Draw the water curve
        for (int i = 0; i < waterPointsCount - 3; i++)
        {
            Vector2 p0 = new Vector2(waterPoints[i].transform.localPosition.x + textureWidth / 2, waterPoints[i].transform.localPosition.y + textureHeight / 2);
            Vector2 p1 = new Vector2(waterPoints[i + 1].transform.localPosition.x + textureWidth / 2, waterPoints[i + 1].transform.localPosition.y + textureHeight / 2);
            Vector2 p2 = new Vector2(waterPoints[i + 2].transform.localPosition.x + textureWidth / 2, waterPoints[i + 2].transform.localPosition.y + textureHeight / 2);
            Vector2 p3 = new Vector2(waterPoints[i + 3].transform.localPosition.x + textureWidth / 2, waterPoints[i + 3].transform.localPosition.y + textureHeight / 2);

            // Draw the Catmull-Rom spline
            DrawCatmullRomSpline(p0, p1, p2, p3, curveColor, curveThickness, texturePixels);

            // Fill the area under the curve
            FillUnderCurve(p0, p1, p2, p3, fillColor, texturePixels);
        }

        waterTexture.SetPixels(texturePixels);
        waterTexture.Apply();
    }

    private void FillUnderCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color, Color[] texturePixels)
    {
        int segmentCount = 100;
        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            Vector2 point = CatmullRom(p0, p1, p2, p3, t);
            int x = Mathf.RoundToInt(point.x);
            int y = Mathf.RoundToInt(point.y);

            // Fill downwards from the curve to the bottom of the texture
            for (int fillY = 0; fillY <= y; fillY++)
            {
                if (x >= 0 && x < textureWidth && fillY >= 0 && fillY < textureHeight)
                {
                    texturePixels[fillY * textureWidth + x] = color;
                }
            }
        }
    }

    private void DrawCatmullRomSpline(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color, int thickness, Color[] texturePixels)
    {
        int segmentCount = 100;
        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            Vector2 point = CatmullRom(p0, p1, p2, p3, t);
            int x = Mathf.RoundToInt(point.x);
            int y = Mathf.RoundToInt(point.y);

            // Calculate the tangent and normal vectors
            Vector2 tangent = (CatmullRom(p0, p1, p2, p3, t + 0.01f) - point).normalized;
            Vector2 normal = new Vector2(-tangent.y, tangent.x);

            // Draw the curve with thickness
            for (int tOffset = -thickness / 2; tOffset <= thickness / 2; tOffset++)
            {
                Vector2 offset = normal * tOffset;
                int offsetX = Mathf.RoundToInt(x + offset.x);
                int offsetY = Mathf.RoundToInt(y + offset.y);

                if (offsetX >= 0 && offsetX < textureWidth && offsetY >= 0 && offsetY < textureHeight)
                {
                    texturePixels[offsetY * textureWidth + offsetX] = color;
                }
            }
        }
    }

    private Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float x = 0.5f * ((2f * p1.x) + (-p0.x + p2.x) * t + (2f * p0.x - 5f * p1.x + 4f * p2.x - p3.x) * t2 + (-p0.x + 3f * p1.x - 3f * p2.x + p3.x) * t3);
        float y = 0.5f * ((2f * p1.y) + (-p0.y + p2.y) * t + (2f * p0.y - 5f * p1.y + 4f * p2.y - p3.y) * t2 + (-p0.y + 3f * p1.y - 3f * p2.y + p3.y) * t3);

        return new Vector2(x, y);
    }
}
