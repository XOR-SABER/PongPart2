using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeIntensity = 0.1f;
    public float shakeDecay = 0.02f;

    private Vector3 originalPosition;
    private float currentShakeIntensity;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (currentShakeIntensity > 0)
        {
            float offsetX = Mathf.PerlinNoise(Time.time * 10, 0) - 0.5f;
            float offsetY = Mathf.PerlinNoise(0, Time.time * 10) - 0.5f;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0) * currentShakeIntensity;
            currentShakeIntensity -= shakeDecay * Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    public void Shake()
    {
        // Trigger camera shake
        currentShakeIntensity = shakeIntensity;
    }
}