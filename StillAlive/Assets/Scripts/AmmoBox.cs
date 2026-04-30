using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 5;

    void OnTriggerEnter2D(Collider2D col) //use Collision2D to detect player
    {
        if (col.CompareTag("Player"))
        {
            PlayerController player = col.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddAmmo(ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}