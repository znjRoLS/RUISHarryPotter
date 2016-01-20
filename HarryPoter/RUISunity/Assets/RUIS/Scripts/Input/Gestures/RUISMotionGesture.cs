using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HarryPotter.Magics;
using PointData = RUISPointTracker2.PointData;

public class RUISMotionGesture : RUISGestureRecognizer 
{
	private int cnt = 0;
	
	public int _maxSlope = 3;
	private readonly string configFolderPath = "C:\\Users\\Nordeus\\Downloads\\gestures1.txt"; // todo set it where your txt is;
	private readonly string configFolderPath2 = "C:\\Users\\Nordeus\\Downloads\\gestures2.txt";
	private readonly string configFolderPath3 = "C:\\Users\\Nordeus\\Downloads\\gestures3.txt";
	private readonly string configFolderPath4 = "C:\\Users\\Nordeus\\Downloads\\gestures4.txt";
	private int _minimumLength;
	
	DtwGestureRecognizer dtwOG;
	DtwGestureRecognizer dtwV;
	DtwGestureRecognizer dtwZ;
	DtwGestureRecognizer dtwK;
	
	RUISPointTracker2 ruisInput;
	GameObject head;
	double length;
	
	public float distanceTresholdV;
	public float distanceTresholdOG;
	public float distanceTresholdK;
	public float distanceTresholdZ;

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
		dtwOG = new DtwGestureRecognizer (distanceTresholdOG, 100, 0);
		dtwV = new DtwGestureRecognizer (distanceTresholdV, 100, 0);
		dtwZ = new DtwGestureRecognizer (distanceTresholdZ, 100, 0);
		dtwK = new DtwGestureRecognizer (distanceTresholdK, 100, 0);
		Debug.Log ("Reading gestures...");
		dtwOG.ReadAll (configFolderPath);
		dtwV.ReadAll (configFolderPath2);
		dtwZ.ReadAll (configFolderPath3);
		dtwK.ReadAll (configFolderPath4);
		Debug.Log ("Gestures read!");
		ruisInput = (RUISPointTracker2) GetComponent ("RUISPointTracker2");
		Debug.Log (ruisInput+"RUISINPUT");
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
		string s = "";

		for (int i=0; i<colliders.Length; i++) {
			s+=colliders[i].transform.position.y + " " + colliders[i].transform.position.z+ " ";
		}
		s += '\n';

		File.WriteAllText ("temporaryfile.txt", s);
		//Debug.Log (System.Threading.Thread.CurrentThread.ManagedThreadId);
		//Debug.Log (ruisInput.points [ruisInput.points.Count - 1].position);
		gestureState = false;
		
		if (ruisInput.points.Count > 10) 
		{	
			//NOT SURE IF WE NEED TO check in each iteration. maybe once in some number 
			string recognized = Recognize (ruisInput.points, colliders);
			if (!recognized.Contains ("UNKNOWN")) 
			{	
				Magic magic = MagicFactory.GetMagic(recognized);
				magic.TryActivate();
				gestureState = true;
			}
		}
		
	}
	
	string Recognize(List<PointData> seq1,Transform[] colliders)
	{
		double minimum = double.PositiveInfinity;
		string result = "UNKNOWN";
		double v = dtwV.Recognize (seq1, colliders, true),
		z = dtwZ.Recognize (seq1, colliders),
		o = dtwOG.Recognize (seq1, colliders),
		k = dtwK.Recognize(seq1, colliders);
		
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
		if (minimum > k) 
		{
			minimum = k;
			result = "KShape";
		}
		
		return result;
	}
	
	void GestureEvent()
	{
		Debug.Log ("Recognized motion");
	}
}