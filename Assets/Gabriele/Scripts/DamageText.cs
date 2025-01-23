using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [HideInInspector()] public int amount;
    [SerializeField] private Text outline;
    [SerializeField] private Text damageText;
    [SerializeField] private Color maxColor;
    [SerializeField] private bool percentaceToLeft = false;

    int startSizeOutline;
    int startSizeDamageText;

    public void Start()
    {
        startSizeOutline = outline.fontSize;
        startSizeDamageText = damageText.fontSize;
    }

    public void UpdateText(int max)
    {
        string t = (percentaceToLeft ? "%" : "") + amount.ToString() + (percentaceToLeft ? "" : "%");
        outline.text = t;
        damageText.text = t;
        damageText.color = Color.Lerp(Color.white, maxColor, (float)amount / max);

        outline.fontSize = startSizeOutline + Mathf.FloorToInt(amount/(max/5));
        damageText.fontSize = startSizeDamageText + Mathf.FloorToInt(amount/(max/5));
    }

}
