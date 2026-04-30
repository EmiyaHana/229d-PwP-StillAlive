using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [Header("Setting")]
    public float moveSpeed = 2.5f;
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject ammoBoxPrefab;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    void OnTriggerEnter2D(Collider2D col) //use Trigger2D to detect weapon
    {
        if (col.CompareTag("Weapon"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (ammoBoxPrefab != null)
        {
            Instantiate(ammoBoxPrefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}