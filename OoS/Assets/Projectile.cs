using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);

    Rigidbody2D rb;
    public float destroyTime = 3f; // Waktu sebelum menghancurkan proyektil

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Hit the target
            bool gotHit = damageable.Hit(damage, deliveredKnockback);

            if (gotHit)
            {
                Debug.Log(collision.name + " Hit " + damage);
                Destroy(gameObject);
            }
        }
        else
        {
            // Destroy the projectile if it hits an object other than the damageable ones
            Destroy(gameObject);
        }
    }
}
