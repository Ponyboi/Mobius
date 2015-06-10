using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
	public GameManager gm;
	public Teleport teleport;
	public GameObject portalPlane;
	public GameObject portalPair;
	public Mesh portalMesh;
	public Material portalMatLate;
	public Material portalSlideMat;
	

	//Factory
	private GameObject portalObject;
	public GameObject portalObj {
		get {
			if( portalObject == null) {
				portalObject = new GameObject("Portal");
			}
			return portalObject;
		}
	}

	public static Portal CreatePortal(GameManager gm) { //Teleport teleport, GameObject portalPlane,GameObject portalPair, Material portalMatLate
		var portalObj = new GameObject("Portal");
		var thisObj = portalObj.AddComponent<Portal>();
		//calls Start() on the object and initializes it.
		thisObj.gm = gm;
//		thisObj.portalPair = new GameObject("PortalTemp");
//
//
//		thisObj.teleport = thisObj.gameObject.AddComponent<Teleport>();
//		thisObj.teleport.OtherEnd = thisObj.portalPair.transform;
//		thisObj.teleport.gm = gm;
//
//		thisObj.portalMatLate = gm.portalMatLate;
//		thisObj.portalMesh = gm.portalMesh;
//
//		thisObj.gameObject.AddComponent<BoxCollider>();
//
//		thisObj.portalPlane = new GameObject("PortalPlane");
//		var meshFilter = thisObj.portalPlane.AddComponent<MeshFilter>();
//		var meshRenderer = thisObj.portalPlane.AddComponent<MeshRenderer>();
//		meshFilter.mesh = thisObj.portalMesh;
//		meshRenderer.material = thisObj.portalMatLate;
//		thisObj.portalPlane.transform.parent = thisObj.transform;
		
		return thisObj;
	}

	public void Init(GameObject portalPair) {
		this.portalPair = portalPair;
		this.portalMatLate = gm.portalMatLate;
		this.portalSlideMat = gm.portalSlideMat;
		this.portalMesh = gm.portalMesh;
		this.gameObject.layer = LayerMask.NameToLayer("PortalHelpers1");
		
		this.teleport = this.gameObject.AddComponent<Teleport>();
		this.teleport.OtherEnd = this.portalPair.transform;
		this.teleport.gm = gm;
		
		this.gameObject.AddComponent<BoxCollider>();
		this.GetComponent<BoxCollider>().isTrigger = true;

		var meshFilter = this.gameObject.AddComponent<MeshFilter>();
		var meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
		meshFilter.mesh = this.portalMesh;
		meshRenderer.material = this.portalSlideMat;
		
		this.portalPlane = new GameObject("PortalPlane");
		this.portalPlane.gameObject.layer = LayerMask.NameToLayer("PortalHelpers2");
		meshFilter = this.portalPlane.AddComponent<MeshFilter>();
		meshRenderer = this.portalPlane.AddComponent<MeshRenderer>();
		meshFilter.mesh = this.portalMesh;
		meshRenderer.material = this.portalMatLate;
		this.portalPlane.transform.parent = this.transform;
		this.gameObject.transform.localScale = new Vector3(gm.roomSize * 2, gm.teleportWidth, gm.roomSize * 2);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
