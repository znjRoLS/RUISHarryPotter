using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RUISPointTracker2 : MonoBehaviour {
	public GameObject hand;

	public class PointData
	{
		public Vector3 position;
		public Quaternion rotation;
		public float deltaTime;
		public Vector3 velocity;
		public float startTime;
		
		public PointData(Vector3 position, Quaternion rotation, float deltaTime, float startTime, PointData previous)
		{
			this.position = position;
			this.rotation = rotation;
			this.deltaTime = deltaTime;
			this.startTime = startTime;
			
			if (previous != null)
			{
				velocity = (position - previous.position) / deltaTime;
			}
		}
	}

	private static int cnt = 0;
	private static void Hehe() {
		cnt ++;
		Debug.Log (cnt + " HEHE");
	}

	RUISPointTracker2(){Hehe ();}
	internal List<PointData> points = new List<PointData>();
	PointData previousPoint = null;
	
	public int bufferLength = 70;
	
	float timeSinceLastUpdate = 0;


	private Transform[] colliders;
	public GameObject pathsColls;

	void Awake()
	{
		cachedAverageSpeed = new CachedAverageSpeed(ref points);
		cachedMaxVelocity = new CachedMaxVelocity(ref points);
		cachedAverageVelocity = new CachedAverageVelocity(ref points);
		if (pathsColls != null) {
			colliders = new Transform[pathsColls.transform.childCount];
			for (int i = 0; i < colliders.Length; i++) {
				colliders [i] = pathsColls.transform.GetChild (i);
			}
		}
	}
	
	void Update()
	{
		timeSinceLastUpdate += Time.deltaTime;
		//Debug.Log ("zero   "+hand.transform.TransformPoint(Vector3.zero));
		//Debug.Log ("pos  "+hand.transform.position);
		//Debug.Log ("pp x"+hand.transform.TransformPoint(Vector3.zero).x+" pp y "+hand.transform.TransformPoint(Vector3.zero).y
		//           +" pp z"+hand.transform.TransformPoint(Vector3.zero).z);
		PointData newPoint = new PointData(hand.transform.TransformPoint(Vector3.zero), hand.transform.localRotation, timeSinceLastUpdate, Time.timeSinceLevelLoad, previousPoint);
		
		//remove zero velocities just in case, in order for the speeds not to get polluted by nonexisting data
		//if (newPoint.velocity == Vector3.zero) return;
		
		points.Add(newPoint);
		previousPoint = newPoint;
		
		while (points.Count >= bufferLength)
		{
			points.RemoveAt(0);
		}
		//Debug.Log (points.Count + " br");
		int maks = Math.Min (50, points.Count);
		//Debug.Log ("maks " + maks);
		for (int i=0; i<maks; i++) {
			//Debug.Log ("pp x"+points [i].position.x);
			//Debug.Log ("pp y"+points [i].position.y);
			//Debug.Log ("pp z"+points [i].position.z);
			colliders [i].position = points [i].position;//-new Vector3(0,0,points[i].position.z);
			//colliders[i].position.z = 0;
		}
		//Debug.Log ("THREAD ID" + System.Threading.Thread.CurrentThread.ManagedThreadId);

		//Debug.Log ("brojTacaka"points.Count);

		//if (points.Count > bufferSize) points.RemoveAt(0);
		
		//InvalidateCaches();
		
		//Debug.Log(averageSpeed);
		
		timeSinceLastUpdate = 0;
	}
	
	private void InvalidateCaches()
	{
		cachedAverageSpeed.Invalidate();
		cachedMaxVelocity.Invalidate();
		cachedAverageVelocity.Invalidate();
	}
	
	private CachedAverageSpeed cachedAverageSpeed;
	public float averageSpeed
	{
		get
		{
			return cachedAverageSpeed.GetValue();
		}
	}
	
	private CachedMaxVelocity cachedMaxVelocity;
	public Vector3 maxVelocity
	{
		get
		{
			return cachedMaxVelocity.GetValue();
		}
	}
	
	private CachedAverageVelocity cachedAverageVelocity;
	public Vector3 averageVelocity
	{
		get
		{
			return cachedAverageVelocity.GetValue();
		}
	}
	
	
	
	public class CachedAverageSpeed : CachedValue<float>
	{
		List<PointData> valueList;
		
		public CachedAverageSpeed(ref List<PointData> valueList)
		{
			this.valueList = valueList;
		}
		
		protected override float CalculateValue()
		{
			float speed = 0;
			foreach (PointData data in valueList)
			{
				speed += data.velocity.magnitude;
			}
			return speed / valueList.Count;
		}
	}
	
	public class CachedMaxVelocity : CachedValue<Vector3>
	{
		List<PointData> valueList;
		
		public CachedMaxVelocity(ref List<PointData> valueList)
		{
			this.valueList = valueList;
		}
		
		protected override Vector3 CalculateValue()
		{
			Vector3 maxVelocity = Vector3.zero;
			foreach (PointData data in valueList)
			{
				maxVelocity = maxVelocity.sqrMagnitude > data.velocity.sqrMagnitude ? maxVelocity : data.velocity;
			}
			
			return maxVelocity;
		}
	}
	
	public class CachedAverageVelocity : CachedValue<Vector3>
	{
		List<PointData> valueList;
		
		public CachedAverageVelocity(ref List<PointData> valueList)
		{
			this.valueList = valueList;
		}
		
		protected override Vector3 CalculateValue()
		{
			Vector3 velocity = Vector3.zero;
			foreach (PointData data in valueList)
			{
				velocity += data.velocity;
			}
			return velocity / valueList.Count;
		}
	}

}
