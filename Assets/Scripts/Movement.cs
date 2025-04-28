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
    public SpriteRenderer spriteToFlip; // Assign the specific sprite in Unity Inspector
    public Animator animator; // Reference to the Animator component

    private int currentHealth;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private ScoreManager scoreManager;
    private Vector3 startPosition;

    private KeyCode jumpKey;
    private KeyCode leftKey;
    private KeyCode rightKey;
    private KeyCode attackKey;
    private KeyCode downKey;
    private KeyCode upKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        scoreManager = FindObjectOfType<ScoreManager>();
        startPosition = transform.position;
        UpdateHealthBar();
        SetPlayerControls();
    }

    private void SetPlayerControls()
    {
        if (playerID == 1)
        {
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            jumpKey = KeyCode.W;
            downKey = KeyCode.S;
            upKey = KeyCode.W;
            attackKey = KeyCode.F;
        }
        else if (playerID == 2)
        {
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            jumpKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
            upKey = KeyCode.UpArrow;
            attackKey = KeyCode.RightControl;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        bool isMoving = false;

        if (Input.GetKey(leftKey))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (isFacingRight)
            {
                Flip();
            }
            isFacingRight = false;
            isMoving = true;
        }

        if (Input.GetKey(rightKey))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (!isFacingRight)
            {
                Flip();
            }
            isFacingRight = true;
            isMoving = true;
        }

        if (Input.GetKeyDown(jumpKey) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (animator != null)
            {
                animator.SetBool("jump", true); // Trigger jump animation
                animator.SetBool("jump", false);

            }
        }

        if (animator != null)
        {
            animator.SetBool("run", isMoving);
        }
    }

    private void Flip()
    {
        if (spriteToFlip != null)
        {
            spriteToFlip.flipX = !spriteToFlip.flipX;
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

        // Set the attack animations based on direction
        if (attackPoint == attackPointRight || attackPoint == attackPointLeft)
        {
            if (animator != null)
            {
                animator.SetBool("attack", true); // Right or Left Attack
            }
        }
        else if (attackPoint == attackPointUp)
        {
            if (animator != null)
            {
                animator.SetBool("attackUp", true); // Upward Attack
            }
        }
        else if (attackPoint == attackPointDown)
        {
            if (animator != null)
            {
                animator.SetBool("attackDown", true); // Downward Attack
            }
        }

        // Reset attack animations after they are done
        StartCoroutine(ResetAttackAnimations());
    }

    private Transform GetActiveAttackPoint()
    {
        if (Input.GetKey(downKey)) return attackPointDown;
        if (Input.GetKey(upKey)) return attackPointUp;
        return isFacingRight ? attackPointRight : attackPointLeft;
    }

    private IEnumerator ResetAttackAnimations()
    {
        // Wait for the animation to play before resetting
        yield return new WaitForSeconds(0.3f); // Adjust this value depending on your animation length

        if (animator != null)
        {
            animator.SetBool("attack", false);
            animator.SetBool("attackUp", false);
            animator.SetBool("attackDown", false);
        }
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