using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistInteractable : MonoBehaviour
{
    public bool relative = false;
    virtual public void Interact(Transform player, RaycastHit hit = new RaycastHit())
    {
    }
}
