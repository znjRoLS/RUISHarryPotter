//-----------------------------------------------------------------------
// <copyright file="DtwGestureRecognizer.cs" company="Rhemyst and Rymix">
//     Open Source. Do with this as you will. Include this statement or 
//     don't - whatever you like.
//
//     No warranty or support given. No guarantees this will work or meet
//     your needs. Some elements of this project have been tailored to
//     the authors' needs and therefore don't necessarily follow best
//     practice. Subsequent releases of this project will (probably) not
//     be compatible with different versions, so whatever you do, don't
//     overwrite your implementation with any new releases of this
//     project!
//
//     Enjoy working with Kinect!
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using PointData = RUISPointTracker2.PointData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarryPotter.Magics;
using UnityEngine;

using Debug = UnityEngine.Debug;
/// <summary>
/// Dynamic Time Warping nearest neighbour sequence comparison class.
/// Called 'Gesture Recognizer' but really it can work with any vectors
/// </summary>
public class DtwGestureRecognizer
{
    /*
     * By Rhemyst. Dude's a freakin' genius. Also he can do the Rubik's Cube. I mean REALLY do the Rubik's Cube.
     * 
     * http://social.msdn.microsoft.com/Forums/en-US/kinectsdknuiapi/thread/4a428391-82df-445a-a867-557f284bd4b1
     * http://www.youtube.com/watch?v=XsIoN96yF3E
     */

	Queue<double> queueZaRoskaDaMozeDaPrati = new Queue<double>();
	int asd=0;
    /// <summary>
    /// Maximum distance between the last observations of each sequence.
    /// </summary>
    private readonly double _firstThreshold;

    /// <summary>
    /// Minimum length of a gesture before it can be recognised
    /// </summary>
    private readonly double _minimumLength;

    /// <summary>
    /// Maximum DTW distance between an example and a sequence being classified.
    /// </summary>
    private readonly double _globalThreshold;

    /// <summary>
    /// The gesture names. Index matches that of the sequences array in _sequences
    /// </summary>
    private readonly ArrayList _labels;

    /// <summary>
    /// Maximum vertical or horizontal steps in a row.
    /// </summary>
    private readonly int _maxSlope;

    /// <summary>
    /// The recorded gesture sequences
    /// </summary>
	private readonly List<List<RUISPointTracker2.PointData>> _sequences;

    /// <summary>
    /// Initializes a new instance of the DtwGestureRecognizer class
    /// First DTW constructor
    /// </summary>
    /// <param name="dim">Vector size</param>
    /// <param name="threshold">Maximum distance between the last observations of each sequence</param>
    /// <param name="firstThreshold">Minimum threshold</param>
    public DtwGestureRecognizer(double threshold, double firstThreshold, double minLen)
    {
        _sequences = new List<List<RUISPointTracker2.PointData>>();
        _labels = new ArrayList();
        _globalThreshold = threshold;
        _firstThreshold = firstThreshold;
        _maxSlope = int.MaxValue;
        _minimumLength = minLen;
    }

    /// <summary>
    /// Initializes a new instance of the DtwGestureRecognizer class
    /// Second DTW constructor
    /// </summary>
    /// <param name="dim">Vector size</param>
    /// <param name="threshold">Maximum distance between the last observations of each sequence</param>
    /// <param name="firstThreshold">Minimum threshold</param>
    /// <param name="ms">Maximum vertical or horizontal steps in a row</param>
    public DtwGestureRecognizer(double threshold, double firstThreshold, int ms, double minLen)
    {
		_sequences = new List<List<RUISPointTracker2.PointData>>();
        _labels = new ArrayList();
        _globalThreshold = threshold;
        _firstThreshold = firstThreshold;
        _maxSlope = ms;
        _minimumLength = minLen;
    }

	public void ReadAll(string path)
	{
		Debug.Log (path);
		TextReader tr = new StreamReader(path);
		string line;

		while ((line = tr.ReadLine()) != null)    //read till end 
		{
			string[] columns = line.Split (' ');

			string label = columns[0];
			//int cooldown = Convert.ToInt32(columns[1]);
			int cooldown = 1;
			MagicFactory.AddMagic(label, cooldown);

			List<PointData> points = new List<PointData> ();

			for(int i=1; i < columns.Length; i+=2)
			{

				float x = float.Parse(columns[i]);
				float z = float.Parse (columns[i+1]);

				points.Add (new PointData(new UnityEngine.Vector3(x, z, 0), new UnityEngine.Quaternion(), 0.1f, 0.1f, null)); 
			}

			AddOrUpdate(points, label);
		}
	}
    /// <summary>
    /// Add a seqence with a label to the known sequences library.
    /// The gesture MUST start on the first observation of the sequence and end on the last one.
    /// Sequences may have different lengths.
    /// </summary>
    /// <param name="seq">The sequence</param>
    /// <param name="lab">Sequence name</param>
    public void AddOrUpdate(List<PointData> seq1, string lab)
	{

        // First we check whether there is already a recording for this label. If so overwrite it, otherwise add a new entry
        int existingIndex = -1;

        for (int i = 0; i < _labels.Count; i++)
        {
            if ((string)_labels[i] == lab)
            {
                existingIndex = i;
            }
        }

		List<PointData> seq = Preprocess (seq1);

        // If we have a match then remove the entries at the existing index to avoid duplicates. We will add the new entries later anyway
        if (existingIndex >= 0)
        {
            _sequences.RemoveAt(existingIndex);
            _labels.RemoveAt(existingIndex);
        }

        // Add the new entries
        _sequences.Add(seq);
        _labels.Add(lab);
    }

