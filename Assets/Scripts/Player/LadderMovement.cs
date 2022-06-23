using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LadderMovement : MonoBehaviour
{
    public float speed = 2f;
    private float _vertical;
    private float _gravityScale;
    
    private bool _isOnLadder;
    private bool _isClimbing;

    private Rigidbody2D _rb;
    private Animator _animator;
    private AudioSource _audio;

    public AudioClip _climbing;

    // Update is called once per frame
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _gravityScale = _rb.gravityScale;
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _vertical = Input.GetAxis("Vertical");
        if (!_isOnLadder) return;
        
        _isClimbing = Mathf.Abs(_vertical) > 0;

        _animator.SetBool("isClimbing", _isClimbing);
        
        _rb.gravityScale = 0f;
        GetComponent<PlayerCombat>().enabled = false;

    }

    private void FixedUpdate()
    {
        if (!_isClimbing) return;
        if (!_audio.isPlaying) _audio.PlayOneShot(_climbing);
        
        _rb.velocity = new Vector2(_rb.velocity.x, _vertical * speed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Ladder")) return;
        
        _isOnLadder = true;
        _animator.SetTrigger("startClimbing");
        _animator.SetBool("isOnLadder", true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Ladder")) return;

        _isOnLadder = true;
        _animator.SetBool("isOnLadder", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Ladder")) return;
        
        Debug.Log("left ladder");
        
        _isOnLadder = false;
        _isClimbing = false;
        
        _animator.SetBool("isOnLadder", false);
        _animator.SetBool("isClimbing", false);
        
        GetComponent<PlayerCombat>().enabled = true;
        _rb.gravityScale = _gravityScale;
    }
}
