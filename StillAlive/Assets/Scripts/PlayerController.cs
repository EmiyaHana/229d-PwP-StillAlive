using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Setting Movement")]
    public float moveSpeed = 5f;
    public Camera cam;

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
    public float meleeDamage = 1f;
    public float knockbackForce = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            MeleeAttack();
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

        foreach (Collider2D hit in hitZombies)
        {
            if (hit.CompareTag("Zombie"))
            {
                Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;

                hit.GetComponent<ZombieController>().TakeDamage(meleeDamage, knockbackDir, knockbackForce);
                Debug.Log("Slam!!!");
            }
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

    void OnDrawGizmosSelected()
    {
        if (meleePoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }
}