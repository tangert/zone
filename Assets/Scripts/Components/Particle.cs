using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Affects the properties of the particle.
public enum TimeType
{
    second = 0,
    minute = 1,
    hour = 2,
}

public class ParticleBase
{
    public GameObject obj;
    public GameObject model;
    public Vector3 velocity;
    public Vector3 position;
    public Vector3 originalScale;
    public Vector3 gravityDirection = Vector3.up;

    public ParticleBase(GameObject proto, Vector3 pos)
    {

        position = pos;
        velocity = Vector3.one;
        obj = Object.Instantiate(proto);
        originalScale = proto.transform.localScale;

        // Initially randomize rotation slightly
        obj.transform.Rotate(Vector3.forward, Random.Range(0, 360));
        model = obj.transform.GetChild(0).GetChild(0).gameObject;
    }

    public void setPosition(Vector3 pos)
    {
        this.position = pos;
        this.obj.transform.position = position;
    }

    virtual public void reset(DynamicContext dyn)
    {
        generatePosition(dyn);
        velocity = new Vector3(0, -2, 0);
        //obj.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        obj.GetComponent<TrailRenderer>().Clear();
    }

    virtual public void generatePosition(DynamicContext dyn)
    {
        float x = Random.Range(dyn.bounds.min.x, dyn.bounds.max.x);
        float z = Random.Range(dyn.bounds.min.z, dyn.bounds.max.z);

        // Initialize back to random height
        Vector3 pos = new Vector3(x, dyn.bounds.max.y, z);
        this.setPosition(pos);
    }

    virtual public void move(DynamicContext dyn, float gravity)
    {
        Vector3 acceleration = gravityDirection * gravity;
        velocity += acceleration * dyn.timeStep;
        position += velocity * dyn.timeStep;
        this.setPosition(position);
    }
}


// "Time" based particles

public class TimeParticle: ParticleBase {

    public TimeType type;
    public bool isTrapped;
    public float timeMultiplier;
    public float adjustedTimeMultiplier;
    public float timeDifferenceScale = 4;

    public TimeParticle(TimeType type, GameObject proto, Vector3 pos) : base(proto, pos)
    {
        this.type = type;
        switch (type)
        {
            case TimeType.second:
                timeMultiplier = 1;
                break;
            case TimeType.minute:
                timeMultiplier = 1/timeDifferenceScale;
                break;
            case TimeType.hour:
                timeMultiplier = 1 / Mathf.Pow(timeDifferenceScale, 2.0f);
                break;
            default:
                timeMultiplier = 1;
                break;
        }
        adjustedTimeMultiplier = timeMultiplier;
        obj.GetComponent<ColorCycle>().speed = timeMultiplier;
    }

    override public void move(DynamicContext dyn, float gravity)
    {
        Vector3 acceleration = gravityDirection * gravity;
        velocity += acceleration * (dyn.timeStep * adjustedTimeMultiplier);
        position += velocity * (dyn.timeStep * adjustedTimeMultiplier);
        this.setPosition(position);
    }

}