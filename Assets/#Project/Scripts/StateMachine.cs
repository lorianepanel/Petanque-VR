using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
    public GameObject goal;

    public GameObject ballBlue;

    public GameObject ballRed;

    [SerializeField] Transform spawnArea;


    public enum GameState{
        WaitForGoal,
        GoalLaunched,
        WaitForP1,
        P1HasPlayed,
        WaitForP2,
        P2HasPlayed,
        RoundFinished,
        ValidationScore
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

        goal = Instantiate(goal, spawnArea.position, transform.rotation);

        // else if (state == GameState.WaitForP1)
        // {
        //     Instantiate(ballBlue, spawnArea.position, transform.rotation);
        // }

        // else if (state == GameState.WaitForP2)
        // {
        //     Instantiate(ballRed, spawnArea.position, transform.rotation);
        // }
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void UpdateWaitForGoal()
    {
        if(terrainInfo.IsIn(goal)){
            state = GameState.GoalLaunched;
        }
    }

    private void UpdateGoalLaunched()
    {
        // ici code : if(terrainInfo.IsStable(goal)){ state = GameState.WaitForP1;}
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
            break;

            case GameState.WaitForP1:
            currentStateText.SetText("Wait for P1");
            break;

            case GameState.P1HasPlayed:
            currentStateText.SetText("P1 has played");
            break;

            case GameState.WaitForP2:
            currentStateText.SetText("Wait for P2");
            break;

            case GameState.P2HasPlayed:
            currentStateText.SetText("P2 has played");
            break;

            case GameState.RoundFinished:
            currentStateText.SetText("Round finished");
            break;

            case GameState.ValidationScore:
            currentStateText.SetText("Validation Score");
            break;
        }

        
    }

}
