using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float FXduration = 5f;
    public float FXspeedDelta = 0.5f;
    private new Renderer renderer;
    private TrailRenderer Trender;
    private bool FXRunning = false;
    private bool isColorReset = false; 
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();  
        Trender = GetComponent<TrailRenderer>();

    }
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("FXStart")) StartCoroutine(RainbowEffectFX());
    }

    IEnumerator RainbowEffectFX() {
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
            Color result = Color.HSVToRGB(progress % 1 ,1,1);

            renderer.material.color = result;
            Trender.startColor = result;
            yield return null;
        }
        FXRunning = false;
        Trender.startColor = intiColor;
        renderer.material.color = intiColor;
    }

    public void Reset() {
        isColorReset = true;
    }
}
