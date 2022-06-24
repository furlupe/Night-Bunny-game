using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class Pillar : MonoBehaviour
    {
        private Animator _animator;
        public Animator particles;
        private AudioSource _audio;
        private bool _near;

        private readonly Dictionary<KeyCode, GameObject> _qteDict = new();
        private QTEManager _qte;

        public Door door;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audio = GetComponent<AudioSource>();

            _qteDict.Add(KeyCode.E, transform.GetChild(0).gameObject);

            _qte = GetComponent<QTEManager>();
            _qte.Init(_qteDict, fA: 2f, dfA: 0f, sA: 0f);
            
            _qte.enabled = false;
        }

        private void Update()
        {
            if (!_near || !Input.GetKeyDown(KeyCode.E) || _qte.eventComplete) return;
            Activate();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            _near = col.CompareTag("Player");

            if (!_near) return;

            _qte.enabled = true;
            _qte.EnableQte();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _qte.enabled = false;
            _near = false;

            _qte.DisableQte();
        }

        private void Activate()
        {
            _animator.SetTrigger("Activate");
            particles.SetTrigger("Activate");
            _audio.Play();
            //_animator.SetBool("Activated", true);
            door.Open();
        }
    }
}