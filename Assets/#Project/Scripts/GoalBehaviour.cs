using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    private Rigidbody goalRb;


    public bool goalIsStable 
    {
        get { return goalRb.velocity.magnitude < 0.001f; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
