using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAttack : MonoBehaviour
{
    private Enemy _enemy;

    public void Init(Enemy enemy)
    {
        _enemy = enemy;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        col.GetComponent<Player>().TakeDamage(
            _enemy.Damage, 
            col.transform.position.x < transform.position.x ? -1 : 1
        );

        StartCoroutine(_enemy.rb.GetComponent<EnemyAI>().Wait(2, false));
    }
}