using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantrap : MonoBehaviour
{
    public Animator animator;

    public int damage = 40;

    public AudioClip activateSound, defusedSound;
    private AudioSource _audio;

    private bool _activated, _defused;
    private Collider2D _player;

    private QTEManager _qte;
    private readonly Dictionary<KeyCode, GameObject> _qteDict = new();

    private static readonly int Activate = Animator.StringToHash("Activate");
    private static readonly int Defused = Animator.StringToHash("defused");

    private void Start()
    {
        _audio = GetComponent<AudioSource>();

        _qteDict.Add(
            KeyCode.E,
            transform.GetChild(0).gameObject
        );

        _qte = GetComponent<QTEManager>();
        _qte.Init(_qteDict);

        _qte.enabled = false;
    }

    private void Update()
    {
        if (!(_activated && gameObject.GetComponent<QTEManager>().eventComplete)) return;

        _player.GetComponent<Player>().Enable();

        animator.SetBool(Defused, true);
        _audio.PlayOneShot(defusedSound);

        _defused = true;
        _activated = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        _player = col;

        if (_activated || _defused) return;

        _audio.PlayOneShot(activateSound);
        animator.SetTrigger(Activate);

        col.GetComponent<Player>().Disable();

        col.GetComponent<Player>().TakeDamage(
            damage,
            0
        );

        _qte.enabled = true;
        transform.GetChild(0).position =
            new Vector3(
                col.transform.position.x,
                col.transform.position.y + 1.25f,
                col.transform.position.z
            );

        _activated = true;
    }
}