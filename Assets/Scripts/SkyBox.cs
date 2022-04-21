using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBox : MonoBehaviour
{
    public Material day;
    public Material morning;
    public Material sunset;
    public Material night;
    public Material[] skyboxes;
    int pointer = 0;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        skyboxes = new Material[] { morning, day, sunset, night };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter=counter+1;
        counter = counter % 1500;
        if (counter == 99)
        {
            ChangeSkybox();
        }
        
       
    }

    void ChangeSkybox()
    {
        pointer = (pointer + 1);
        pointer = pointer % skyboxes.Length;
        RenderSettings.skybox = skyboxes[pointer];
       
       
    }
}
