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

    [Header("Ammo System")]
    public int maxAmmo = 10; // กระสุนสูงสุด
    private int currentAmmo;
    public TextMeshProUGUI ammoTextUI;

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
            Instantiate(spinningWeaponPrefab, firePoint.position, firePoint.rotation);
            
            currentAmmo--;
            UpdateAmmoUI();
        }
        else
        {
            Debug.Log("Ammo out!");
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
}