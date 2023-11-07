using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBalls : MonoBehaviour
{
    public GameObject goal;
    public GameObject ballBlue;
    public GameObject ballRed;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(goal);
        Instantiate(ballBlue);
        Instantiate(ballRed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
