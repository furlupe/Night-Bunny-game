using UnityEngine;

public class NPC : MonoBehaviour
{
    public Vector2 fieldOfView = new (4, 0);
    public Transform player;
    public DialogueManager dialogueManager;

    public Dialogue dialogue;
    public GameObject dialogueWindow;

    private bool _playerActive = true;
    private void Update()
    {
        if (!((player.position - transform.position).magnitude < fieldOfView.magnitude)
            || !Input.GetKeyDown(KeyCode.E) || dialogueManager.isActive)
        {
            if (!dialogueManager.isEnded || _playerActive) return;
            
            player.GetComponent<Player>().Enable();
            _playerActive = true;

            return;
        }

        dialogueWindow.SetActive(true);
        dialogueManager.StartDialogue(dialogue);

        if (!_playerActive) return;
        
        player.GetComponent<Player>().Disable();
        _playerActive = false;
    }
}