using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    public float FXduration = 10f;
    public new Renderer renderer;
    private bool isColorReset = false;
    void Start()
    {
        renderer = GetComponent<Renderer>();  
        
    }

    IEnumerator RainbowEffectFX()
    {
        float startTime = Time.time;
        Color intiColor = renderer.material.color;
        while (Time.time - startTime < FXduration)
        {
            if (isColorReset)
            {
                isColorReset = false;
                break;
            }
            float progress = (Time.time - startTime) * 2 / FXduration;
            Color result = Color.HSVToRGB(progress % 1 ,1,1);

            renderer.material.color = result;
            yield return null;
        }
        renderer.material.color = intiColor; // Reset color after the effect is done
    }

    public void ResetColor()
    {
        isColorReset = true;
    }

    public void startRainbowFX() {
        StartCoroutine(RainbowEffectFX());
    }
}
