using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball ball;
    public Paddle LeftPaddle;
    public Paddle RightPaddle;  
    public float startSpeed = 5f;
    public GoalTrigger leftGoalTrigger;
    public GoalTrigger rightGoalTrigger;
    private bool modifierActive = false;
    private string currentModifier = "";
    private AudioManager Amanager;
    private string[] scoreSFX = {"Score1", "Score2", "Score3"};

    int leftPlayerScore;
    int rightPlayerScore;
    Vector3 ballStartPos;

    const int scoreToWin = 11;

    //---------------------------------------------------------------------------
    void Awake()
    {
        Amanager = FindObjectOfType<AudioManager>();
    }


    //---------------------------------------------------------------------------
    void Start()
    {
        ballStartPos = ball.transform.position;
        Rigidbody ballBody = ball.GetComponent<Rigidbody>();
        ballBody.velocity = new Vector3(1f, 0f, 0f) * startSpeed;
    }

    //---------------------------------------------------------------------------
    public void OnGoalTrigger(GoalTrigger trigger)
    {
        // If the ball entered a goal area, increment the score, check for win, and reset the ball
        // Amanager.Play("Score");
        CheckIfCatchUp();
        int randomIndex = UnityEngine.Random.Range(0, scoreSFX.Length);
        Amanager.Play(scoreSFX[randomIndex]);
        if (trigger == leftGoalTrigger)
        {
            rightPlayerScore++;
            Debug.Log($"Right player scored: {rightPlayerScore}");

            if (rightPlayerScore == scoreToWin)
                Debug.Log("Right player wins!");
            else
                ResetGame(-1f);
        }
        else if (trigger == rightGoalTrigger)
        {
            leftPlayerScore++;
            Debug.Log($"Left player scored: {leftPlayerScore}");

            if (rightPlayerScore == scoreToWin)
                Debug.Log("Right player wins!");
            else
                ResetGame(1f);
        }
    }

    //---------------------------------------------------------------------------
    void ResetGame(float directionSign)
    {
        LeftPaddle.Reset();
        RightPaddle.Reset();
        ball.Reset();
        ball.transform.position = ballStartPos;

        // Start the ball within 20 degrees off-center toward direction indicated by directionSign
        directionSign = Mathf.Sign(directionSign);
        Vector3 newDirection = new Vector3(directionSign, 0f, 0f) * startSpeed;
        newDirection = Quaternion.Euler(0f, UnityEngine.Random.Range(-20f, 20f), 0f) * newDirection;

        var rbody = ball.GetComponent<Rigidbody>();
        rbody.velocity = newDirection;
        rbody.angularVelocity = new Vector3();

        // We are warping the ball to a new location, start the trail over
        ball.GetComponent<TrailRenderer>().Clear();
    }
    // The catch up! Mechanic
    void CheckIfCatchUp() {
        Debug.Log(Math.Abs(leftPlayerScore - rightPlayerScore));
        int diff = Math.Abs(leftPlayerScore - rightPlayerScore);
        if (diff > 2 && !modifierActive) {
            // Roll 
            int roll = UnityEngine.Random.Range(1,10);
            // Around 50% 
            if(roll > 5) {
                Amanager.Play("PaddleSizeExpandedVFX");
                modifierActive = true;
                currentModifier = "PaddleSizeExpanded";
                if(leftPlayerScore < rightPlayerScore) {
                    LeftPaddle.transform.localScale = new Vector3(LeftPaddle.transform.localScale.x, LeftPaddle.transform.localScale.y, 6);
                    LeftPaddle.minTravelHeight = -2;
                    LeftPaddle.maxTravelHeight = 2;
                } else {
                    RightPaddle.transform.localScale = new Vector3(RightPaddle.transform.localScale.x, RightPaddle.transform.localScale.y, 6);
                    RightPaddle.minTravelHeight = -2;
                    RightPaddle.maxTravelHeight = 2;
                }
                // Around 20% chance
            } else if (roll <= 2) {
                // Slow the bitch down.. 
                modifierActive = true;
                currentModifier = "SlowedDown";
                Amanager.Play("OhYeah");
                Amanager.Play("Score1");
                Time.timeScale = 0.75f;
            }
        } else if (modifierActive) {
            int roll = UnityEngine.Random.Range(1,10);
            // 40% to reset the power up!
            if(roll > 6) {
                resetPowerUp();
            }
        }

        if(diff <= 1 && modifierActive) {
            resetPowerUp();
            // Perma Speed
        }
    }

    void resetPowerUp() {
        Amanager.Play("PowerUpResetVFX");
        modifierActive = false;
        if(currentModifier == "PaddleSizeExpanded") {
            // Reset the transforms.. 
            LeftPaddle.transform.localScale = new Vector3(LeftPaddle.transform.localScale.x, LeftPaddle.transform.localScale.y, 4);
            RightPaddle.transform.localScale = new Vector3(RightPaddle.transform.localScale.x, RightPaddle.transform.localScale.y, 4);
            LeftPaddle.minTravelHeight = -3;
            LeftPaddle.maxTravelHeight = 3;
            RightPaddle.minTravelHeight = -3;
            RightPaddle.maxTravelHeight = 3;
        }
        if(currentModifier == "BallSizeDecreased") {

        }
        if(currentModifier == "SlowedDown") {
            Time.timeScale = 1.0f;
        }
        // Etc 
        currentModifier = "";
    }
}
