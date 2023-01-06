using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class w_wave : MonoBehaviour
{
    public Transform next_circle;
	public DistanceJoint2D Spring;

    private void Start()
    {
        Spring = GetComponent<DistanceJoint2D>();
        Spring.connectedAnchor = next_circle.transform.position;
    }
    private void FixedUpdate()
    {
        Spring.connectedAnchor = next_circle.transform.position;
    }
}
