using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡El proyectil golpeó al Player!");

            Destroy(gameObject);
        }
    }
}