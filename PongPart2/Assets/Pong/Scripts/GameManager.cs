using System;
using System.Diagnostics;
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
        CheckIfCatchUp();
        Amanager.Play(scoreSFX[UnityEngine.Random.Range(0, scoreSFX.Length)]);
        if (trigger == leftGoalTrigger)
        {
            rightPlayerScore++;
            UnityEngine.Debug.Log($"Right player scored: {rightPlayerScore}");

            if (rightPlayerScore == scoreToWin)
                UnityEngine.Debug.Log("Right player wins!");
            else
                ResetGame(-1f);
        }
        else if (trigger == rightGoalTrigger)
        {
            leftPlayerScore++;
            UnityEngine.Debug.Log($"Left player scored: {leftPlayerScore}");

            if (rightPlayerScore == scoreToWin)
                UnityEngine.Debug.Log("Right player wins!");
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
        int diff = Math.Abs(leftPlayerScore - rightPlayerScore);
        if (diff < 3) return;
        int roll = UnityEngine.Random.Range(1,10);

        // So essentually.. just check if th difference is too great, to "Catch up!"
        if (!modifierActive) {
            // Around 50% 
            if(roll > 5) {
                Amanager.Play("PaddleSizeExpandedVFX");
                modifierActive = true;
                currentModifier = "PaddleSizeExpanded";
                if(leftPlayerScore < rightPlayerScore) ScalePaddle(LeftPaddle, 6);
                else ScalePaddle(RightPaddle, 6);
                
                // Around 20% chance
            } else if (roll <= 2) {
                // Slow the bitch down.. 
                modifierActive = true;
                currentModifier = "SlowedDown";
                Amanager.Play("OhYeah");
                Amanager.Play("Score1");
                Time.timeScale = 0.75f;
            }
            // No need to continue; 
            return;
        }
        // Player is unlucky! 
        if (roll <= 3) resetPowerUp();
        // Player is caught up
        if(diff <= 1 && modifierActive) resetPowerUp();
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

    void ScalePaddle(Paddle scalePaddle, int scalar) {
        // Clamp the bitch.. a twink in Nipple clamps, a dick cage, and thigh highs sounds like a good time to me. 
        // Best comment I ever made right here.. 
        scalar = Math.Clamp(scalar, 1, 8);
        scalePaddle.transform.localScale = new Vector3(scalePaddle.transform.localScale.x, scalePaddle.transform.localScale.y, scalar);
        float travelHeight = -(.5f * scalar) + 5;
        scalePaddle.minTravelHeight = -travelHeight;
        scalePaddle.maxTravelHeight = travelHeight;
    }
}
