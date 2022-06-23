using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public new string name = "bell";

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Player")) return;

        var playerInventory = col.GetComponent<Player>().Inventory;

        if (!playerInventory.Keys.ToList().Contains(name)) 
            playerInventory[name] = 0;
        
        playerInventory[name]++;
        
        Debug.Log("Collide!");
        
        Destroy(gameObject);
    }
}
