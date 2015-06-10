using UnityEngine;
using System.Collections;

public enum RoomLayer {Past, Present, Future}

public class Room : MonoBehaviour {
	GameManager gm;
	public Room futureRoom;
	public Room pastRoom;
	public RoomLayer roomLayer = RoomLayer.Present;
	public StairCase stairCase;
	public StairType stairType = StairType.Straight;
	public StairSlope stairSlope = StairSlope.Incline;

	public Room(GameManager gm, Room pastRoom, Room futureRoom, RoomLayer roomLayer, StairType stairType, StairSlope stairSlope) {
		this.futureRoom = futureRoom;
		this.pastRoom = pastRoom;
		this.roomLayer = roomLayer;
		this.stairType = stairType;
		this.stairSlope = stairSlope;

		roomInit();
	}

	//Factory
	private static GameObject roomObject;
	public static GameObject roomObj {
		get {
			if( roomObject == null) {
				roomObject = new GameObject("Room");
			}
			return roomObject;
		}
	}
	
	public static Room CreateRoom(GameAI gameAI, Room pastRoom, Room futureRoom, RoomLayer roomLayer, StairType stairType, StairSlope stairSlope) {
		GameObject roomObj = new GameObject("Room");
		var thisObj = roomObj.AddComponent<Room>();
		//calls Start() on the object and initializes it.
		thisObj.gm = gameAI.gm;
		thisObj.futureRoom = futureRoom;
		thisObj.pastRoom = pastRoom;
		thisObj.roomLayer = roomLayer;
		thisObj.stairType = stairType;
		thisObj.stairSlope = stairSlope;
		thisObj.roomInit();
		return thisObj;
	}
	//	public Room(Room pastRoom, Room futureRoom, StairCase stairCase, RoomLayer roomLayer) {
//		this.futureRoom = futureRoom;
//		this.pastRoom = pastRoom;
//		this.stairCase = stairCase;
//		this.roomLayer = roomLayer;
//	}

	public void roomInit() {
		int layer = LayerMask.NameToLayer(roomLayer.ToString());
		stairCase = StairCase.CreateStaircase(gm, roomLayer, stairType, stairSlope, layer);
		stairCase.transform.parent = transform;
	}

	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
