using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
    public GameObject goal;

    public GameObject ballP1;

    public GameObject ballP2;

    [SerializeField] Transform instantiateArea;

    [SerializeField] Transform spawnArea;


    public enum GameState{
        WaitForGoal,
        GoalLaunched,
        WaitForP1,
        P1HasPlayed,
        WaitForP2,
        P2HasPlayed,
        RoundFinished
    }

    public GameState state;

    [SerializeField]
    private TMP_Text currentStateText;

    TerrainInfo terrainInfo;

    // Start is called before the first frame update
    void Start()
    {
        if(terrainInfo == null){
            terrainInfo = FindObjectOfType<TerrainInfo>();
        }

        state = GameState.WaitForGoal;

        goal = Instantiate(goal, instantiateArea.position, transform.rotation);
        ballP1 = Instantiate(ballP1, instantiateArea.position, transform.rotation);
        ballP2 = Instantiate(ballP2, instantiateArea.position, transform.rotation);


        goal.GetComponent<Rigidbody>().position = spawnArea.position;
        
    }


    void Update()
    {
        UpdateState();
    }



    private void UpdateWaitForGoal()
    {
        if(terrainInfo.IsIn(goal)){
            // si goal est sur le terrain : état suivant
            state = GameState.GoalLaunched;
        }
    }

    private void UpdateGoalLaunched()
    {
        
        if (!terrainInfo.IsIn(goal))
        {
            // si goal n'est pas sur le terrain : état précédent
            state = GameState.WaitForGoal;
        }  
        else if(terrainInfo.IsStable(goal))
        {
            // si goal est stable : état suivant et ballP1 apparait
            state = GameState.WaitForP1;
            ballP1.GetComponent<Rigidbody>().position = spawnArea.position;
        }
    }

    private void UpdateWaitForP1()
    {
        if (!terrainInfo.IsStable(goal))
        {
            // si goal n'est pas stable : état précédent
            state = GameState.GoalLaunched;
        }
        else if(terrainInfo.IsIn(ballP1))
        {
            // si BallP1 est sur le terrain : état suivant
            state = GameState.P1HasPlayed;
        }
    }

    private void UpdateP1HasPlayed()
    {
        if (!terrainInfo.IsIn(ballP1))
        {
            // si ballP1 n'est pas sur le terrain : état précédent
            state = GameState.WaitForP1;
        }
        else if (terrainInfo.IsStable(ballP1))
        {
            // si ballP1 est stable : état suivant et ballP2 apparait
            state = GameState.WaitForP2;
            ballP2.GetComponent<Rigidbody>().position = spawnArea.position;
        }
    }

    private void UpdateWaitForP2()
    {
        if (!terrainInfo.IsStable(ballP1))
        {
            // si ballP1 n'est pas stable : état précédent
            state = GameState.P1HasPlayed;
        }
        else if (terrainInfo.IsIn(ballP2))
        {   
            // si ballP2 est sur le terrain : état suivant
            state = GameState.P2HasPlayed;
        }
    }

    private void UpdateP2HasPlayed()
    {
        if (!terrainInfo.IsIn(ballP2))
        {
            // si ballP2 n'est pas sur le terrain : état précédent
            state = GameState.WaitForP2;
        }
        else if (terrainInfo.IsStable(ballP2))
        {
            // si ballP2 est stable : état suivant (fin du round)
            state = GameState.RoundFinished;
        }
    }

    private void UpdateRoundFinished()
    {
        if (!terrainInfo.IsStable(ballP2))
        {
            // si ballP2 n'est pas stable : état précédent
            state = GameState.P2HasPlayed;
        }

        // sinon : état suivant (score final)
        else state = GameState.WaitForGoal;
    }




    public void UpdateState()
    {
        switch (state)
        {
            case GameState.WaitForGoal:
            currentStateText.SetText("Wait for goal");
            UpdateWaitForGoal();
            break;

            case GameState.GoalLaunched:
            currentStateText.SetText("Goal launched");
            UpdateGoalLaunched();
            break;

            case GameState.WaitForP1:
            currentStateText.SetText("Wait for P1");
            UpdateWaitForP1();
            break;

            case GameState.P1HasPlayed:
            currentStateText.SetText("P1 has played");
            UpdateP1HasPlayed();
            break;

            case GameState.WaitForP2:
            currentStateText.SetText("Wait for P2");
            UpdateWaitForP2();
            break;

            case GameState.P2HasPlayed:
            currentStateText.SetText("P2 has played");
            UpdateP2HasPlayed();
            break;

            case GameState.RoundFinished:
            currentStateText.SetText("Round finished");
            UpdateRoundFinished();
            break;
        }

        
    }

}
