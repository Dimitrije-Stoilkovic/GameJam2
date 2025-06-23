using System;
using TMPro;
using UnityEngine;

public class TimeLeft : MonoBehaviour
{

    [SerializeField] private BrushController brush;
    
    private float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time = brush.timer;
        int roundedUp = (int)Math.Ceiling(time);
        GetComponent<TextMeshPro>().text = roundedUp.ToString();
    }
}
