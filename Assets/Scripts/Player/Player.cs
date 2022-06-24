using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int totalHealth = 5;
    private int health;
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

    private GameObject[] _hpUI = { };
    private int _hpIndex = 0;

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
        for (var d = _hpIndex; d < Math.Min(_hpIndex + damage, totalHealth); d++)
        {
            _hpUI[d].GetComponent<Health>().Lose();
        }

        _hpIndex += damage;

        if (health <= 0)
        {
            Die();
            return;
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

    private void Die()
    {
        animator.SetBool("isDead", true);
        Disable();
    }

    public void Enable()
    {
        GetComponent<CharacterController>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;
    }

    public void Reload(int reloadAfterSeconds)
    {
        Invoke("WaitForSeconds", reloadAfterSeconds);
        SceneManager.LoadScene(Application.loadedLevel);
    }

    void Start()
    {
        totalHealth = 5;
        health = totalHealth;
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hpUI = GameObject.FindGameObjectsWithTag("Health");
        Disable();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Spawn(spawnable);
        }

        if (Input.GetKey(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}