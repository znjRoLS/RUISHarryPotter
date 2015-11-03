using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HarryPotter.Magics;


public class RUISMotionGesture : RUISGestureRecognizer 
{
	private int cnt = 0;

	public int _maxSlope = 3;
	private readonly string configFolderPath = "C:\\Users\\Nordeus\\Downloads\\gestures.txt"; // todo set it where your txt is;

	private int _minimumLength;

	DtwGestureRecognizer dtwOG;
	DtwGestureRecognizer dtwV;
	DtwGestureRecognizer dtwZ;

	RUISPointTracker2 ruisInput;
	GameObject head;
	double length;

	public float distanceTreshold;

	private bool gestureEnabled = false;

	private bool gestureState = false;

	public override bool GestureIsTriggered()
	{
		return gestureState;
	}
	public override bool GestureWasTriggered()
	{
		//TODO ignore - maybe
		return false;
	}
	
	public override float GetGestureProgress()
	{
		return gestureState ? 1 : 0;
	}
	public override void ResetProgress()
	{
		gestureState = false;
	}
	
	public override void EnableGesture()
	{
		gestureEnabled = true;
	}
	public override void DisableGesture()
	{
		gestureEnabled = false;
	}
	
	public override bool IsBinaryGesture()
	{
		//TODO dafuq is this ? Implement or just leave return false wont use anyway

		return false;
	}
	
	void Start()
	{
		dtwOG = new DtwGestureRecognizer (30, 3, 0);
		dtwV = new DtwGestureRecognizer (30, 3, 0);
		Debug.Log ("Reading gestures...");
		dtwOG.ReadAll (configFolderPath);
		dtwV.ReadAll (configFolderPath);
		dtwZ.ReadAll (configFolderPath);
		Debug.Log ("Gestures read!");
		ruisInput = (RUISPointTracker2) GetComponent ("RUISPointTracker2");
	}

	//ON AWAKE
	//load gesture from config file
	//calculate gesutre circumfence and put in length
	//maybe something more
	void Awake()
	{	
		if (pathsColls != null) {
			colliders = new Transform[pathsColls.transform.childCount];
			for (int i = 0; i < colliders.Length; i++) {
				colliders [i] = pathsColls.transform.GetChild (i);
			}
		}

		head = GameObject.FindGameObjectWithTag ("head");
	}


	private Transform[] colliders;
	public GameObject pathsColls;
	void Update () 
	{	
		//Debug.Log (System.Threading.Thread.CurrentThread.ManagedThreadId);
		//Debug.Log (ruisInput.points [ruisInput.points.Count - 1].position);
		gestureState = false;

		if (ruisInput.points.Count > 10 && Mathf.Abs (ruisInput.AverageCord(3)-head.transform.position.z) > distanceTreshold) 
		{	
			//NOT SURE IF WE NEED TO check in each iteration. maybe once in some number 
			string recognized = Recognize (ruisInput.points, colliders);
			Magic magic = MagicFactory.GetMagic("VShape");
			magic.TryActivate();
			if (!recognized.Contains ("UNKNOWN")) 
			{	
				Debug.Log (recognized);
				//Magic magic = MagicFactory.GetMagic(recognized);
				//magic.TryActivate();
				gestureState = true;
			}
		}

	}

	string Recognize(List<PointData> seq1,Transform[] colliders)
	{
		double minimum = double.PositiveInfinity;
		string result = "UNKNOWN";
		double v = dtwV.Recognize (seq1, colliders),
		z = dtwZ.Recognize (seq1, colliders),
		o = dtwOG.Recognize (seq1, colliders);
		if (minimum > v) 
		{
			minimum = v;
			result = "VShape";
		}
		if (minimum > z) 
		{
			minimum = z;
			result = "ZShape";
		}
		if (minimum > o) 
		{
			minimum = o;
			result = "OShape";
		}

		return result;
	}

	void GestureEvent()
	{
		Debug.Log ("Recognized motion");
	}
}