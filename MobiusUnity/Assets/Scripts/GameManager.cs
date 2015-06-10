using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameAI gameAI;

	//Staircase
	public Transform step;
	public int stepCount = 20;
	public float stepLerpPosSpeed = 3;
	public float stepWidth = 1;
	public float stepRotatedWidth = 0.25f;
	public float roomSize = 10;
	public float xOffset = 0f;
	public float yOffset = 0.3f;
	public float zOffset = 1.25f;
	public float staircaseYOffset;// = (2 * Vector3.up * (stepCount * yOffset - step.transform.localScale.y));

	public StairType stairType = StairType.Straight;
	public StairSlope stairSlope = StairSlope.Incline;

	//Portals
	public Camera cam;
	public float portalSize = 22f;
	public float teleportWidth = 0.2f;
	public Material portalMatLate;
	public Material portalSlideMat;
	public Mesh portalMesh;

	//Misc
	public Transform lever;
	public Transform sky;


	// Use this for initialization
	void Start () {
		staircaseYOffset = (stepCount * yOffset - (step.transform.localScale.y * 0.5f));
		Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Init() {
		gameAI = GameAI.CreateGameAI(this, stairType, stairSlope);
		sky.transform.localScale = new Vector3(roomSize * 2, roomSize * 2, roomSize * 2);
	}

}
