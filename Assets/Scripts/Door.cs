using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TiltInteractable
{
    List<Quaternion> targetRots = new List<Quaternion>();
    float dir = 0;
    float timer = 0;
    private void Start()
    {
        targetRots.Add(transform.parent.rotation);
        targetRots.Add(Quaternion.AngleAxis(90, Vector3.up));
    }
    override public void Interact(RaycastHit hit)
    {
        dir = dir <= 0 ? 1.0f : -1.0f;
    }

    private void Update()
    {
        timer = Mathf.Clamp(timer + dir * Time.deltaTime, 0.0f, 1.0f);
        transform.parent.rotation = Quaternion.Lerp(targetRots[0], targetRots[1], timer);
    }
}
