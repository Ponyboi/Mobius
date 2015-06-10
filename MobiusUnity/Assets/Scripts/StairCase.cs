using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StairType {Straight, Left, Right}
public enum StairSlope {Incline, None, Decline}
//public enum StairLayer {Past, Present, Future}

public class StairCase : MonoBehaviour {
	public GameManager gm;
	public List<GameObject> stairCase;
	public Bezier curve;
	public Vector3[] curvePoints;
	public float rotateVal;
	private Transform step;
	private int stepCount = 20;
	private float stepLerpPosSpeed = 3;
	private float stepRotatedWidth = 0.25f;
	private float xOffset = 0f;
	private float yOffset = 0.3f;
	private float zOffset = 1.25f;

	public Vector3 zeroDir;
	public Vector3 oneDir;

	public RoomLayer roomLayer;
	public StairType stairType = StairType.Straight;
	public StairSlope stairSlope = StairSlope.Incline;
	//public StairLayer staircaseLayer = StairLayer.Present;
	public int stairLayer;
	

	public StairCase(GameManager gm, StairType stairType, StairSlope stairSlope, int stairLayer) {
		this.stairType = stairType;
		this.stairSlope = stairSlope;
		this.stairLayer = stairLayer;

		//gm = GameObject.Find("_GameManager").GetComponent<GameManager>();
		this.gm = gm;
		InitValues();
		StairInit();
		Debug.Log ("stairCase constructor");
	}
	public StairCase(StairType stairType, StairSlope stairSlope, int stairLayer 
	                 ,int stepCount, float xOffset, float yOffset, float zOffset) {
		this.stairType = stairType;
		this.stairSlope = stairSlope;
		this.stairLayer = stairLayer;
		this.stepCount = 20;
		this.xOffset = 0f;
		this.yOffset = 0.3f;
		this.zOffset = 1.25f;

		//gm = GameObject.Find("_GameManager").GetComponent<GameManager>();
		InitValues();
		StairInit();
	}

	//Factory
	private static GameObject staircaseObject;
	public static GameObject staircaseObj {
		get {
			if( staircaseObject == null) {
				staircaseObject = new GameObject("Room");
			}
			return staircaseObject;
		}
	}
	
	public static StairCase CreateStaircase(GameManager gm, RoomLayer roomLayer, StairType stairType, StairSlope stairSlope, int stairLayer) {
		GameObject staircaseObj = new GameObject("Staircase");
		var thisObj = staircaseObj.AddComponent<StairCase>();
		//calls Start() on the object and initializes it.
		thisObj.gm = gm;
		thisObj.roomLayer = roomLayer;
		thisObj.stairType = stairType;
		thisObj.stairSlope = stairSlope;
		thisObj.stairLayer = stairLayer;
		thisObj.InitValues(ref thisObj);
		thisObj.StairInit();
		return thisObj;
	}
	void InitValues(ref StairCase sc) {
		Debug.Log ("hello init");
		GameManager gm = sc.gm;
		sc.step = gm.step;
		sc.stepCount = gm.stepCount;
		sc.stepRotatedWidth = gm.stepRotatedWidth;
		sc.stepLerpPosSpeed = gm.stepLerpPosSpeed;
		sc.xOffset = gm.xOffset;
		sc.yOffset = gm.yOffset;
		sc.zOffset = gm.zOffset;
		//sc.curve = new Bezier();
		sc.curve = sc.gameObject.AddComponent<Bezier>();
		sc.curvePoints = new Vector3[]{Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};
	}
	void InitValues() {
		Debug.Log ("hello init");
		GameManager gm = this.gm;
		this.step = gm.step;
		this.stepCount = gm.stepCount;
		this.xOffset = gm.xOffset;
		this.yOffset = gm.yOffset;
		this.zOffset = gm.zOffset;
		this.curve = new Bezier();
	}

	// Use this for initialization
	void Start () {
		//curve = new Bezier();
		
	}
	
	// Update is called once per frame
	void Update () {
		//UpdateStaircasePosition();
	}

