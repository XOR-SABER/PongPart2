using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class RainbowTextEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float FXduration = 5f; // Duration of the rainbow effect
    private bool isColorReset = false;
    private float startTime;
    private GameManager manager;
    private Vector3 initialScale;
    public float RotateFrequency = 0.25f;
    public float RotateAmplitude = 0.1f;
    public float scaleFrequency = 0.25f;
    public float scaleAmplitude = 0.1f;
    public float rotationSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        // So we can ref score..
        manager = FindObjectOfType<GameManager>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        initialScale = transform.localScale;
        StartCoroutine(RainbowEffectFX());
    }
    void Update() {
        float rotationAngle = Mathf.Sin(Time.time * RotateFrequency) * RotateAmplitude;
    
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle * rotationSpeed);
        // Absolute Territory 😉
        float scaleOffset = Mathf.Abs(Mathf.Sin(Time.time * scaleFrequency) * scaleAmplitude);
        Vector3 scale = initialScale + new Vector3(scaleOffset, scaleOffset, scaleOffset);
        transform.localScale = scale;

        int runningScore = Math.Max(manager.leftPlayerScore, manager.rightPlayerScore);
        if(runningScore == 6) {
            FXduration = 7.5f;
            scaleFrequency = RotateFrequency = 0.5f;
            scaleAmplitude = RotateAmplitude = 0.25f;
        } 
        if (runningScore == 8) {
            scaleFrequency = RotateFrequency = 1.0f;
            scaleAmplitude = RotateAmplitude = 0.5f;
        }
        if(runningScore == 10) {
            FXduration = 5f;
            scaleFrequency = RotateFrequency = 2.0f;
            scaleAmplitude = RotateAmplitude = 1.0f;
        }
     }

    IEnumerator RainbowEffectFX()
    {
        startTime = Time.time;
        Color initialColor = textMeshPro.color;
        while (true)
        {
            if (isColorReset)
            {
                isColorReset = false;
                break;
            }
            float progress = (Time.time - startTime) * 2 / FXduration;
            Color result = Color.HSVToRGB(progress % 1, 1, 1);

            textMeshPro.color = result;
            yield return null;
        }
        textMeshPro.color = initialColor;
    }
}
