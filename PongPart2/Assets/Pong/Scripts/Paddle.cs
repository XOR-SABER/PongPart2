using System;
using UnityEngine;
using System.Collections;
using TreeEditor;

public class Paddle : MonoBehaviour
{
    public GameObject particleFX; 
    public float maxTravelHeight;
    public float minTravelHeight;
    public float speed;
    public float collisionBallSpeedUp = 1.5f;
    public float FXduration = 5f;
    public float ballMaxSpeed = 150f;
    public string inputAxis;
    private Vector3 startingPos;
    private AudioManager Amanager;
    private new Renderer renderer;
    private bool FXRunning;
    private CameraShake cam; 
    private bool isColorReset; 
    private float startTime;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        Amanager = FindObjectOfType<AudioManager>();
        cam = FindObjectOfType<CameraShake>();
        startingPos = transform.position;
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
        float direction = Input.GetAxis(inputAxis);
        Vector3 newPosition = transform.position + new Vector3(0, 0, direction) * speed * Time.deltaTime;
        newPosition.z = Mathf.Clamp(newPosition.z, minTravelHeight, maxTravelHeight);
        transform.position = newPosition;
    }

    //-----------------------------------------------------------------------------
    void OnCollisionEnter(Collision other)
    {
        var paddleBounds = GetComponent<BoxCollider>().bounds;
        float maxPaddleHeight = paddleBounds.max.z;
        float minPaddleHeight = paddleBounds.min.z;
        // Thuggin it out
        cam.Shake();

        // Particles
        Vector3 particleDirection = other.transform.position - transform.position;
        Quaternion particleRotation = Quaternion.LookRotation(particleDirection);
        Instantiate(particleFX, other.contacts[0].point, particleRotation);


        // Get the percentage height of where it hit the paddle (0 to 1) and then remap to -1 to 1 so we have symmetry
        float pctHeight = (other.transform.position.z - minPaddleHeight) / (maxPaddleHeight - minPaddleHeight);
        float bounceDirection = (pctHeight - 0.5f) / 0.5f;
        // Debug.Log($"pct {pctHeight} + bounceDir {bounceDirection}");

        if (bounceDirection >= 0.3) Amanager.playAtPitch("BallHit", 2.5f);
        else if (bounceDirection <= -0.3) Amanager.playAtPitch("BallHit", 0.5f);
        else Amanager.playAtPitch("BallHit", 1.0f);

        // flip the velocity and rotation direction
        Vector3 currentVelocity = other.relativeVelocity;
        float newSign = -Math.Sign(currentVelocity.x);
        float newRotSign = -newSign;

        // Change the velocity between -60 to 60 degrees based on where it hit the paddle
        float newSpeed = currentVelocity.magnitude * collisionBallSpeedUp;
        // Clamp the speed to fix a bug.. 
        newSpeed = Math.Clamp(newSpeed, 0, ballMaxSpeed);
        Vector3 newVelocity = new Vector3(newSign, 0f, 0f) * newSpeed;
        newVelocity = Quaternion.Euler(0f, newRotSign * 60f * bounceDirection, 0f) * newVelocity;
        // Particles
        other.rigidbody.velocity = newVelocity;
        StartCoroutine(RainbowEffectFX());
        // Debug.DrawRay(other.transform.position, newVelocity, Color.yellow);
    }


    IEnumerator RainbowEffectFX() {
        // Give a rainbow effect for 5 seconds.. 
        // Initial color values
        startTime = Time.time;
        if(FXRunning) yield break;
        

        FXRunning = true;
        Color intiColor = Color.white;
        while (Time.time - startTime < FXduration)
        {
            if(isColorReset) {
                isColorReset = false;
                break;
            }
            float progress = (Time.time - startTime) * 10 / FXduration;
            renderer.material.color = Color.HSVToRGB(progress % 1 ,1,1);
            yield return null;
        }
        FXRunning = false;
        renderer.material.color = intiColor;
    }

    public void Reset() {
        isColorReset = true;
        transform.position = startingPos;
    }
}
