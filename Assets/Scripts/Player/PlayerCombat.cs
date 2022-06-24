using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.Bushes;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audio;

    public AudioClip swing;

    public LayerMask enemyLayers;
    public Transform attackPoint;

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float meleeCooldown = 0.5f;
    private float _timer;
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    private void Start()
    {
        _animator = GetComponent<Player>().animator;
        _audio = GetComponent<Player>().audio;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) ||
            !(Time.time > _timer)) return;
        Attack();
        _timer = Time.time + meleeCooldown;
    }

    private void Attack()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _animator.SetTrigger(Attack1);

        _audio.Stop();
        _audio.PlayOneShot(swing);

        var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (var enemy in hitEnemies)
        {
            var parent = enemy.transform.parent;

            if (parent.GetComponent<Bushes>() != null)
                parent.GetComponent<Bushes>().TakeDamage(attackDamage);
            
            else if (parent.GetComponent<Spider>() != null)
                parent.GetComponent<Spider>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}