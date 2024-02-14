using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball ball;
    public Paddle LeftPaddle;
    public Paddle RightPaddle;  
    public float startSpeed = 3f;
    public GoalTrigger leftGoalTrigger;
    public GoalTrigger rightGoalTrigger;
    private AudioManager Amanager;

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
        Amanager.Play("Score");
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
        newDirection = Quaternion.Euler(0f, Random.Range(-20f, 20f), 0f) * newDirection;

        var rbody = ball.GetComponent<Rigidbody>();
        rbody.velocity = newDirection;
        rbody.angularVelocity = new Vector3();

        // We are warping the ball to a new location, start the trail over
        ball.GetComponent<TrailRenderer>().Clear();
    }
}
