using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform pos;

    public float smoothSpeed;

    public Vector3 offset;
    private RaycastHit hit;
    public Vector3 vel = Vector3.zero;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(target);

        Vector3 desiredPos = target.position + pos.position;

        if (!Physics.Linecast(target.position, desiredPos))
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref vel, 0.4f); ;
        }
        else if (!Physics.Linecast(target.position, desiredPos, out hit))
        {
            transform.position = Vector3.SmoothDamp(transform.position, hit.point, ref vel, 0.4f);
        }
         
    }
}
