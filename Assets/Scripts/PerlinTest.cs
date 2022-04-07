using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTest : MonoBehaviour
{

    float tt1, tt2, tt3;
    float inc1 = 0.01f;
    float inc2 = 0.05f;
    float inc3 = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   

        float hp1 = Mathf.PerlinNoise(tt1, 1);
        float hp2 = 0.25f * Mathf.PerlinNoise(tt2, 1);
        float hp3 = 0.125f * Mathf.PerlinNoise(tt3, 1);
        tt1 += inc1;
        tt2 += inc2;
        tt3 += inc3;

        Grapher.Log(hp1+hp2+hp3, "Perlin", Color.yellow);
    }
}