	public void UpdateStaircasePosition() {
		curve.points = new Vector3[]{curvePoints[0], curvePoints[1], curvePoints[2], curvePoints[3]};
		//Debug.Log(curvePoints[0] + " " + Vector3.zero + " " + Vector3.zero + " " + curvePoints[1]);
		float stepSpacing = ((2*gm.roomSize)/(stairCase.Count+4)) - ((gm.zOffset/2)/(2*gm.roomSize));

		//Debug.Log("begin bezier");
		int index = -1;
		float stepZscale = (1 - ((1-stepRotatedWidth) * rotateVal));
		foreach(GameObject step in stairCase) {
			float currentStepIncrement = index/(((float)stairCase.Count-2)) + ((gm.zOffset/2)/(2*gm.roomSize));
			step.transform.localScale = new Vector3(step.transform.localScale.x, step.transform.localScale.y, gm.stepWidth * stepZscale);
			if (index == -1) {
				Vector3 pos = curve.GetPoint(0) + (Vector3.Normalize(curve.GetDirection(0)) * -stepSpacing);
				zeroDir = pos;
				step.transform.position = Vector3.Lerp(step.transform.position, pos, stepLerpPosSpeed);
				step.transform.forward = curve.GetDirection(0);
			} else if (index == stairCase.Count-2) {
				Vector3 pos = curve.GetPoint(1) + (Vector3.Normalize(curve.GetDirection(1)) * stepSpacing);
				oneDir = pos;
				step.transform.position = Vector3.Lerp(step.transform.position, pos, stepLerpPosSpeed);
				step.transform.forward = curve.GetDirection(stairCase.Count);
			} else {
				step.transform.position = Vector3.Lerp(step.transform.position, curve.GetPoint(currentStepIncrement), stepLerpPosSpeed);
				step.transform.forward = curve.GetDirection(currentStepIncrement);
			}
			index++;
		}
	}

	public void SetCurvePoints(Vector3[] points) {

	}

	public void changeLayer(string layerName) {
		Debug.Log ("changeLayer");
		int layer = LayerMask.NameToLayer(layerName);
		if (layer != -1) {
			foreach (GameObject step in stairCase) {
				step.layer = layer;
			}
		}
	}
	void StairInit() {
		stairCase = new List<GameObject>();
		Transform newStep;
		switch(stairType) {
		case StairType.Straight:
		{
			for (int i=(-stepCount); i<=stepCount; i++) {
				newStep = GameObject.Instantiate(step) as Transform;
				newStep.transform.Find("vis").gameObject.layer = stairLayer;
				newStep.hideFlags = HideFlags.HideInHierarchy;
				stairCase.Add(newStep.gameObject);
				newStep.transform.position  = new Vector3(i * xOffset, yOffset, i * zOffset);
				if (roomLayer != RoomLayer.Present)
					newStep.transform.Find("col").GetComponent<BoxCollider>().enabled = false;
			}

			break;
		}
		case StairType.Left:
		{
			for (int i=(-stepCount+1); i<stepCount; i++) {
				newStep = GameObject.Instantiate(step) as Transform;
				newStep.transform.position  = new Vector3(i * xOffset, i * yOffset, i * zOffset);
			}
			break;
		}
		case StairType.Right:
		{
			for (int i=(-stepCount+1); i<stepCount; i++) {
				newStep = GameObject.Instantiate(step) as Transform;
				newStep.transform.position  = new Vector3(i * xOffset, i * yOffset, i * zOffset);
			}
			break;
		}
		default:
		{
			for (int i=(-stepCount+1); i<stepCount; i++) {
				newStep = GameObject.Instantiate(step) as Transform;
				newStep.transform.position  = new Vector3(i * xOffset, i * yOffset, i * zOffset);
			}
			break;
		}
		}
	}

}



//switch(stairType) {
//case StairType.Straight:
//{
//	for (int i=(-stepCount); i<=stepCount; i++) {
//		newStep = GameObject.Instantiate(step) as Transform;
//		newStep.hideFlags = HideFlags.HideInHierarchy;
//		//newStep.gameObject.layer = LayerMask.NameToLayer(staircaseLayer.ToString());
//		newStep.gameObject.layer = stairLayer;
//		stairCase.Add(newStep.gameObject);
//		
//		float slope = 0;
//		if (stairSlope == StairSlope.Incline)
//			slope = i;
//		else if (stairSlope == StairSlope.Decline)
//			slope = (2 * stepCount) - i;
//		
//		newStep.transform.position  = new Vector3(i * xOffset, slope * yOffset, i * zOffset);
//	}
//	break;
//}
//case StairType.Left:
//{
//	for (int i=(-stepCount+1); i<stepCount; i++) {
//		newStep = GameObject.Instantiate(step) as Transform;
//		newStep.transform.position  = new Vector3(i * xOffset, i * yOffset, i * zOffset);
//	}
//	break;
//}
//case StairType.Right:
//{
//	for (int i=(-stepCount+1); i<stepCount; i++) {
//		newStep = GameObject.Instantiate(step) as Transform;
//		newStep.transform.position  = new Vector3(i * xOffset, i * yOffset, i * zOffset);
//	}
//	break;
//}
//default:
//{
//	for (int i=(-stepCount+1); i<stepCount; i++) {
//		newStep = GameObject.Instantiate(step) as Transform;
//		newStep.transform.position  = new Vector3(i * xOffset, i * yOffset, i * zOffset);
//	}
//	break;
//}
//}