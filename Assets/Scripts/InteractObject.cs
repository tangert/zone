using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    private Color color;
    public GameObject viewer;
    public GameObject main;

    // Keep track
    //private bool isGazing = false;
    // time in seconds that the object is gazed at
    public float timer = 0;
    public float scalingRate;
    float transitionTime = 1.0f;
    public bool isGazing = false;

    public float scaleFactor;
    public Vector3 originalScale;
    public Vector3 originalPosition;
    Vector3 moveAway;

    // 1/60 of a second
    float interval = 0.016666666666667f;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.transform.position;
        originalScale = this.transform.localScale;
        moveAway = viewer.transform.position - this.transform.position;
        scaleFactor = 3;
        scalingRate = 2;
    }

    public void setPosition(Vector3 pos)
    {
        this.originalPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGazing && timer < 2.5)
        {
            timer += interval;
            this.transform.localScale *= 1.02f;
            transform.position -= moveAway * (interval/3);

        }
        else if (!isGazing && timer > 0)
        {
            // fix the timer
            timer -= interval;

            // TODO: Optimize the lerp
            transform.position = Vector3.Lerp(transform.position, originalPosition, scalingRate * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, scalingRate * Time.deltaTime);
        }
    }

    public void SetGazedAt(bool gazedAt)
    {
        if (gazedAt)
        {
            isGazing = true;
            main.GetComponent<Main>().currentGazed = this.gameObject;
            main.GetComponent<Main>().enterGaze();
           
        }
        else
        {
            main.GetComponent<Main>().exitGaze();
            isGazing = false;
        }

    }

    public void OnGazeEnter()
    {
        SetGazedAt(true);
    }

    public void OnGazeExit()
    {
        SetGazedAt(false);
    }

    // Click
    public void OnGazeTrigger()
    {
    }
}
