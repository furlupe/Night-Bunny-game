using System;
using System.Collections;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float maxSpeed = 2f;
    
    private bool _isWaiting, _isPatrolling;

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float patrolZone;
    private Vector3 _spawnPoint;

    private bool _turnedOnEdge;

    private static readonly int PlayerSpotted = Animator.StringToHash("playerSpotted");

    private Enemy _enemy;

    public void Init(Enemy enemy)
    {
        _enemy = enemy;
        player = _enemy.Player.transform;
    }

    private void Move(bool diff)
    {
        var newVelocity = new Vector2(_enemy.dir * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        var tpos = transform.position;

        var condition = tpos.x < _spawnPoint.x - patrolZone && !_enemy.facingRight
                        || tpos.x > _spawnPoint.x + patrolZone && _enemy.facingRight;

        _isPatrolling = true;

        var onEdge = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if (onEdge.CompareTag("Edge") && !_turnedOnEdge)
        {
            _turnedOnEdge = true;
            StartCoroutine(Wait(2, true));
        }

        else if (!onEdge.CompareTag("Edge"))
        {
            _turnedOnEdge = false;
        }

        if (diff)
        {
            condition = tpos.x < player.position.x && !_enemy.facingRight
                        || tpos.x > player.position.x && _enemy.facingRight;
            _isPatrolling = false;
        }

        if (condition)
        {
            if (!_isPatrolling)
            {
                Debug.Log("attacking");
                GetComponent<Enemy>().Flip();
                return;
            }

            StartCoroutine(Wait(2, true));
        }

        _enemy.rb.velocity = newVelocity + _enemy.outsideForces;
    }

    public IEnumerator Wait(int secs, bool patrol)
    {
        _isWaiting = true;
        yield return new WaitForSeconds(secs);

        _isWaiting = false;
        if (patrol) GetComponent<Enemy>().Flip();
    }

    private void Start()
    {
        _spawnPoint = transform.position;
    }

    private void Update()
    {
        //Debug.Log(_enemy.FieldOfView);
        var diff = GetComponent<Enemy>().CheckIfPlayerWithinFov(_enemy.FieldOfView);
        _enemy.Animator.SetBool(PlayerSpotted, diff);

        if (!_isWaiting) Move(diff);
        else
        {
            _enemy.rb.velocity = Vector2.zero;
        }
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        _enemy.Audio.PlayOneShot(clip);
    }
}