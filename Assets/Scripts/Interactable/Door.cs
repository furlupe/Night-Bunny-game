using UnityEngine;

namespace Interactable
{
    public class Door : MonoBehaviour
    {
        private Animator _animator;
        private AudioSource _audio;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audio = GetComponent<AudioSource>();
        }

        public void Open()
        {
            _animator.SetTrigger("Open");
            _audio.Play();
        }

        public void Disable()
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}
