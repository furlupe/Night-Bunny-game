using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMove : MonoBehaviour
{
    public float speed = 300000f;
    public float height = 0.5f;

    private Vector2 pos;

    private void Start()
    {
        pos = GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        var newY = Mathf.Sin(Time.time * speed);
        
        GetComponent<RectTransform>().anchoredPosition = new Vector2(
            pos.x, 
            pos.y + height * newY
        );

    }
}
