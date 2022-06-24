using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SpiderProjectile : MonoBehaviour
{
    public float projectileSpeed = 0.5f;
    public int projectileDamage = 1;

    public float flyingTime = 7f;
    private float _timer;

    private bool _move;

    private Vector3 _dir;

    private void Update()
    {
        if (!_move) return;
        transform.position += _dir * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.tag);
        switch (col.tag)
        {
            case "Player":
                col.GetComponent<Player>().TakeDamage(
                    projectileDamage, 
                    0
                );
                
                Destroy(gameObject);
                _move = false;
                break;

            case "Edge":
            case "Terrain":
                Destroy(gameObject);
                _move = false;
                break;
        }
    }

    public void Launch(Vector3 pos)
    {
        _move = true;
        _dir = (pos - transform.position).normalized;
    }
}