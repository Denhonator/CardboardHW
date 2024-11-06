using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistInteract : MonoBehaviour
{
    public Transform cam;
    public LayerMask layerMask;
    public Transform lineup;
    Transform currentObject;
    Renderer rend;
    Renderer rend2;
    TwistGrab grabbed = null;
    float grabDistance = 0;
    static float lineupAngle = 45;
    float triggerAngle = 1.0f;
    float twistMultiplier = 3f;
    float resetAngle = lineupAngle * 0.6f;
    float startAngle = 0;
    bool hasTriggered = false;
    public static TwistInteract instance;
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        rend2 = lineup.GetComponentInChildren<Renderer>();
        instance = this;
    }

    // Checks if conditions for twist interaction are met
    bool CheckOrientation()
    {
        // Angle between the 2 crosshairs. 45 degrees in rest position
        float angle = Quaternion.Angle(transform.rotation, lineup.rotation);
        // Loop the value from 90 degrees to 0; the shape repeats every 90 degrees after all
        if (angle > lineupAngle)
            angle = Mathf.Abs(angle - lineupAngle * 2);

        if (angle < triggerAngle && !hasTriggered)
        {
            hasTriggered = true;
            return true;
        }
        // If interaction was recently triggered, head orientation must be reset a bit to interact again
        else if (angle > resetAngle)
            hasTriggered = false;
        return false;
    }

    // Find what is being looked at
    void FindSurface()
    {
        // Use Raycast to find an object in front of the camera
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 10, layerMask))
        {
            float rot = cam.rotation.eulerAngles.z > 180 ? cam.rotation.eulerAngles.z - 360 : cam.rotation.eulerAngles.z;

            // Check if new object is being looked at
            if (!grabbed && hit.transform != currentObject)
            {
                //Enable twister relative to starting roll
                if (hit.transform.GetComponent<TwistInteractable>() && hit.transform.GetComponent<TwistInteractable>().relative)
                    startAngle = rot * twistMultiplier;
                else
                    startAngle = 0;
                currentObject = hit.transform;
            }

            // Set crosshair rotations
            transform.LookAt(cam);
            transform.Rotate(0, 0, Mathf.Clamp(-twistMultiplier * rot, -lineupAngle - startAngle, lineupAngle - startAngle));
            lineup.LookAt(cam);
            lineup.Rotate(0, 0, lineupAngle - startAngle);

            // Set crosshair visibility depending if something interactive is being looked at
            lineup.gameObject.SetActive(grabbed || hit.transform.GetComponent<TwistInteractable>());
            rend.enabled = lineup.gameObject.activeSelf;

            // Different processing depending if grabbing or just looking
            if (!grabbed && hit.transform.GetComponent<TwistInteractable>() && CheckOrientation())
                hit.transform.GetComponent<TwistInteractable>().Interact(transform.parent, hit);
        }
        else if (!grabbed)
        {
            lineup.gameObject.SetActive(false);
            rend.enabled = false;
        }
        if(grabbed)
            Grabbing();
    }

    // At the moment of grabbing, save reference to grabbed object and distance at which it was grabbed
    public void Grab(TwistGrab grabbable)
    {
        grabbed = grabbable;
        if (grabbed)
        {
            grabDistance = Vector3.Distance(grabbed.transform.position, cam.position);
        }
    }

    // Unique processing for when grabbing an object
    void Grabbing()
    {
        // Set grabbed object target position according to distance it was originally grabbed at
        grabbed.ProcessGrab(cam.position + cam.forward * grabDistance);
        if(CheckOrientation())
            grabbed.Interact(transform.parent);
    }

    void Update()
    {
        FindSurface();

        // Set red color when interaction has happened to notify user they must reset head position before interacting again
        rend.material.color = hasTriggered ? Color.red : Color.white;
        rend2.material.color = Color.green;
    }
}

