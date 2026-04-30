using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].enabled = true; // หรือเปลี่ยน Sprite: hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].enabled = false; // หรือเปลี่ยน Sprite: hearts[i].sprite = emptyHeart;
            }
        }
    }
}