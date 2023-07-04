using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;

    private Vector2 originalLocalPosition;
    private Collider2D swordCollider;

    private void Awake()
    {
        swordCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    public void AttackRight()
    {
        Debug.Log("Attack Right");
        swordCollider.enabled = true;
        transform.localPosition = originalLocalPosition;
    }

    public void AttackLeft()
    {
        Debug.Log("Attack Left");
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(originalLocalPosition.x - 1f, originalLocalPosition.y);
    }

    public void AttackUp()
    {
        Debug.Log("Attack Up");
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(originalLocalPosition.x, originalLocalPosition.y + 0.8f); // Adjust the Y position as desired
    }

    public void AttackDown()
    {
        Debug.Log("Attack Down");
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(originalLocalPosition.x - 1f, originalLocalPosition.y - 0.8f); // Adjust the Y position as desired
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
