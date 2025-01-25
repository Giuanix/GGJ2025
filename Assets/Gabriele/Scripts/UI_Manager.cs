using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [HideInInspector()] public int amount;
    [HideInInspector()] public Transform targetPlayer;

    [SerializeField] private Image powerUp;
    [SerializeField] private Transform playerIndicator;

    [SerializeField] private Text outlineDamageText;
    [SerializeField] private Text damageText;
    [SerializeField] private Color maxColor;
    [SerializeField] private bool percentaceToLeft = false;

    int startSizeOutline;
    int startSizeDamageText;


    public void Start()
    {
        startSizeOutline = outlineDamageText.fontSize;
        startSizeDamageText = damageText.fontSize;
        playerIndicator.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (targetPlayer)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPlayer.position + new Vector3(0, 1f, 0));
            playerIndicator.position = screenPos;
        }

    }

    public void UpdateText(int max)
    {
        string t = (percentaceToLeft ? "%" : "") + amount.ToString() + (percentaceToLeft ? "" : "%");
        outlineDamageText.text = t;
        damageText.text = t;
        damageText.color = Color.Lerp(Color.white, maxColor, (float)amount / max);

        outlineDamageText.fontSize = startSizeOutline + Mathf.FloorToInt(amount/(max/5));
        damageText.fontSize = startSizeDamageText + Mathf.FloorToInt(amount/(max/5));
    }


    public void SetupPowerUpImage(Sprite sprite)
    {
        if (sprite)
        {
            powerUp.color = Color.white;
            powerUp.sprite = sprite;
        }
        else
        {
            powerUp.color = Color.clear;
        }
    }

    public void DeactivateUI()
    {
        playerIndicator.gameObject.SetActive(false);
    }
}
