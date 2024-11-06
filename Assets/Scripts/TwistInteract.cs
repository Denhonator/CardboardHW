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
        instance = this;
    }

    // Checks if conditions for twist interaction are met
    bool CheckOrientation()
    {
        float angle = Quaternion.Angle(transform.rotation, lineup.rotation);
        if (angle > lineupAngle)
            angle = Mathf.Abs(angle - lineupAngle * 2);
        if (angle < triggerAngle && !hasTriggered)
        {
            hasTriggered = true;
            return true;
        }
        else if (angle > resetAngle)
            hasTriggered = false;
        return false;
    }

    void FindSurface()
    {
        transform.localPosition = Vector3.forward;
        transform.localRotation = Quaternion.identity;
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 10, layerMask))
        {
            float rot = cam.rotation.eulerAngles.z > 180 ? cam.rotation.eulerAngles.z - 360 : cam.rotation.eulerAngles.z;
            if (hit.transform != currentObject)
            {
                //Enable twister relative to starting roll
                if (hit.transform.GetComponent<TwistInteractable>() && hit.transform.GetComponent<TwistInteractable>().relative)
                    startAngle = rot * twistMultiplier;
                else
                    startAngle = 0;
                currentObject = hit.transform;
            }
            transform.position = hit.point + hit.normal*0.01f;
            // Conform to surface normal
            // transform.LookAt(hit.point + hit.normal);
            transform.LookAt(cam);
            transform.Rotate(0, 0, Mathf.Clamp(-twistMultiplier*rot, -lineupAngle-startAngle, lineupAngle-startAngle));
            if (hit.transform.GetComponent<TwistInteractable>())
                FindInteractable(hit);
        }
    }

    void FindInteractable(RaycastHit hit)
    {
        lineup.gameObject.SetActive(true);
        lineup.position = transform.position;
        // Conform to surface normal
        // lineup.LookAt(hit.point + hit.normal);
        lineup.LookAt(cam);
        lineup.Rotate(0, 0, lineupAngle - startAngle);
        rend.enabled = true;
        if(CheckOrientation())
            hit.transform.GetComponent<TwistInteractable>().Interact(transform.parent, hit);
    }

    public void Grab(TwistGrab grabbable)
    {
        grabbed = grabbable;
        if (grabbed)
        {
            grabDistance = Vector3.Distance(grabbed.transform.position, cam.position);
        }
    }

    void Grabbing()
    {
        Vector3 target = cam.position + cam.forward * grabDistance;
        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, grabDistance);
        RaycastHit hit = new RaycastHit();
        foreach(RaycastHit h in hits)
        {
            if (h.transform != grabbed.transform && h.distance < grabDistance)
            {
                target = cam.position + cam.forward * (h.distance - grabbed.GetComponent<Collider>().bounds.size.magnitude);
                hit = h;
            }
        }
        transform.position = target;
        lineup.position = target;
        transform.LookAt(cam);
        lineup.LookAt(cam);
        transform.Rotate(0, 0, -2f * cam.rotation.eulerAngles.z);
        lineup.Rotate(0, 0, lineupAngle);
        grabbed.ProcessGrab(target);
        if(CheckOrientation())
            grabbed.Interact(transform.parent, hit);
    }

    void Update()
    {
        lineup.gameObject.SetActive(grabbed);
        rend.enabled = grabbed;
        if (grabbed)
            Grabbing();
        else
            FindSurface();
        transform.localScale = Vector3.one * 0.01f * Vector3.Distance(cam.position, transform.position);
        lineup.localScale = transform.localScale;
        rend.material.color = hasTriggered ? Color.red : Color.white;
    }
}

