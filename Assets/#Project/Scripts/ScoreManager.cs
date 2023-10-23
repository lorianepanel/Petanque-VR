using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    GameObject[] balls;


    // Start is called before the first frame update
    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("Ball"); 
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject ball in balls)
        {
            Debug.Log(balls.Length);
        }
    }
}
