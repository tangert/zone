using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    GameObject model;
    public float speed = 1.0f;
    public Color startColor;
    public Color endColor;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        // The model is nested underneath two levels of the container object.
        model = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1.0f);
        //model.GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, t);
    }
}
