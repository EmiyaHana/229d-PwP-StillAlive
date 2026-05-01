using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [Header("Setting")]
    public float moveSpeed = 2.5f;
    public float maxHealth = 3f;
    private float currentHealth;

    [Tooltip("Offset")]
    public float angleOffset = 0f;

    public GameObject ammoBoxPrefab;

    private float knockbackTimer = 0f;

    private Transform player;
    private Rigidbody2D rb;

    private bool isDead = false;

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
            if (knockbackTimer > 0)
            {
                knockbackTimer -= Time.fixedDeltaTime;
                return; 
            }

            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
            rb.rotation = angle;
        }
    }

    void OnTriggerStay2D(Collider2D col) //use Trigger2D to detect player
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    void OnTriggerEnter2D(Collider2D col) //use Trigger2D to detect weapon
    {
        if (col.CompareTag("Weapon"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float damage, Vector2 knockbackDir = default, float knockbackForce = 0f)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (knockbackForce > 0)
        {
            knockbackTimer = 0.2f;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public void ApplyWaveBuff(int waveIndex)
    {
        maxHealth += waveIndex;
        moveSpeed += (waveIndex * 0.2f);
        currentHealth = maxHealth;
    }

    void Die()
    {
        if (ammoBoxPrefab != null)
        {
            Instantiate(ammoBoxPrefab, transform.position, Quaternion.identity);
        }

        WaveManager waveManager = Object.FindFirstObjectByType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.ZombieDied();
        }
        
        Destroy(gameObject);
    }
}