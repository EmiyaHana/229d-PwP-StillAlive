using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Setting Movement")]
    public float moveSpeed = 5f;
    public Camera cam;

    [Header("HPSetting")]
    public int maxHealth = 3;
    public int currentHealth;
    public float invincibilityDuration = 1.5f;
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    [Header("Weapon Range")]
    public GameObject spinningWeaponPrefab;
    public Transform firePoint;
    [HideInInspector] public float rangedDamageBonus = 0f;

    [HideInInspector] public float weaponScaleMultiplier = 1f;

    [Header("Ammo System")]
    public int maxAmmo = 10;
    private int currentAmmo;
    public TextMeshProUGUI ammoTextUI;

    [Header("Melee Weapon")]
    public Transform meleePoint;
    public float meleeRange = 1.5f;
    public float meleeDamage = 0.5f;
    public float knockbackForce = 5f;

    public float meleeCooldown = 1.5f;
    private float nextMeleeTime = 0f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;

            float alpha = Mathf.PingPong(Time.time * 10f, 1f);
            spriteRenderer.color = new Color(1, 1, 1, alpha);
        }

        else if (isInvincible)
        {
            isInvincible = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Time.time >= nextMeleeTime) 
            {
                MeleeAttack();
                nextMeleeTime = Time.time + meleeCooldown; 
            }
            else
            {
                Debug.Log("Cooldowning...");
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            GameObject spawnedWeapon = Instantiate(spinningWeaponPrefab, firePoint.position, firePoint.rotation);

            spawnedWeapon.GetComponent<SpinningWeapon>().damage += rangedDamageBonus;
            
            currentAmmo--;
            UpdateAmmoUI();
        }
        else
        {
            Debug.Log("Ammo out!");
        }
    }

    void MeleeAttack()
    {
        Collider2D[] hitZombies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange);

        List<GameObject> alreadyHitZombies = new List<GameObject>();

        foreach (Collider2D hit in hitZombies)
        {
            if (hit.CompareTag("Zombie"))
            {
                if (!alreadyHitZombies.Contains(hit.gameObject))
                {
                    Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;
                    hit.GetComponent<ZombieController>().TakeDamage(meleeDamage, knockbackDir, knockbackForce);

                    alreadyHitZombies.Add(hit.gameObject);

                    Debug.Log("Slam!!!");
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log("You got hit. HP remain  : " + currentHealth + "HP.");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
        }
    }

    public void UpdateAmmoUI()
    {
        if (ammoTextUI != null)
        {
            ammoTextUI.text = "Ammo: " + currentAmmo + " / " + maxAmmo;
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Die()
    {
        Object.FindFirstObjectByType<WaveManager>().GameOver(); 
        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (meleePoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }
}