using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.XR.CoreUtils;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class StateMachine : MonoBehaviour
{
    public UnityEvent whenStateChanged;

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
    public GameState previousState;

    [SerializeField]
    private TMP_Text currentStateText;

    [SerializeField]
    private TMP_Text announcementText;

    private TerrainInfo terrainInfo;

    private ScoreManager scoreManager;

    private BallsManager ballsManager;

    private AIBehaviour aIBehaviour;

    private bool isRoundRestarting = false;
    

    void Start()
    {

        ballsManager = FindObjectOfType<BallsManager>();
        terrainInfo = FindObjectOfType<TerrainInfo>();
        scoreManager = FindObjectOfType<ScoreManager>();
        aIBehaviour = FindObjectOfType<AIBehaviour>();

        if (ballsManager == null || terrainInfo == null || scoreManager == null || aIBehaviour == null)
        {
            Debug.LogError("One or more managers are not assigned!");
        }
        else
        {
            state = GameState.WaitForGoal;
            previousState = state;
            ballsManager.CreateGoal();
            ballsManager.CreateBall(1);
        }
    }


    void Update()
    {
        UpdateState();
    }


    private void UpdateWaitForGoal()
    {
        if(terrainInfo.IsIn(ballsManager.playingGoal)){
            state = GameState.GoalLaunched;
        }
    }

    private void UpdateGoalLaunched()
    {
        if(terrainInfo.IsStable(ballsManager.playingGoal))
        {
            state = GameState.WaitForP1;
        }

    }

    private void UpdateWaitForP1()
    {

        if(terrainInfo.IsIn(ballsManager.playingBall) && terrainInfo.IsStable(ballsManager.playingBall))
        {
            scoreManager.UpdateCheckTheDistance();
            state = GameState.P1HasPlayed;
        }
    }

    private void UpdateP1HasPlayed()
    {

        //condition a : si P1 et P2 n'ont plus de balles : état RoundFinished
        if (!ballsManager.PlayerStillHaveShoot(1) && !ballsManager.PlayerStillHaveShoot(2))
        {
            scoreManager.UpdateCheckTheDistance();
            scoreManager.CalculateScores();
            state = GameState.RoundFinished;
        }

        // condition b : si P1 a encore des balles et si P1 est perdant ou si P2 n'a plus de balles => P1 rejoue
        else if(ballsManager.PlayerStillHaveShoot(1) && (scoreManager.GetTheLooser() == 1 || !ballsManager.PlayerStillHaveShoot(2)))
        {
            state = GameState.WaitForP1;
            ballsManager.CreateBall(1);
        }
        
        // condition c : si P2 a encore des balles et si P2 est perdant ou qi P1 n'a plus de balles => au tour de P2
        else if(ballsManager.PlayerStillHaveShoot(2) && (scoreManager.GetTheLooser() == 2 || !ballsManager.PlayerStillHaveShoot(1))){
            ballsManager.CreateBall(2);

            StartCoroutine(AICoroutine());
            state = GameState.WaitForP2;
   
        }
    }


    private void UpdateWaitForP2()
    {
        if (terrainInfo.IsIn(ballsManager.playingBall) && terrainInfo.IsStable(ballsManager.playingBall))
        {
            scoreManager.UpdateCheckTheDistance();
            state = GameState.P2HasPlayed;
        }
            
    }

    private void UpdateP2HasPlayed()
    {

        if (!terrainInfo.IsIn(ballsManager.playingBall) || !terrainInfo.IsStable(ballsManager.playingBall))
        {
            return;
        }

        //condition a' : si P1 et P2 n'ont plus de balles : état RoundFinished
        else if (!ballsManager.PlayerStillHaveShoot(1) && !ballsManager.PlayerStillHaveShoot(2))
        {
            scoreManager.UpdateCheckTheDistance();
            scoreManager.CalculateScores();
            state = GameState.RoundFinished;   
        }

        // condition b' : si P2 a encore des balles et si P2 est perdant ou si P1 n'a plus de balles => P2 rejoue
        else if(ballsManager.PlayerStillHaveShoot(2) && (scoreManager.GetTheLooser() == 2 || !ballsManager.PlayerStillHaveShoot(1)))
        {
            ballsManager.CreateBall(2);

            StartCoroutine(AICoroutine());
            state = GameState.WaitForP2;
        }
        
        // condition c' : si P1 a encore des balles et si P1 est perdant ou si P2 n'a plus de balles => au tour de P1
        else if(ballsManager.PlayerStillHaveShoot(1) && (scoreManager.GetTheLooser() == 1 || !ballsManager.PlayerStillHaveShoot(2))){
            state = GameState.WaitForP1;
            ballsManager.CreateBall(1);
        }
    }


    private void UpdateRoundFinished()
    {
        if (scoreManager.scoreP1 == scoreManager.winningScore || scoreManager.scoreP2 == scoreManager.winningScore)
        {
            Debug.Log("Scene des scores démarre");

            currentStateText.SetText("Game done");

            StartCoroutine(LoadScoresScene());
        }
        else if (scoreManager.scoreP1 < scoreManager.winningScore || scoreManager.scoreP2 < scoreManager.winningScore)
        {
            if (!isRoundRestarting)
            {
                // Démarrer la Coroutine pour gérer le délai avant la suppression des balles et la réinitialisation du round
                StartCoroutine(WaitAndRemoveCoroutine());
            }
        }
    }

    private IEnumerator LoadScoresScene()
    {
        yield return new WaitForSeconds(5f);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ScoresScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator WaitAndRemoveCoroutine()
    {
        isRoundRestarting = true;

        float totalDelay = 10f;

        while (totalDelay > 0f)
        {
            announcementText.SetText($"Next round in {Mathf.CeilToInt(totalDelay)}");
            yield return new WaitForSeconds(1f);
            totalDelay -= 1f;
        }
        announcementText.SetText("");

        ballsManager.RemoveAllBalls();
        ballsManager.ResetRound();

        isRoundRestarting = false;

        // Changer l'état après la réinitialisation du round
        state = GameState.WaitForGoal;
    }
    


    private IEnumerator AICoroutine()
    {
        // Attendre un court délai avant que l'IA ne commence à jouer
        yield return new WaitForSeconds(10f);

        // Appeler la fonction AIPlay de l'AIBehaviour
        aIBehaviour.AIPlay();

        // Attendre que l'IA ait terminé de jouer
        while (aIBehaviour.IsAIPlaying())
        {
            yield return null;
        }

        yield return new WaitForSeconds(10f);

    }



    public void UpdateState()
    {
        if(state != previousState){
            whenStateChanged?.Invoke();
        }
        previousState = state;
        switch (state)
        {
            case GameState.WaitForGoal:
            currentStateText.SetText("P1 start the round");
            announcementText.SetText("Throw the goal");
            UpdateWaitForGoal();
            break;

            case GameState.GoalLaunched:
            currentStateText.SetText(" ");
            announcementText.SetText(" ");
            UpdateGoalLaunched();
            break;

            case GameState.WaitForP1:
            currentStateText.SetText("P1's turn");
            announcementText.SetText("Throw the ball");
            UpdateWaitForP1();
            break;

            case GameState.P1HasPlayed:
            currentStateText.SetText(" ");
            announcementText.SetText(" ");
            UpdateP1HasPlayed();
            break;

            case GameState.WaitForP2:
            currentStateText.SetText("P2's turn");
            announcementText.SetText("AI is playing");
            UpdateWaitForP2();
            break;

            case GameState.P2HasPlayed:
            currentStateText.SetText(" ");
            announcementText.SetText(" ");
            UpdateP2HasPlayed();
            break;

            case GameState.RoundFinished:
            currentStateText.SetText("Round finished");
            // announcementText.SetText(" ");
            UpdateRoundFinished();
            break;
        }

        
    }



}
