using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BallBehaviour : MonoBehaviour
{

    public enum PlayerColor
    {
        Red,
        Blue,
        None
    }


    public PlayerColor playerColor;

    private Rigidbody rb;

    public bool isStable 
    {
        get { return rb.velocity.magnitude < 0.001f; }

    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Debug.Log(rb.velocity.magnitude);
    }

}
