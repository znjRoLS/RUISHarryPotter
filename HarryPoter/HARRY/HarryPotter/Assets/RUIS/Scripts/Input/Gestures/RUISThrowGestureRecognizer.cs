/*****************************************************************************

Content    :   Implements a basic jump gesture
Authors    :   Mikael Matveinen
Copyright  :   Copyright 2013 Tuukka Takala, Mikael Matveinen. All Rights reserved.
Licensing  :   RUIS is distributed under the LGPL Version 3 license.

******************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Joint = RUISSkeletonManager.Joint;
using JointData = RUISSkeletonManager.JointData;
using HarryPotter.Magics;

[RequireComponent(typeof(RUISPointTracker))]
public class RUISThrowGestureRecognizer : RUISGestureRecognizer
{
	Magic magic;

	public int playerId = 0;
	public int bodyTrackingDeviceID = 0;

	public float requiredUpwardVelocity = 1.0f;
	public float timeBetweenJumps = 1.0f;
	public float distanceTreshold = 0.1f;
	public float requiredConfidence = 1.0f;

	List<Vector3> handPoints = new List<Vector3> ();
	List<Vector3> shdPoints = new List<Vector3> ();
	List<Vector3> elbowPoints = new List<Vector3> ();

	private bool gestureStarted = false;

	private float timeCounter = 0;
	private bool gestureEnabled = true;
	private float scale;
	private GameObject hand;
	private GameObject neck;
	private GameObject head;
	int cnt = 0;

	private bool previousIsTracking = false;
	private bool isTrackingBufferTimeFinished = false;
	
	public void Awake()
	{
		hand = GameObject.FindGameObjectWithTag ("hand");
		neck = GameObject.FindGameObjectWithTag ("neck");
		head = GameObject.FindGameObjectWithTag ("head");
		MagicFactory.AddMagic ("throw", 0);
		magic = MagicFactory.GetMagic ("throw");
		scale = CalculateDistance ();
	}

	private float CalculateDistance()
	{
		return Vector3.Distance (hand.transform.position, neck.transform.position + new Vector3(0f, 0f, -0.08f));
	}

	public void Start()
	{
//		scale = GetWorldScale (elbow);
	}

	public void Update()
	{		
		if (!gestureEnabled)
			return;

		if (!gestureStarted) {
			ClearPoints ();
			cnt = 0;

			float dist = CalculateDistance();

			if (dist < distanceTreshold) {
				gestureStarted = true;
			}
		} else {
			cnt++;

			if (cnt >= 60) {
				gestureStarted = false;
			}

			float distance = CalculateDistance ();
			//Debug.Log (distance + " distnace now!");
			//Debug.Log (scale + " Scale now");
			if (distance >= scale - requiredConfidence && distance <= scale + requiredConfidence) 
			{
				magic.Effect();	
				gestureStarted = false;
			}
		}

	}
	
	public override bool GestureIsTriggered()
	{
		return gestureEnabled;
	}
	
	public override bool GestureWasTriggered()
	{
		return false; // Not implemented
	}
	
	public override float GetGestureProgress()
	{
		return 0;
	}
	
	public override void ResetProgress()
	{	
		timeCounter = 0;
	}
	
	public void UpdatePoints()
	{

	}

	public void ClearPoints()
	{
		handPoints.Clear();
		shdPoints.Clear();
		elbowPoints.Clear();
	}
	
	public override void EnableGesture()
	{
		gestureEnabled = true;
		ResetProgress();
	}

	public Vector3 GetWorldScale(GameObject g)
	{
		Transform transform = g.transform;

		Vector3 worldScale = transform.localScale;
		Transform parent = transform.parent;
		
		while (parent != null)
		{
			worldScale = Vector3.Scale(worldScale,parent.localScale);
			parent = parent.parent;
		}
		
		return worldScale;
	}
	
	public override void DisableGesture()
	{
		gestureEnabled = false;
	}

	
	private void DoAfterJump()
	{
		timeCounter += Time.deltaTime;
		
		if (timeCounter >= timeBetweenJumps)
		{
			ResetProgress();
			return;
		}
	}
	

	public override bool IsBinaryGesture()
	{
		return true;
	}
}
