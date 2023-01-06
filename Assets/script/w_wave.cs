using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class w_wave : MonoBehaviour
{
    public Transform next_circle;
	public DistanceJoint2D Spring;
    Rigidbody2D rigid;

    public float hook_distance;

    //public bool is_line_max;

    private void Start()
    {
        Spring = GetComponent<DistanceJoint2D>();
        rigid = GetComponent<Rigidbody2D>();
        Spring.connectedAnchor = next_circle.transform.position;
    }
    private void FixedUpdate()
    {
        Spring.connectedAnchor = next_circle.transform.position;
        Spring.distance = hook_distance;
        

    }
}
