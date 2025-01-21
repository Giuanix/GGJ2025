using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBubble : MonoBehaviour
{
    [SerializeField] private int bubbleSize = 50;  // The diameter of the bubble
    [SerializeField] private int textureSize = 100; // The resolution of the texture
    [SerializeField] private Color[] colorPalette;
    [SerializeField] private int colorCounter = 5;
    [SerializeField] private int thickness = 1;

    private Texture2D bubbleTexture;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        bubbleTexture = new Texture2D(textureSize, textureSize);
        bubbleTexture.filterMode = FilterMode.Point;
        bubbleTexture.wrapMode = TextureWrapMode.Clamp;

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        spriteRenderer.sprite = Sprite.Create(bubbleTexture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f), 1f);

        DrawBubbleCircle();
    }

    private void DrawBubbleCircle()
    {
        Color[] texturePixels = new Color[textureSize * textureSize];

        // Clear texture
        for (int i = 0; i < texturePixels.Length; i++)
        {
            texturePixels[i] = Color.clear;
        }

        int radius = bubbleSize / 2;
        int textureCenter = textureSize / 2;
        int counter = 0;

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                int px = textureCenter + x;
                int py = textureCenter + y;

                if (px >= 0 && px < textureSize && py >= 0 && py < textureSize)
                {
                    int distanceSquared = x * x + y * y;
                    if (Mathf.Abs(distanceSquared - (radius * radius)) < thickness)
                    {
                        texturePixels[py * textureSize + px] = colorPalette[counter];

                        if (y % colorCounter == 0)
                        {
                            counter++;
                            if (counter == colorPalette.Length) counter = 0;
                        }
                    }
                    else if (distanceSquared <= (radius * radius))
                    {

                        texturePixels[py * textureSize + px] =  new Color(1,1,1,0.1f);
                    }
                }
            }
        }

        bubbleTexture.SetPixels(texturePixels);
        bubbleTexture.Apply();
    }
}
