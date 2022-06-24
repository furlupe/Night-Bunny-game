using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText, dialogueText;
    private Queue<string> _sentences = new ();

    public bool isActive;
    public bool isEnded;
    public Transform dialogBox;

    private GameObject _parent;

    private void Start()
    {
        _parent = transform.parent.gameObject;
        
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        transform.parent.gameObject.SetActive(true);

        dialogBox.position = new Vector3(Screen.width / 2, 70, 0);
        
        _sentences.Clear();
        foreach (var sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }

        isActive = true;
        isEnded = false;
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        _parent.SetActive(false);
        isActive = false;
        isEnded = true;
    }
}