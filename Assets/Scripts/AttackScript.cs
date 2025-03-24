using System.Collections;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public Animator animator; // Referens till Animator
    public Transform attackPoint; // Punkt där attacken träffar
    public float attackRange = 0.5f; // Radie för attacken
    public LayerMask enemyLayers; // Fiendens LayerMask
    public int attackDamage = 20; // Skada på fiender

    public float attackCooldown = 0.5f; // Tid mellan attacker
    private bool canAttack = true; // Kontroll för cooldown

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

        // Vänta lite så att animationen hinner träffa
        yield return new WaitForSeconds(0.2f);

        // Kolla om fiender träffas
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
        }

        // Vänta innan spelaren kan attackera igen
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
