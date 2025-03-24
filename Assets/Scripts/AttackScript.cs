using System.Collections;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public Animator animator; // Referens till Animator
    public Transform attackPoint; // Punkt d�r attacken tr�ffar
    public float attackRange = 0.5f; // Radie f�r attacken
    public LayerMask enemyLayers; // Fiendens LayerMask
    public int attackDamage = 20; // Skada p� fiender

    public float attackCooldown = 0.5f; // Tid mellan attacker
    private bool canAttack = true; // Kontroll f�r cooldown

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;

        // Spela attack-animationen
        animator.SetTrigger("Attack");

        // V�nta lite s� att animationen hinner tr�ffa
        yield return new WaitForSeconds(0.2f);

        // Kolla om fiender tr�ffas
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
        }

        // V�nta innan spelaren kan attackera igen
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
