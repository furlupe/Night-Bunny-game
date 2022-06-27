using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            0
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lose()
    {
        GetComponent<Animator>().SetTrigger("LoseHealth");
    }
}
