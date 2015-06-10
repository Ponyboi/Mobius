// Copyright (C) Stanislaw Adaszewski, 2013
// http://algoholic.eu

using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {
	public GameManager gm;
	public GameAI gameAI;
	
	public Camera cam;

	//Future
	public Portal portal1;
	private Transform portal1InitialTrans;
	public Camera portal1Cam;

	//Past
	public Portal portal2;
	public Camera portal2Cam;

	Quaternion portal1InitRot;
	Quaternion portal2InitRot;
	Quaternion portal1Rotations;
	Quaternion portal2Rotations;
	
	public Transform sky;

	public float rotateVal = 0;
	public float rotateTarget = 0;
	public float rotateSpeed = 0.7f;
	private float oldRotateTarget = 0;
	public float rotateTime = 0;
	public float rotateStart = 0;
	public RoomLayer roomTarget = RoomLayer.Future;
	public Vector3[] endPoints;

	public PortalController(GameManager gm) {
		this.gm = gm;
		Init();
	}
	//Factory
	private static GameObject portalControllerObject;
	public static GameObject portalControllerObj {
		get {
			if( portalControllerObject == null) {
				portalControllerObject = new GameObject("PortalController");
			}
			return portalControllerObject;
		}
	}
	
	public static PortalController CreatePortalController(GameAI gameAI) {
		var thisObj = portalControllerObj.AddComponent<PortalController>();
		portalControllerObj.AddComponent<MeshFilter>();
		//calls Start() on the object and initializes it.
		thisObj.gm = gameAI.gm;
		thisObj.gameAI = gameAI;
		thisObj.Init(thisObj.gm);
		return thisObj;
	}

	void Init(GameManager gm) {
		//this.gameAI = gameAI;
		this.InitValues();
		this.portal1 = Portal.CreatePortal(gm);//, "Portal1");
		this.portal2 = Portal.CreatePortal(gm);//, "Portal2");
		this.portal1.Init(portal2.gameObject);
		this.portal2.Init(portal1.gameObject);
		portal1.transform.localEulerAngles = new Vector3(270, 0, 0);
		portal2.transform.localEulerAngles = new Vector3(270, 180, 0);

		var portal1CamObj = new GameObject("Portal1Camera");
		var portal2CamObj = new GameObject("Portal2Camera");
		//var portal3CamObj = new GameObject("Portal3Camera");
		//portal3CamObj.AddComponent<Camera>();
		
		this.portal1Cam = portal1CamObj.AddComponent<Camera>();
		this.portal1Cam.depth = -4;
		this.portal1Cam.clearFlags = CameraClearFlags.SolidColor;
		this.portal1Cam.cullingMask = (1 << LayerMask.NameToLayer("Future")) + (1 << LayerMask.NameToLayer("Skybox"));
		this.portal2Cam = portal2CamObj.AddComponent<Camera>();
		this.portal2Cam.depth = -2;
		this.portal2Cam.clearFlags = CameraClearFlags.Nothing;
		this.portal2Cam.cullingMask = (1 << LayerMask.NameToLayer("Past")) + (1 << LayerMask.NameToLayer("Skybox"));

		this.portal2.portalPlane.gameObject.SetActive(false);

		float stepWidth = gm.step.transform.localScale.z;
		portal1.transform.position = new Vector3(portal1.transform.position.x, 
		                                         portal1.transform.position.y,
		                                         gm.roomSize);
		
		portal2.transform.position = new Vector3(portal2.transform.position.x, 
		                                         portal2.transform.position.y,
		                                         -1 * gm.roomSize);
		
	}
	void InitValues() {
		if (gm != null) {
			roomTarget = gameAI.lever.roomLayerTarget;
			cam = gm.cam;
			//			portal1 = gm.portal1;
			//			portal1Cam= gm.portal1Cam;
			//			portal2= gm.portal2;
			//			portal2Cam= gm.portal2Cam;
			sky = gm.sky;
			rotateSpeed = 0.4f;
		}
	}
	void Init() {
		gameAI = gm.gameAI;
		InitValues();
		float stepWidth = gm.step.transform.localScale.z;
		portal1.transform.position = new Vector3(portal1.transform.position.x, 
		                                         portal1.transform.position.y,
		                                         gm.roomSize - gm.zOffset/2);
		
		portal2.transform.position = new Vector3(portal2.transform.position.x, 
		                                         portal2.transform.position.y,
		                                         -1 * (gm.roomSize + gm.zOffset/2));
		
	}
	
	// Use this for initialization
	void Start () {
		this.portal1Cam.GetComponent<Camera>().enabled = false;
		this.portal2Cam.GetComponent<Camera>().enabled = false;
		this.portal1Cam.GetComponent<Camera>().enabled = true;
		this.portal2Cam.GetComponent<Camera>().enabled = true;
		// Camera.main.depthTextureMode = DepthTextureMode.Depth;
		portal1InitRot = portal1.transform.rotation;
		portal2InitRot = portal2.transform.rotation;

	}

	void Update () {
//		portal1.teleport.sign = -portalViewOffset(gameAI.presentRoom, gameAI.pastRoom);
//		portal2.teleport.sign = -portalViewOffset(gameAI.presentRoom, gameAI.futureRoom);
//		endPoints = rotateRoom();
//		gameAI.pastRoom.stairCase.curvePoints = new Vector3[]{endPoints[0], endPoints[1], endPoints[2], endPoints[3]};
//		gameAI.presentRoom.stairCase.curvePoints = new Vector3[]{endPoints[0], endPoints[1], endPoints[2], endPoints[3]};
//		gameAI.futureRoom.stairCase.curvePoints = new Vector3[]{endPoints[0], endPoints[1], endPoints[2], endPoints[3]};
//		gameAI.pastRoom.stairCase.rotateVal = rotateVal;
//		gameAI.presentRoom.stairCase.rotateVal = rotateVal;
//		gameAI.futureRoom.stairCase.rotateVal = rotateVal;
		//UpdatePortals();
		
	}

	public void UpdatePortals() {
		portal1.teleport.sign = -portalViewOffset(gameAI.presentRoom, gameAI.pastRoom);
		portal2.teleport.sign = -portalViewOffset(gameAI.presentRoom, gameAI.futureRoom);
		Vector3[] endPoints = rotateRoom();
		gameAI.pastRoom.stairCase.curvePoints = new Vector3[]{endPoints[0], endPoints[1], endPoints[2], endPoints[3]};
		gameAI.presentRoom.stairCase.curvePoints = new Vector3[]{endPoints[0], endPoints[1], endPoints[2], endPoints[3]};
		gameAI.futureRoom.stairCase.curvePoints = new Vector3[]{endPoints[0], endPoints[1], endPoints[2], endPoints[3]};

		gameAI.pastRoom.stairCase.rotateVal = rotateVal;
		gameAI.presentRoom.stairCase.rotateVal = rotateVal;
		gameAI.futureRoom.stairCase.rotateVal = rotateVal;

		gameAI.futureRoom.stairCase.UpdateStaircasePosition();
		gameAI.pastRoom.stairCase.UpdateStaircasePosition();
		gameAI.presentRoom.stairCase.UpdateStaircasePosition();
		
	}
	
	public Vector3[] rotateRoom() {
		GameObject currentPortal;
		Quaternion currentRot;
		if (roomTarget == RoomLayer.Future) {
			currentPortal = portal1.gameObject;
			currentRot = portal1InitRot;
		} else if (roomTarget == RoomLayer.Past) {
			currentPortal = portal2.gameObject;
			currentRot = portal2InitRot;
		} else {
			currentPortal = portal1.gameObject;
			currentRot = portal1InitRot;
		}

		if (rotateTarget != oldRotateTarget) {
			if((Time.time - rotateTime) * rotateSpeed < 1) {
				rotateTime = Time.time - ((1/rotateSpeed) - (Time.time - rotateTime));
			} else {
				rotateTime = Time.time;
			}
			rotateStart = oldRotateTarget;
			oldRotateTarget = rotateTarget;
		}
		if (Mathf.Abs(rotateVal - rotateTarget) < 0.01f) {
			rotateStart = rotateTarget;
		}
		float t = (Time.time - rotateTime) * rotateSpeed;
		
		rotateVal = Mathf.Lerp(rotateStart, rotateTarget, t);
		float z = Mathf.Cos(rotateVal * 90 *Mathf.Deg2Rad) * (gm.roomSize);
		float x = Mathf.Sin(rotateVal * 90 *Mathf.Deg2Rad) * (gm.roomSize);
		Vector3 futurePortalPos = new Vector3(x, currentPortal.transform.position.y, z);
		currentPortal.transform.position = futurePortalPos;// Vsector3.Lerp(currentPortal.transform.position, futurePortalPos, 0.1f);
		//currentPortal.transform.rotation = Quaternion.FromToRotation(currentPortal.transform.up, Vector3.zero - currentPortal.transform.position);
		portal1Rotations = Quaternion.Euler(0,rotateVal * 90, 0);// * portal1InitRot;
		portal2Rotations = Quaternion.Euler(0,rotateVal * -90, 0);// * portal2InitRot;

		currentPortal.transform.rotation = Quaternion.Euler(0,rotateVal * 90, 0) * currentRot;
		//currentPortal.transform.LookAt(new Vector3(Vector3.zero.x, currentPortal.transform.position.y, Vector3.zero.z), transform.up);
		//currentPortal.transform.Rotate( 90, 0, 0 );

		float controlPointScaler = (4/3)*Mathf.Tan(Mathf.PI/(4)) * (gm.roomSize/2) * 1.08f; //(4/3)*tan(pi/(2n))
		Vector3 futureConPoint = portal1.transform.up *  ((1 + (0.1f * (1-rotateVal))) * controlPointScaler) + portal1.transform.position; //(((0.3f * (1f-rotateVal)) + 1) *
		Vector3 pastConPoint = portal2.transform.up * ((1 + (0.3f * (1-rotateVal))) * controlPointScaler) + portal2.transform.position;

		Vector3 pastPortalPos = new Vector3(0,0,-gm.roomSize);
		return new Vector3[]{futurePortalPos, futureConPoint, pastConPoint, pastPortalPos};
	}

	void FixedUpdate() {
		UpdatePortals();
		//UpdateCameras();
	}

	public void UpdateCameras() {
		Quaternion lookQ =  Quaternion.FromToRotation(-portal1.transform.up, cam.transform.forward);// * portal1Rotations;
		Quaternion planeQ =  Quaternion.FromToRotation(-portal1.transform.up, portal2.transform.up);// * portal1Rotations;
		float mag = Vector3.Magnitude(cam.transform.position - portal1.transform.position);
		float angle = Vector3.Angle(portal1.transform.up, cam.transform.position - portal1.transform.position);
		
		portal1Cam.transform.position = portal2.transform.position + (planeQ * (cam.transform.position - portal1.transform.position))//(cam.transform.position - portal1.transform.position)
			+ (Vector3.up * portalViewOffset(gameAI.presentRoom, gameAI.futureRoom) * gm.staircaseYOffset);
		portal1Cam.transform.rotation = planeQ *cam.transform.rotation;
		//portal1Cam.transform.LookAt(portal1Cam.transform.position + Quaternion.Inverse(planeQ) * (planeQ * (lookQ * portal2.transform.up)), portal2.transform.forward);
		portal1Cam.nearClipPlane = 0.1f;//(portal1Cam.transform.position - portal2.position).magnitude - 0.5f;
		
		
		lookQ = Quaternion.FromToRotation(-portal2.transform.up, cam.transform.forward);
		planeQ =  Quaternion.FromToRotation(-portal2.transform.up, portal1.transform.up);
		portal2Cam.transform.position = portal1.transform.position + (planeQ * (cam.transform.position - portal2.transform.position))
			+ (Vector3.up *  portalViewOffset(gameAI.presentRoom, gameAI.pastRoom) * gm.staircaseYOffset);
		portal2Cam.transform.rotation = planeQ *cam.transform.rotation;
		//portal2Cam.transform.LookAt (portal2Cam.transform.position + lookQ * portal1.transform.up, portal1.transform.forward);
		portal2Cam.nearClipPlane = 0.1f;//(portal2Cam.transform.position - portal1.position).magnitude - 0.5f;
		
		Vector3[] scrPoints = new Vector3[4];
		scrPoints[0] = new Vector3(0, 0, 0.1f);
		scrPoints[1] = new Vector3(1, 0, 0.1f);
		scrPoints[2] = new Vector3(1, 1, 0.1f);
		scrPoints[3] = new Vector3(0, 1, 0.1f);
		
		for (int i = 0; i < scrPoints.Length; i++) {
			scrPoints[i] = transform.worldToLocalMatrix.MultiplyPoint(cam.ViewportToWorldPoint(scrPoints[i]));
		}
		
		int[] tris = new int[6] {0, 1, 2, 2, 3, 0};
		
		MeshFilter mf = GetComponent<MeshFilter>();
		mf.mesh.Clear();
		mf.mesh.vertices = scrPoints;
		mf.mesh.triangles = tris;
		mf.mesh.RecalculateBounds();
	}

	// Update is called once per frame
	void LateUpdate () {
		UpdateCameras();
		
		//UpdatePortals();
		//sky.position = cam.transform.position;
		
//		Quaternion lookQ =  Quaternion.FromToRotation(-portal1.transform.up, cam.transform.forward);// * portal1Rotations;
//		Quaternion planeQ =  Quaternion.FromToRotation(-portal1.transform.up, portal2.transform.up);// * portal1Rotations;
//		float mag = Vector3.Magnitude(cam.transform.position - portal1.transform.position);
//		float angle = Vector3.Angle(portal1.transform.up, cam.transform.position - portal1.transform.position);
//
//		portal1Cam.transform.position = portal2.transform.position + (planeQ * (cam.transform.position - portal1.transform.position))//(cam.transform.position - portal1.transform.position)
//			+ (Vector3.up * portalViewOffset(gameAI.presentRoom, gameAI.futureRoom) * gm.staircaseYOffset);
//		portal1Cam.transform.rotation = planeQ *cam.transform.rotation;
//		//portal1Cam.transform.LookAt(portal1Cam.transform.position + Quaternion.Inverse(planeQ) * (planeQ * (lookQ * portal2.transform.up)), portal2.transform.forward);
//		portal1Cam.nearClipPlane = 0.1f;//(portal1Cam.transform.position - portal2.position).magnitude - 0.5f;
//
//
//		lookQ = Quaternion.FromToRotation(-portal2.transform.up, cam.transform.forward);
//		planeQ =  Quaternion.FromToRotation(-portal2.transform.up, portal1.transform.up);
//		portal2Cam.transform.position = portal1.transform.position + (planeQ * (cam.transform.position - portal2.transform.position))
//			+ (Vector3.up *  portalViewOffset(gameAI.presentRoom, gameAI.pastRoom) * gm.staircaseYOffset);
//		portal2Cam.transform.rotation = planeQ *cam.transform.rotation;
//		//portal2Cam.transform.LookAt (portal2Cam.transform.position + lookQ * portal1.transform.up, portal1.transform.forward);
//		portal2Cam.nearClipPlane = 0.1f;//(portal2Cam.transform.position - portal1.position).magnitude - 0.5f;
//		
//		Vector3[] scrPoints = new Vector3[4];
//		scrPoints[0] = new Vector3(0, 0, 0.1f);
//		scrPoints[1] = new Vector3(1, 0, 0.1f);
//		scrPoints[2] = new Vector3(1, 1, 0.1f);
//		scrPoints[3] = new Vector3(0, 1, 0.1f);
//		
//		for (int i = 0; i < scrPoints.Length; i++) {
//			scrPoints[i] = transform.worldToLocalMatrix.MultiplyPoint(cam.ViewportToWorldPoint(scrPoints[i]));
//		}
//		
//		int[] tris = new int[6] {0, 1, 2, 2, 3, 0};
//		
//		MeshFilter mf = GetComponent<MeshFilter>();
//		mf.mesh.Clear();
//		mf.mesh.vertices = scrPoints;
//		mf.mesh.triangles = tris;
//		mf.mesh.RecalculateBounds();

	}




	int portalViewOffset(Room currentRoom, Room nextRoom) {
		int currentRoomSlope = 0;
		int nextRoomSlope = 0;
		int viewInverter = 1;
		if (nextRoom.roomLayer == RoomLayer.Past)
			viewInverter = -1;

		if (currentRoom.stairSlope == StairSlope.Incline)
			currentRoomSlope = -1 * viewInverter;
		else if (currentRoom.stairSlope == StairSlope.Decline)
			currentRoomSlope = 1 * viewInverter;

		if (nextRoom.stairSlope == StairSlope.Incline)
			nextRoomSlope = -1 * viewInverter;
		else if (currentRoom.stairSlope == StairSlope.Decline)
			nextRoomSlope = 1 * viewInverter;

		return currentRoomSlope + nextRoomSlope;
	}
}
