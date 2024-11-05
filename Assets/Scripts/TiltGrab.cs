using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltGrab : TiltInteractable
{
    Rigidbody rb;
    bool isGrabbed = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    override public void Interact(RaycastHit hit)
    {
        isGrabbed = !isGrabbed;
        TiltInteract.instance.Grab(isGrabbed ? this : null);
        rb.useGravity = !isGrabbed;
    }
    public void ProcessGrab(Vector3 target)
    {
        float dist = Vector3.Distance(transform.position, target);
        rb.angularVelocity *= 0.99f;
        if (dist < 1.5f)
        {
            rb.velocity = (target - transform.position) * 10.0f;
        }
        else
            transform.position = target;
    }
}
