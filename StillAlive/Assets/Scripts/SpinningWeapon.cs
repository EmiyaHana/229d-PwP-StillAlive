using UnityEngine;

public class SpinningWeapon : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Damage")]
    public float damage = 1f;

    [Header("RotationalMotion")]
    public float targetAngularVelocity = 1000f;
    public float timeToReachSpeed = 0.5f;

    private Rigidbody2D rb;
    private float momentOfInertia; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * moveSpeed; 
        momentOfInertia = rb.inertia; //Inertia for object

        float angularAcceleration = targetAngularVelocity / timeToReachSpeed; //Calculates for Acceleration
        float appliedTorque = momentOfInertia * angularAcceleration; //calculates for Torque force (Tau = I * Alpha)
        rb.AddTorque(appliedTorque);

        Destroy(gameObject, 2.5f);
    }

    void OnTriggerEnter2D(Collider2D col) //Use Collision2D for detect object
    {
        if (col.CompareTag("Zombie")) //if detect 'zombie'
        {
            col.GetComponent<ZombieController>().TakeDamage(damage);
        }
        else if (col.CompareTag("Wall")) //if detect 'wall'
        {
            Destroy(gameObject);
        }
    }
}