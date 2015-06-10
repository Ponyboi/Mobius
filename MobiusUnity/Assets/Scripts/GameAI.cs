using UnityEngine;
using System.Collections;

public class GameAI : MonoBehaviour {
	public GameManager gm;
	public PortalController portalController;
	public Room pastRoom;
	public Room presentRoom;
	public Room futureRoom;

	public StairType stairType = StairType.Straight;
	public StairSlope stairSlope = StairSlope.Incline;
	
	public Lever lever;
	public Transform leverObj;
	
	public GameAI(GameManager gm, StairType stairType, StairSlope stairSlope) {
		this.gm = gm;
		this.stairType = stairType;
		this.stairSlope = stairSlope;
		Init();
	}

	//Factory
	private static GameObject gameAIObject;
	public static GameObject gameAIObj {
		get {
			if( gameAIObject == null) {
				gameAIObject = new GameObject("GameAI");
			}
			return gameAIObject;
		}
	}
	
	public static GameAI CreateGameAI(GameManager gm, StairType stairType, StairSlope stairSlope) {
		var thisObj = gameAIObj.AddComponent<GameAI>();
		//calls Start() on the object and initializes it.
		thisObj.gm = gm;
		thisObj.stairType = stairType;
		thisObj.stairSlope = stairSlope;
		thisObj.Init();

		return thisObj;
	}
	public void Init() {
		Debug.Log("YO");
		leverObj = GameObject.Instantiate(gm.lever) as Transform;
		lever = leverObj.gameObject.GetComponent<Lever>();

		pastRoom = Room.CreateRoom(this, null, null, RoomLayer.Past, stairType, stairSlope);
		futureRoom = Room.CreateRoom(this, null, null, RoomLayer.Future, stairType, stairSlope);
		presentRoom = Room.CreateRoom(this, pastRoom, futureRoom, RoomLayer.Present, stairType, stairSlope);
		portalController = PortalController.CreatePortalController(this);
	}
	
	// Use this for initialization
	void Start () {
		//gm = GameObject.Find("_GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		lever = leverObj.GetComponent<Lever>();
		UpdateRoomState();
	}

	void UpdateRoomState() {
		portalController.rotateTarget = (lever.state) ? 1 : 0;
		portalController.roomTarget = lever.roomLayerTarget;
	}

}
