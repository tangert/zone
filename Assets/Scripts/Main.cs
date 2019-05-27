using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Main : MonoBehaviour
{
    // Reference objects around the user
    List<GameObject> refObjects = new List<GameObject>();
    public GameObject currentGazed;

    public GameObject player;
    public GameObject viewer;
    public GameObject refProto;
    public GameObject particleProto;
    ParticleSystem system;

    // Particle system variables
    public float timeStep = 0.015f;
    public float timeStepMultiplier = (1 / 10);
    public float gravity = 9.8f;
    Vector3 referenceAcceleration = Vector3.one;

    public float gridDiameter = 25f;

    // MARK: motion change detection
    private float maxAccumulator = 2000.0f;
    public float accumulator = 0.0f;
    public float decayRate = 0.0f;
    public Vector3 previousRotation;
    public Vector3 currentRotation;
    public Vector3 lastNonZeroDr;
    public Vector3 lastDrAfterExit;
    public Vector3 maxDR = Vector3.zero;
    public Vector3 dR;

    // MARK: Audio
    public AudioSource highTrack;
    public AudioSource midTrack;
    public AudioSource lowTrack;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(highTrack.clip.length);

        // REFERENCE OBJECTS
        // Refernece sphere to get mesh
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Size of the reference sphere
        sphere.transform.localScale *= gridDiameter;
        sphere.transform.position = new Vector3(0, 0, 0);
        sphere.SetActive(false);

        Vector3[] originalVertices = sphere.GetComponent<MeshFilter>().mesh.vertices;
        List<Vector3> uniqueVertices = new List<Vector3>(originalVertices).Distinct().ToList();
        for (int i = 0; i < uniqueVertices.Count; ++i)
        {
            GameObject r = Object.Instantiate(refProto);
            r.transform.position = uniqueVertices[i] * gridDiameter;
            r.transform.localScale *= 1.5f;
            refObjects.Add(r);
        }

        // PARTICLE SYSTEM
        float width = 75.0f;
        system = new ParticleSystem(width);
        system.populateTimeType(1500, TimeType.second, particleProto);
        system.populateTimeType(500, TimeType.minute, particleProto);
        system.populateTimeType(100, TimeType.hour, particleProto);

        particleProto.SetActive(false);
        refProto.SetActive(false);

        //player.transform.position = new Vector3(0, -gridDiameter/2, 0);

    }

    Vector3 calculateAudioVolume(float headTilt)
    {
        float highVol = 1.0f;
        float midVol = 1.0f;
        float lowVol = 1.0f; 

        if (headTilt > 0)
        {
            midVol = Mathf.Lerp(1.0f, 0.1f, headTilt / 0.67f);
            highVol = Mathf.Lerp(1.0f, 0.1f, headTilt / 0.67f);
        } else
        {
            midVol = Mathf.Lerp(1.0f, 0.1f, headTilt / -0.67f);
            lowVol = Mathf.Lerp(1.0f, 0.1f, headTilt / -0.67f);
        }
        return new Vector3(highVol,midVol,lowVol);
    }

    public void enterGaze()
    {
        viewer.GetComponent<PostProcessingControl>().focusSphere = currentGazed;
    }

    public void exitGaze()
    {
        //viewer.GetComponent<PostProcessingControl>().focusSphere = null;
        //Debug.Log("EXIT");
        lastDrAfterExit = lastNonZeroDr;
    }

    void Update()
    {
        // Update the viewers position according to the length of the song
        system.context.timeStep = timeStep;
        system.context.viewer = viewer;
        system.context.viewerRight = viewer.transform.right;
        currentRotation = viewer.transform.rotation.eulerAngles;

        dR = currentRotation - previousRotation;

        if (accumulator < maxAccumulator)
        {

            // change to 5 for mobile
            accumulator += dR.magnitude/3;
        }

        if (accumulator > 0)
        {
            decayRate = EasingFunction.EaseInQuad(1.0f, 30f, (accumulator/maxAccumulator));
            accumulator -= decayRate;
            accumulator = Mathf.Clamp(accumulator, 0, 2000);
        }

        // Reset accumulator if it goes under
        if (accumulator < 0) accumulator = 0;

        if (dR.magnitude > 0) 
        {
            lastNonZeroDr = dR;
        }

        if(dR.magnitude > maxDR.magnitude)
        {
            maxDR = dR;
        }

        float normalizedAccumulator = accumulator.Remap(0, maxAccumulator, 0, 1);
        previousRotation = viewer.transform.rotation.eulerAngles;

        foreach (TimeParticle p in system.particles)
        {
            float percentToBorder;
            Color transitionColor;
            switch (p.type)
            {
                case TimeType.second:
                    percentToBorder = Mathf.Clamp(normalizedAccumulator / (1 / (Mathf.Pow(p.timeDifferenceScale, 2.0f))), 0, 1.0f);
                    break;
                case TimeType.minute:
                    percentToBorder = Mathf.Clamp(normalizedAccumulator / (1 / p.timeDifferenceScale), 0, 1.0f);
                    break;
                case TimeType.hour:
                    percentToBorder = Mathf.Clamp((normalizedAccumulator / 1.0f), 0, 1.0f);
                    break;
                default:
                    percentToBorder = 1.0f;
                    transitionColor = Color.white;
                    break;
            }


            if(currentGazed != null)
            {
                if (currentGazed.GetComponent<InteractObject>().isGazing)
                {
                    SphereCollider sphere = currentGazed.GetComponent<SphereCollider>();
                    float radius = sphere.radius * currentGazed.transform.localScale.magnitude * 0.75f;

                    float dist = Vector3.Distance(p.obj.transform.position, currentGazed.transform.position);
                    if (dist < radius)
                    {
                        float velocityFactor = dist.Remap(0, radius, 0.01f, 0.1f);
                        p.adjustedTimeMultiplier = p.timeMultiplier * velocityFactor;
                    }
                }

            }

            Color transColor = Color.Lerp(Color.white, Color.cyan, percentToBorder);
            p.model.transform.localScale = new Vector3(1.0f - percentToBorder,1.0f - percentToBorder,1.0f - percentToBorder);
            p.model.GetComponent<Renderer>().material.color = transColor;
            p.obj.GetComponent<Renderer>().material.color = transColor;
            p.obj.GetComponent<TrailRenderer>().startColor = transColor;
            p.obj.GetComponent<TrailRenderer>().endColor = transColor;
            p.obj.GetComponent<TrailRenderer>().time = 1.0f - percentToBorder;

        }

        float headRoll = Mathf.Abs(viewer.transform.rotation.z);
        float particleAdjustedMultiplier = headRoll.Remap(0.0f, 0.67f, 0.5f, 3.0f);


        // Audio adjustments
        if (currentGazed != null)
        {
            float pitchFactor = currentGazed.GetComponent<InteractObject>().timer.Remap(0, 2.5f, 1.0f, 0.5f);
            highTrack.pitch = pitchFactor;
            lowTrack.pitch = pitchFactor;
            midTrack.pitch = pitchFactor;
        }

        Vector3 volumes = calculateAudioVolume(viewer.transform.rotation.x);
        highTrack.volume = volumes.x;
        midTrack.volume = volumes.y;
        lowTrack.volume = volumes.z;

        system.context.timeStep = 0.015f * particleAdjustedMultiplier;
        system.move(gravity);

    }

}

