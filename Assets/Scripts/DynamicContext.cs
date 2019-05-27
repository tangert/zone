using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicContext
{
    public float timeStep;
    public Vector3 viewerPos;
    public Vector3 viewerUp;
    public Vector3 viewerFront;
    public Vector3 viewerRight;
    public GameObject viewer;
    public Bounds bounds;
}