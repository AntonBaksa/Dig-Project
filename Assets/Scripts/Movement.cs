using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public int maxHealth = 100;
    public int attackDamage = 20;
    public float attackRange = 1f;
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public Transform attackPointUp;
    public Transform attackPointDown;
    public LayerMask opponentLayer;
    public Image healthBar;
    public int playerID; // Assign 1 or 2 in Unity Inspector

    private int currentHealth;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private ScoreManager scoreManager;
    private Vector3 startPosition;

    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode attackKey;
    [SerializeField] KeyCode downKey;
    [SerializeField] KeyCode upKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        scoreManager = FindObjectOfType<ScoreManager>();
        startPosition = transform.position;
        UpdateHealthBar();
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(leftKey))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            isFacingRight = false;
        }

        if (Input.GetKey(rightKey))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            isFacingRight = true;
        }

        if (Input.GetKeyDown(jumpKey) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Transform attackPoint = GetActiveAttackPoint();
        if (attackPoint == null) return;

        Collider2D[] hitOpponents = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, opponentLayer);
        foreach (Collider2D opponent in hitOpponents)
        {
            PlayerController opponentScript = opponent.GetComponent<PlayerController>();
            if (opponentScript != null)
            {
                opponentScript.TakeDamage(attackDamage);
            }
        }
    }

    private Transform GetActiveAttackPoint()
    {
        if (Input.GetKey(downKey)) return attackPointDown;
        if (Input.GetKey(upKey)) return attackPointUp;
        return isFacingRight ? attackPointRight : attackPointLeft;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        if (scoreManager != null)
        {
            scoreManager.AwardPoint(playerID);
        }
        Respawn();
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackPointRight != null) Gizmos.DrawWireSphere(attackPointRight.position, attackRange);
        if (attackPointLeft != null) Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);
        if (attackPointUp != null) Gizmos.DrawWireSphere(attackPointUp.position, attackRange);
        if (attackPointDown != null) Gizmos.DrawWireSphere(attackPointDown.position, attackRange);
    }
}
