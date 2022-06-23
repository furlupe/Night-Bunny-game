using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public Animator animator = new();
    public AudioSource audio;

    private SpriteRenderer _spriteRenderer;

    public GameObject spawnable;
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Speed = Animator.StringToHash("Speed");

    public Dictionary<string, int> Inventory = new();

    private bool _isInvincible;
    public float invincibilityDuration = 2f;
    public float invincibilityBlinkDuration = 0.1f;

    void Spawn(GameObject objectPrefab)
    {
        var obj = Instantiate(objectPrefab);
        obj.GetComponent<EnemyAI>().player = gameObject.transform;
    }

    public void TakeDamage(int damage, int direction)
    {
        if (_isInvincible || damage <= 0) return;
        GetComponent<CharacterController>().Knockback(direction);

        health -= damage;

        if (health <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        StartCoroutine(
            BecomeInvincibleForSeconds(invincibilityDuration)
        );
    }

    private IEnumerator BecomeInvincibleForSeconds(float seconds)
    {
        _isInvincible = true;

        var blinks = seconds / (2 * invincibilityBlinkDuration);
        for (var i = 0; i < blinks; i++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(invincibilityBlinkDuration);

            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(invincibilityBlinkDuration);
        }

        _isInvincible = false;
    }

    public void Disable()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;

        animator.SetBool(Grounded, true);
        animator.SetFloat(Speed, 0);
    }

    public void Enable()
    {
        GetComponent<CharacterController>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;
    }

    void Start()
    {
        health = 100;
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Spawn(spawnable);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}