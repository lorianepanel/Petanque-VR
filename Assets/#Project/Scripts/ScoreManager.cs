using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]

public class ScoreManager : MonoBehaviour
{
    GameObject[] balls;
    Transform goal;

    private TMP_Text scoreText;

    private float minDistance;

    private int score;


    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("Ball"); 
        goal = GameObject.FindWithTag("Goal").transform;
        scoreText = GetComponent<TMP_Text>();
        score = 0;
    }


    void Update()
    {
        minDistance = Mathf.Infinity;

        foreach (GameObject ball in balls)
        {
            
            
            float distanceFromGoal = Vector3.Distance(ball.transform.position, goal.position);
            Debug.Log($"{ball.name} : {distanceFromGoal}");

            if(distanceFromGoal < minDistance)
            {
                minDistance = distanceFromGoal;
                // score ++;
                scoreText.SetText($"Score : {score} point(s) <br>Closest ball from goal : {ball.name} <br>Distance from goal : {minDistance:f} m");
                
            }
        }

        
    }
}