    /// <summary>
    /// Recognize gesture in the given sequence.
    /// It will always assume that the gesture ends on the last observation of that sequence.
    /// If the distance between the last observations of each sequence is too great, or if the overall DTW distance between the two sequence is too great, no gesture will be recognized.
    /// </summary>
    /// <param name="seq">The sequence to recognise</param>
    /// <returns>The recognised gesture name</returns>
	public string Recognize(List<PointData> seq1,Transform[] colliders)
    {
		List<PointData> seq = Preprocess (seq1);
		for (int i=0; i<Mathf.Min(40, seq.Count); i++)
			if (!float.IsInfinity(seq [i].position.x) && !float.IsInfinity(seq [i].position.y))
				colliders [i].position = seq [i].position - new Vector3 (0, 0, seq [i].position.z);
			else
				colliders [i].position = new Vector3 (0, 0, 0);
        double minDist = double.PositiveInfinity;
        string classification = "UNKNOWN";
        for (int i = 0; i < _sequences.Count; i++)
        {
            var example = _sequences[i];
            ////Debug.WriteLine(Dist2((double[]) seq[seq.Count - 1], (double[]) example[example.Count - 1]));
            if (Dist2(seq[seq.Count - 1], example[example.Count - 1]) < _firstThreshold)
            {
				double d = Dtw(example, seq);

                if (d < minDist)
                {
                    minDist = d;
                    classification = (string)_labels[i];
                }
            }
        }

		if (queueZaRoskaDaMozeDaPrati.Count == 40) {
			queueZaRoskaDaMozeDaPrati.Dequeue ();
			queueZaRoskaDaMozeDaPrati.Enqueue (minDist);
			double mini = queueZaRoskaDaMozeDaPrati.Min ();
			//Debug.Log (mini + " " + seq1 [seq1.Count - 1].position);
		}
		else 
		{
			queueZaRoskaDaMozeDaPrati.Enqueue (minDist);
		}

		return (minDist < _globalThreshold ? classification : "UNKNOWN")/*+minDist.ToString()*/;
    }

    public List<PointData> Average(List<PointData> list)
    {
		float midX = (list.Max(x => x.position.x) + list.Min(x => x.position.x)) / 2;
		float midZ = (list.Max(x => x.position.y) + list.Min(x => x.position.y)) / 2;
        for (int i = 0; i < list.Count; i++)
		{
			list [i].position.x -= midX;
			list [i].position.y -= midZ;
		}

		return list;
    }

    public List<PointData> Preprocess(List<PointData> a)
    {
		List<PointData> b = new List<PointData> ();
		//Debug.Log ("a" + a.Count);
		foreach (PointData pointData in a) {
			b.Add (new PointData(pointData.position, pointData.rotation, pointData.deltaTime, pointData.startTime, null));
		}
		//Debug.Log ("b" + b.Count);
		b.RemoveAll (x => float.IsNaN (x.position.x) || float.IsNaN (x.position.y) || float.IsNaN (x.position.z));
		//Debug.Log ("b " + b.Count);
		if (b.Count == 0)
			return a;
		return Normalize(b, -2, 2);
    }
    /// <summary>
    /// Retrieves a text represeantation of the _label and its associated _sequence
    /// For use in dispaying debug information and for saving to file
    /// </summary>
    /// <returns>A string containing all recorded gestures and their names</returns>
    public string RetrieveText()
    {
       /* string retStr = String.Empty;

        if (_sequences != null)
        {
            // Iterate through each gesture
            for (int gestureNum = 0; gestureNum < _sequences.Count; gestureNum++)
            {
                // Echo the label
                retStr += _labels[gestureNum] + "\r\n";

                int frameNum = 0;

                //Iterate through each frame of this gesture
                foreach (double[] frame in ((ArrayList)_sequences[gestureNum]))
                {
                    // Extract each double
                    foreach (double dub in (double[])frame)
                    {
                        retStr += dub + "\r\n";
                    }

                    // Signifies end of this double
                    retStr += "~\r\n";

                    frameNum++;
                }

                // Signifies end of this gesture
                retStr += "----";
                if (gestureNum < _sequences.Count - 1)
                {
                    retStr += "\r\n";
                }
            }
        }

        return retStr;*/

		return "";
    }

