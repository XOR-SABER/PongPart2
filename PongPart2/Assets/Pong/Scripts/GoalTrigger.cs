using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public GameManager gameManager;

    //---------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        gameManager.OnGoalTrigger(this);
    }
}
