using UnityEngine;

namespace Traps
{
    public class Spikes : MonoBehaviour
    {
        public int damage = 1;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(!col.CompareTag("Player")) return;
        
            col.GetComponent<Player>().TakeDamage(damage, 0);
        }
    }
}
