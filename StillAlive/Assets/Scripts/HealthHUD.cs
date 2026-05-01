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
                Sprite: hearts[i].sprite = fullHeart;
            }
            else
            {
                Sprite: hearts[i].sprite = emptyHeart;
            }
        }
    }
}