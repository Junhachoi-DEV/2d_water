using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class w_wave : MonoBehaviour
{
	public float Position, Velocity, Height, TargetHeight;

	public void Update()
	{
		const float k = 0.025f; // adjust this value to your liking 
		float x = Height - TargetHeight;
		float acceleration = -k * x;
		Position += Velocity;
		Velocity += acceleration;
	}

}
