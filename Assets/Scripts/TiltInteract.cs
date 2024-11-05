using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltInteract : MonoBehaviour
{
    public Transform cam;
    public LayerMask layerMask;
    public Transform lineup;
    Transform currentObject;
    Renderer rend;
    TiltGrab grabbed = null;
    float grabDistance = 0;
    static float lineupAngle = 45;
    float triggerAngle = 5.0f;
    float twistMultiplier = 3f;
    float resetAngle = lineupAngle * 0.6f;
    float startAngle = 0;
    bool hasTriggered = false;
    public static TiltInteract instance;
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        instance = this;
    }

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
            if (hit.transform != currentObject)
            {
                //Enable twister relative to starting roll
                //startAngle = cam.rotation.eulerAngles.z;
                currentObject = hit.transform;
            }
            transform.position = hit.point + hit.normal*0.01f;
            transform.LookAt(hit.point + hit.normal);
            transform.Rotate(0, 0, -twistMultiplier*cam.rotation.eulerAngles.z);
            if (hit.transform.GetComponent<TiltInteractable>())
                FindInteractable(hit);
        }
    }

    void FindInteractable(RaycastHit hit)
    {
        lineup.gameObject.SetActive(true);
        lineup.position = transform.position;
        lineup.LookAt(hit.point + hit.normal);
        lineup.Rotate(0, 0, lineupAngle - startAngle * twistMultiplier);
        rend.enabled = true;
        if(CheckOrientation())
            hit.transform.GetComponent<TiltInteractable>().Interact(hit);
    }

    public void Grab(TiltGrab grabbable)
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
            grabbed.Interact(hit);
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
    }
}

