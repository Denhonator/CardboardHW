using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltInteractable : MonoBehaviour
{
    float triggerAngle = 5.0f;
    float resetAngle = 10.0f;
    bool hasTriggered = false;
    virtual public bool Interact(Transform crosshair, Transform lineup, RaycastHit hit)
    {
        float angle = Quaternion.Angle(crosshair.rotation, lineup.rotation);
        if (angle > 45.0f)
            angle = Mathf.Abs(angle - 90f);
        if (angle < triggerAngle && !hasTriggered)
        {
            hasTriggered = true;
            return true;
        }
        else if (angle > resetAngle)
            hasTriggered = false;
        return false;
    }
}
