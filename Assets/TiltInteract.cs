using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltInteract : MonoBehaviour
{
    public Transform cam;
    public LayerMask layerMask;
    public Transform lineup;
    Renderer rend;
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    void FindSurface()
    {
        transform.localPosition = Vector3.forward;
        transform.localRotation = Quaternion.identity;
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 10, layerMask))
        {
            transform.position = hit.point + hit.normal*0.01f;
            transform.LookAt(hit.point + hit.normal);
            Vector3 eulers = transform.rotation.eulerAngles;
            eulers.z = -cam.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(eulers);
            transform.localScale = Vector3.one * 0.005f * Vector3.Distance(cam.position, transform.position);
            if (hit.transform.GetComponent<TiltInteractable>())
                FindInteractable(hit);
        }
    }

    void FindInteractable(RaycastHit hit)
    {
        lineup.gameObject.SetActive(true);
        lineup.position = transform.position;
        lineup.LookAt(hit.point + hit.normal);
        lineup.Rotate(0, 0, 45);
        lineup.localScale = transform.localScale;
        rend.enabled = true;
        hit.transform.GetComponent<TiltInteractable>().Interact(transform, lineup, hit);
    }

    void Update()
    {
        lineup.gameObject.SetActive(false);
        rend.enabled = false;
        FindSurface();
    }
}