    /// <summary>
    /// Compute the min DTW distance between seq2 and all possible endings of seq1.
    /// </summary>
    /// <param name="seq1">The first array of sequences to compare</param>
    /// <param name="seq2">The second array of sequences to compare</param>
    /// <returns>The best match</returns>
    public double Dtw(List<PointData> seq1, List<PointData> seq2)
    {
		if (seq1.Count < 30 || seq2.Count < 30)
			return double.PositiveInfinity;
        // Init
        var seq1R = new List<PointData>(seq1);
        seq1R.Reverse();
        var seq2R = new List<PointData>(seq2);
        seq2R.Reverse();
        var tab = new double[seq1R.Count + 1, seq2R.Count + 1];
        var slopeI = new int[seq1R.Count + 1, seq2R.Count + 1];
        var slopeJ = new int[seq1R.Count + 1, seq2R.Count + 1];

        for (int i = 0; i < seq1R.Count + 1; i++)
        {
            for (int j = 0; j < seq2R.Count + 1; j++)
            {
                tab[i, j] = double.PositiveInfinity;
                slopeI[i, j] = 0;
                slopeJ[i, j] = 0;
            }
        }

        tab[0, 0] = 0;

        // Dynamic computation of the DTW matrix.
        for (int i = 1; i < seq1R.Count + 1; i++)
        {
            for (int j = 1; j < seq2R.Count + 1; j++)
            {
                if (i == j)
                {

                }
                if (tab[i, j - 1] < tab[i - 1, j - 1] && tab[i, j - 1] < tab[i - 1, j] &&
                    slopeI[i, j - 1] < _maxSlope)
                {
                    tab[i, j] = Dist2(seq1R[i - 1], seq2R[j - 1]) + tab[i, j - 1];
                    slopeI[i, j] = slopeJ[i, j - 1] + 1;
                    slopeJ[i, j] = 0;
                }
                else if (tab[i - 1, j] < tab[i - 1, j - 1] && tab[i - 1, j] < tab[i, j - 1] &&
                         slopeJ[i - 1, j] < _maxSlope)
                {
                    tab[i, j] = Dist2(seq1R[i - 1], seq2R[j - 1]) + tab[i - 1, j];
                    slopeI[i, j] = 0;
                    slopeJ[i, j] = slopeJ[i - 1, j] + 1;
                }
                else
                {
                    tab[i, j] = Dist2(seq1R[i - 1], seq2R[j - 1]) + tab[i - 1, j - 1];
                    slopeI[i, j] = 0;
                    slopeJ[i, j] = 0;
                }
            }
        }

        // Find best between seq2 and an ending (postfix) of seq1.
        double bestMatch = double.PositiveInfinity;
        for (int i = 1; i < (seq1R.Count + 1) - _minimumLength; i++)
        {
            if (tab[i, seq2R.Count] < bestMatch)
            {
                bestMatch = tab[i, seq2R.Count];
            }
        }

        return bestMatch;
    }

    /// <summary>
    /// Computes a 2-distance between two observations. (aka Euclidian distance).
    /// </summary>
    /// <param name="a">Point a (double)</param>
    /// <param name="b">Point b (double)</param>
    /// <returns>Euclidian distance between the two points</returns>
    private double Dist2(PointData a, PointData b)
    {
        float d = 0;
		d += (a.position.x - b.position.x) * (a.position.x - b.position.x);
		d += (a.position.y - b.position.y) * (a.position.y - b.position.y);
        return Math.Sqrt(d);
    }
	   
    public List<PointData> Normalize(List<PointData> data, float _minBoundary, float _maxBoundary)
    {
		//Debug.Log ("lll");
		//Debug.Log ("lallala " + data.Count);
		float midX = (data.Max(x => x.position.x) + data.Min(x => x.position.x)) / 2;
		float midY = (data.Max(x => x.position.y) + data.Min(x => x.position.y)) / 2;
		for (int i = 0; i < data.Count; i++)
			data [i].position -= new Vector3(midX, midY, 0);

		//Debug.Log (data [4].position);

		//TODO YOU CHANGED THIS REMOVE MAYBE
		float min = Math.Min(data.Min(x => Math.Abs (x.position.x)), data.Min(x => Math.Abs (x.position.y)));
		float max = Math.Max(data.Max(x => Math.Abs (x.position.x)), data.Max(x => Math.Abs(x.position.y)));

		if ( max <0.06) {
			for (int i = 0; i < data.Count; i++) {
				data [i].position.x = float.PositiveInfinity;
				data [i].position.y = float.PositiveInfinity;
			}
			//Debug.Log ("ZASTO??????");
		} 
		else 
		{
			//Debug.Log ("ZASTO??????");
			//float constFactor = (_maxBoundary - _minBoundary) / (max - min);
			for (int i = 0; i < data.Count; i++) 
				data[i].position*=_maxBoundary/max;
			/*{
				data [i].position.x = (data [i].position.x/max*_maxBoundary + _minBoundary;
				data [i].position.y = (data [i].position.y - min) * constFactor + _minBoundary;
			}*/
		}

		return data;
    }
}