using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessingControl : MonoBehaviour
{
    public GameObject main;
    public PostProcessingProfile profile;
    public GameObject focusSphere;
    public Transform focusTarget;

    // Array of targets
    //public Transform[] focusTargets;

    // Current target
    public float focusTargetID;


    // Adjustable aperture - used in animations within Timeline
    [Range(0.1f, 20f)] public float aperture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (focusSphere != null)
        {
            Transform focus = focusSphere.transform;

            // Get distance from camera and target
            //float dist = Vector3.Distance(transform.position, focusTargets[Mathf.FloorToInt(focusTargetID)].position);
            float dist = Vector3.Distance(transform.position, focus.position);
            // Get reference to the DoF settings
            var dof = profile.depthOfField.settings;

            // Set variables
            dof.focusDistance = dist + focusSphere.transform.localScale.x/2;
            dof.aperture = aperture;
            dof.aperture = 4.0f;

            var vignette = profile.vignette.settings;
            vignette.intensity = focusSphere.GetComponent<InteractObject>().timer.Remap(0, 2.5f, 0.15f, 0.30f);

            // Apply settings
            profile.depthOfField.settings = dof;
            profile.vignette.settings = vignette;
            }
    }
}
