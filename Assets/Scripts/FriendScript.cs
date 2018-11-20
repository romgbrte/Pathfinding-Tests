/*
	Many of the commented lines of code here are for reporting purposes, or left over from constructing 
	the test coordinate sets - though some are also from trial-and-error testing of various components.
*/
using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using ASO;
using FloorGridArray;

public class FriendScript : MonoBehaviour {

	#region Variables
	public const float movementHeight	= 2.0f,
					   vhMoveCost		= 10.0f,
					   dMoveCost		= 14.0f,
					   slothSpeed		= 0.2f, // for testing, 0.2f
					   walkSpeed		= 1.5f, // normal, 1.5f
					   runSpeed  		= 2.25f, // sometimes breaks @3.0f
					   minCoord 		= 1.0f,
					   maxCoord 		= 19.0f,
					   zero				= 0.0f,
					   moveAccuracy		= 0.02f, // Margin of movement accuracy
					   infinity			= 99999.0f;
	const char DELIM = ',';
	static string[] fileName = new string[18] {"AStar_NoWalls_LInf", "GBFS_NoWalls_LInf", "Dijkstra_NoWalls_LInf",
											  "AStar_AsstdWalls_LInf", "GBFS_AsstdWalls_LInf", "Dijkstra_AsstdWalls_LInf",
											  "AStar_UTrap_LInf", "GBFS_UTrap_LInf", "Dijkstra_UTrap_LInf",
											  "AStar_NoWalls_L1", "GBFS_NoWalls_L1", "Dijkstra_NoWalls_L1",
											  "AStar_AsstdWalls_L1", "GBFS_AsstdWalls_L1", "Dijkstra_AsstdWalls_L1",
											  "AStar_UTrap_L1", "GBFS_UTrap_L1", "Dijkstra_UTrap_L1"
											 };
	public static List<int> pathDataList = new List<int>();
	public static List<int> searchedDataList = new List<int>();
	public static List<int> timeDataList = new List<int>();
	//public List<Vector3> coordinateList = new List<Vector3>();
	public int coordCount = 0;
	
	//static List<Vector3> coordinates = new List<Vector3> {};
	
	public static float userInputX = 13.0f,
				 		userInputZ = 18.0f;
	
	public GameObject pathMarker;
	public GameObject goalX;
	public GameObject searchedNode;
	GameObject[] pfobjArray;
	GameObject[] gxobjArray;
	GameObject[] snobjArray;
	
	public bool isMoving = false,
				readyToGo = false;
	public static bool begin = false;
	public int	count = 0;
	public static int totalConsidered = 0;
	
	public static string algorithmLabel;
	public string[] algorithms = new string[3] {"A*", "GBFS", "Dijkstra"};
	public static int algoIndex = 0;
	
	public static string wallLabel;
	public string[] wallSets = new string[3] {"No Walls", "Assorted Walls", "U-Trap"};
	public static int wallIndex = 1;
	public GameObject wallObject1;
	public GameObject wallObject2;
	
	public static string degreeLabel;
	public string[] degrees = new string[2] {"L[Infinity]", "L[1]"};
	public static int degreeIndex = 0;
	
	public static Vector3 defaultStart = new Vector3(5.0f, movementHeight, 1.0f);
	public static Vector3 empty	= new Vector3(zero, movementHeight, zero);
	public static GameObject locationObject;
	public static Vector3 rayCastDirection = new Vector3 (zero, -1, zero);
	public const int rayCastDistance = 20;
	public static Vector3 HUDgoal = new Vector3();
	public static Vector3 HUDstart = new Vector3();
	public static int pathCount = 0;
	public static int goalCount = 0;
	
	public List<AStarObject> finalPath = new List<AStarObject>(); // Holds the list of coords from start to goal
	public List<AStarObject> open;
	public List<AStarObject> closed;
	public List<AStarObject> neighbors;
	
	public List<Vector3> pathNodesList = new List<Vector3>();
	public List<Vector3> closedList	= new List<Vector3>();
	public List<Vector3> openList = new List<Vector3>();
	
	//public List<float> openFloats = new List<float>();
	//public List<float> pathFloats	= new List<float>();
	//public List<float> closedFloats = new List<float>();
	
	public AStarObject start;
	public AStarObject currentGoal;
	public AStarObject endGoal;
	public AStarObject current; // space being evaluated at any given point
	
	Stopwatch stopwatch = new Stopwatch();
	public static int timeElapsed;
	#endregion
	
	//AsstdWalls, UTrap, NoWalls
	#region Coordinates
	static List<Vector3> AsstdWalls = new List<Vector3> {
		new Vector3(13.0f, movementHeight, 18.0f),
		new Vector3(8.0f, movementHeight, 4.0f),
		new Vector3(2.0f, movementHeight, 7.0f),
		new Vector3(14.0f, movementHeight, 7.0f),
		new Vector3(3.0f, movementHeight, 18.0f),
		new Vector3(16.0f, movementHeight, 15.0f),
		new Vector3(2.0f, movementHeight, 16.0f),
		new Vector3(14.0f, movementHeight, 12.0f),
		new Vector3(8.0f, movementHeight, 13.0f),
		new Vector3(3.0f, movementHeight, 13.0f),
		new Vector3(11.0f, movementHeight, 12.0f),
		new Vector3(9.0f, movementHeight, 7.0f),
		new Vector3(6.0f, movementHeight, 14.0f),
		new Vector3(17.0f, movementHeight, 15.0f),
		new Vector3(5.0f, movementHeight, 2.0f),
		new Vector3(11.0f, movementHeight, 10.0f),
		new Vector3(17.0f, movementHeight, 2.0f),
		new Vector3(6.0f, movementHeight, 18.0f),
		new Vector3(3.0f, movementHeight, 17.0f),
		new Vector3(16.0f, movementHeight, 1.0f),
		new Vector3(7.0f, movementHeight, 15.0f),
		new Vector3(3.0f, movementHeight, 14.0f),
		new Vector3(15.0f, movementHeight, 14.0f),
		new Vector3(6.0f, movementHeight, 8.0f),
		new Vector3(9.0f, movementHeight, 17.0f),
		new Vector3(9.0f, movementHeight, 5.0f),
		new Vector3(10.0f, movementHeight, 17.0f),
		new Vector3(8.0f, movementHeight, 17.0f),
		new Vector3(18.0f, movementHeight, 5.0f),
		new Vector3(10.0f, movementHeight, 18.0f),
		new Vector3(9.0f, movementHeight, 13.0f),
		new Vector3(6.0f, movementHeight, 8.0f),
		new Vector3(5.0f, movementHeight, 2.0f),
		new Vector3(11.0f, movementHeight, 8.0f),
		new Vector3(11.0f, movementHeight, 1.0f),
		new Vector3(13.0f, movementHeight, 8.0f),
		new Vector3(13.0f, movementHeight, 14.0f),
		new Vector3(7.0f, movementHeight, 1.0f),
		new Vector3(10.0f, movementHeight, 5.0f),
		new Vector3(3.0f, movementHeight, 17.0f),
		new Vector3(9.0f, movementHeight, 17.0f),
		new Vector3(8.0f, movementHeight, 15.0f),
		new Vector3(4.0f, movementHeight, 2.0f),
		new Vector3(6.0f, movementHeight, 4.0f),
		new Vector3(13.0f, movementHeight, 18.0f),
		new Vector3(17.0f, movementHeight, 3.0f),
		new Vector3(7.0f, movementHeight, 1.0f),
		new Vector3(3.0f, movementHeight, 2.0f),
		new Vector3(18.0f, movementHeight, 6.0f),
		new Vector3(13.0f, movementHeight, 14.0f),
		new Vector3(7.0f, movementHeight, 7.0f),
		new Vector3(2.0f, movementHeight, 3.0f),
		new Vector3(1.0f, movementHeight, 2.0f),
		new Vector3(2.0f, movementHeight, 10.0f),
		new Vector3(12.0f, movementHeight, 11.0f),
		new Vector3(3.0f, movementHeight, 15.0f),
		new Vector3(16.0f, movementHeight, 8.0f),
		new Vector3(2.0f, movementHeight, 1.0f),
		new Vector3(10.0f, movementHeight, 11.0f),
		new Vector3(1.0f, movementHeight, 7.0f),
		new Vector3(3.0f, movementHeight, 6.0f),
		new Vector3(12.0f, movementHeight, 1.0f),
		new Vector3(1.0f, movementHeight, 14.0f),
		new Vector3(1.0f, movementHeight, 16.0f),
		new Vector3(7.0f, movementHeight, 2.0f),
		new Vector3(18.0f, movementHeight, 14.0f),
		new Vector3(14.0f, movementHeight, 8.0f),
		new Vector3(1.0f, movementHeight, 3.0f),
		new Vector3(12.0f, movementHeight, 15.0f),
		new Vector3(10.0f, movementHeight, 14.0f),
		new Vector3(6.0f, movementHeight, 4.0f),
		new Vector3(10.0f, movementHeight, 12.0f),
		new Vector3(5.0f, movementHeight, 16.0f),
		new Vector3(9.0f, movementHeight, 5.0f),
		new Vector3(12.0f, movementHeight, 2.0f),
		new Vector3(6.0f, movementHeight, 2.0f),
		new Vector3(12.0f, movementHeight, 3.0f),
		new Vector3(9.0f, movementHeight, 8.0f),
		new Vector3(6.0f, movementHeight, 4.0f),
		new Vector3(18.0f, movementHeight, 2.0f),
		new Vector3(18.0f, movementHeight, 16.0f),
		new Vector3(12.0f, movementHeight, 17.0f),
		new Vector3(10.0f, movementHeight, 18.0f),
		new Vector3(17.0f, movementHeight, 7.0f),
		new Vector3(6.0f, movementHeight, 16.0f),
		new Vector3(5.0f, movementHeight, 18.0f),
		new Vector3(11.0f, movementHeight, 5.0f),
		new Vector3(13.0f, movementHeight, 12.0f),
		new Vector3(1.0f, movementHeight, 9.0f),
		new Vector3(7.0f, movementHeight, 7.0f),
		new Vector3(12.0f, movementHeight, 11.0f),
		new Vector3(1.0f, movementHeight, 13.0f),
		new Vector3(13.0f, movementHeight, 10.0f),
		new Vector3(18.0f, movementHeight, 9.0f),
		new Vector3(6.0f, movementHeight, 15.0f),
		new Vector3(18.0f, movementHeight, 15.0f),
		new Vector3(15.0f, movementHeight, 14.0f),
		new Vector3(18.0f, movementHeight, 17.0f),
		new Vector3(18.0f, movementHeight, 14.0f),
		new Vector3(1.0f, movementHeight, 6.0f)
	};
	
	static List<Vector3> UTrap = new List<Vector3> {
		new Vector3(13.0f, movementHeight, 18.0f),
		new Vector3(11.0f, movementHeight, 2.0f),
		new Vector3(8.0f, movementHeight, 6.0f),
		new Vector3(2.0f, movementHeight, 16.0f),
		new Vector3(12.0f, movementHeight, 3.0f),
		new Vector3(11.0f, movementHeight, 5.0f),
		new Vector3(4.0f, movementHeight, 18.0f),
		new Vector3(7.0f, movementHeight, 13.0f),
		new Vector3(16.0f, movementHeight, 17.0f),
		new Vector3(5.0f, movementHeight, 14.0f),
		new Vector3(16.0f, movementHeight, 2.0f),
		new Vector3(5.0f, movementHeight, 4.0f),
		new Vector3(11.0f, movementHeight, 12.0f),
		new Vector3(2.0f, movementHeight, 1.0f),
		new Vector3(12.0f, movementHeight, 4.0f),
		new Vector3(1.0f, movementHeight, 12.0f),
		new Vector3(13.0f, movementHeight, 14.0f),
		new Vector3(15.0f, movementHeight, 18.0f),
		new Vector3(1.0f, movementHeight, 4.0f),
		new Vector3(5.0f, movementHeight, 9.0f),
		new Vector3(17.0f, movementHeight, 3.0f),
		new Vector3(10.0f, movementHeight, 5.0f),
		new Vector3(7.0f, movementHeight, 9.0f),
		new Vector3(2.0f, movementHeight, 3.0f),
		new Vector3(8.0f, movementHeight, 17.0f),
		new Vector3(15.0f, movementHeight, 17.0f),
		new Vector3(12.0f, movementHeight, 8.0f),
		new Vector3(17.0f, movementHeight, 14.0f),
		new Vector3(3.0f, movementHeight, 13.0f),
		new Vector3(10.0f, movementHeight, 13.0f),
		new Vector3(17.0f, movementHeight, 8.0f),
		new Vector3(13.0f, movementHeight, 7.0f),
		new Vector3(1.0f, movementHeight, 3.0f),
		new Vector3(18.0f, movementHeight, 10.0f),
		new Vector3(18.0f, movementHeight, 6.0f),
		new Vector3(9.0f, movementHeight, 9.0f),
		new Vector3(8.0f, movementHeight, 8.0f),
		new Vector3(5.0f, movementHeight, 13.0f),
		new Vector3(6.0f, movementHeight, 1.0f),
		new Vector3(8.0f, movementHeight, 6.0f),
		new Vector3(7.0f, movementHeight, 7.0f),
		new Vector3(1.0f, movementHeight, 15.0f),
		new Vector3(7.0f, movementHeight, 8.0f),
		new Vector3(1.0f, movementHeight, 17.0f),
		new Vector3(7.0f, movementHeight, 9.0f),
		new Vector3(12.0f, movementHeight, 1.0f),
		new Vector3(5.0f, movementHeight, 11.0f),
		new Vector3(15.0f, movementHeight, 3.0f),
		new Vector3(5.0f, movementHeight, 7.0f),
		new Vector3(7.0f, movementHeight, 5.0f),
		new Vector3(8.0f, movementHeight, 14.0f),
		new Vector3(1.0f, movementHeight, 16.0f),
		new Vector3(8.0f, movementHeight, 1.0f),
		new Vector3(10.0f, movementHeight, 9.0f),
		new Vector3(7.0f, movementHeight, 13.0f),
		new Vector3(1.0f, movementHeight, 2.0f),
		new Vector3(3.0f, movementHeight, 12.0f),
		new Vector3(9.0f, movementHeight, 15.0f),
		new Vector3(3.0f, movementHeight, 3.0f),
		new Vector3(8.0f, movementHeight, 6.0f),
		new Vector3(18.0f, movementHeight, 10.0f),
		new Vector3(7.0f, movementHeight, 11.0f),
		new Vector3(6.0f, movementHeight, 10.0f),
		new Vector3(17.0f, movementHeight, 14.0f),
		new Vector3(14.0f, movementHeight, 13.0f),
		new Vector3(15.0f, movementHeight, 6.0f),
		new Vector3(16.0f, movementHeight, 18.0f),
		new Vector3(7.0f, movementHeight, 7.0f),
		new Vector3(7.0f, movementHeight, 10.0f),
		new Vector3(11.0f, movementHeight, 3.0f),
		new Vector3(6.0f, movementHeight, 8.0f),
		new Vector3(5.0f, movementHeight, 12.0f),
		new Vector3(13.0f, movementHeight, 11.0f),
		new Vector3(15.0f, movementHeight, 2.0f),
		new Vector3(15.0f, movementHeight, 6.0f),
		new Vector3(6.0f, movementHeight, 13.0f),
		new Vector3(17.0f, movementHeight, 13.0f),
		new Vector3(2.0f, movementHeight, 7.0f),
		new Vector3(6.0f, movementHeight, 15.0f),
		new Vector3(11.0f, movementHeight, 11.0f),
		new Vector3(7.0f, movementHeight, 7.0f),
		new Vector3(3.0f, movementHeight, 8.0f),
		new Vector3(2.0f, movementHeight, 7.0f),
		new Vector3(17.0f, movementHeight, 4.0f),
		new Vector3(17.0f, movementHeight, 11.0f),
		new Vector3(3.0f, movementHeight, 5.0f),
		new Vector3(2.0f, movementHeight, 3.0f),
		new Vector3(2.0f, movementHeight, 15.0f),
		new Vector3(17.0f, movementHeight, 4.0f),
		new Vector3(16.0f, movementHeight, 2.0f),
		new Vector3(1.0f, movementHeight, 9.0f),
		new Vector3(15.0f, movementHeight, 1.0f),
		new Vector3(17.0f, movementHeight, 15.0f),
		new Vector3(8.0f, movementHeight, 18.0f),
		new Vector3(17.0f, movementHeight, 9.0f),
		new Vector3(9.0f, movementHeight, 4.0f),
		new Vector3(6.0f, movementHeight, 12.0f),
		new Vector3(10.0f, movementHeight, 6.0f),
		new Vector3(13.0f, movementHeight, 18.0f),
		new Vector3(5.0f, movementHeight, 11.0f)
	};
	
	static List<Vector3> NoWalls = new List<Vector3> {
		new Vector3(13.0f, movementHeight, 18.0f),
		new Vector3(12.0f, movementHeight, 6.0f),
		new Vector3(17.0f, movementHeight, 15.0f),
		new Vector3(18.0f, movementHeight, 16.0f),
		new Vector3(8.0f, movementHeight, 13.0f),
		new Vector3(6.0f, movementHeight, 13.0f),
		new Vector3(17.0f, movementHeight, 12.0f),
		new Vector3(11.0f, movementHeight, 15.0f),
		new Vector3(17.0f, movementHeight, 18.0f),
		new Vector3(2.0f, movementHeight, 18.0f),
		new Vector3(9.0f, movementHeight, 13.0f),
		new Vector3(13.0f, movementHeight, 9.0f),
		new Vector3(14.0f, movementHeight, 11.0f),
		new Vector3(10.0f, movementHeight, 16.0f),
		new Vector3(17.0f, movementHeight, 7.0f),
		new Vector3(9.0f, movementHeight, 1.0f),
		new Vector3(13.0f, movementHeight, 6.0f),
		new Vector3(4.0f, movementHeight, 11.0f),
		new Vector3(18.0f, movementHeight, 16.0f),
		new Vector3(18.0f, movementHeight, 18.0f),
		new Vector3(7.0f, movementHeight, 5.0f),
		new Vector3(4.0f, movementHeight, 5.0f),
		new Vector3(18.0f, movementHeight, 2.0f),
		new Vector3(8.0f, movementHeight, 13.0f),
		new Vector3(15.0f, movementHeight, 1.0f),
		new Vector3(10.0f, movementHeight, 8.0f),
		new Vector3(4.0f, movementHeight, 10.0f),
		new Vector3(11.0f, movementHeight, 12.0f),
		new Vector3(1.0f, movementHeight, 3.0f),
		new Vector3(17.0f, movementHeight, 15.0f),
		new Vector3(11.0f, movementHeight, 15.0f),
		new Vector3(7.0f, movementHeight, 13.0f),
		new Vector3(15.0f, movementHeight, 2.0f),
		new Vector3(5.0f, movementHeight, 8.0f),
		new Vector3(18.0f, movementHeight, 16.0f),
		new Vector3(15.0f, movementHeight, 11.0f),
		new Vector3(14.0f, movementHeight, 11.0f),
		new Vector3(4.0f, movementHeight, 8.0f),
		new Vector3(4.0f, movementHeight, 18.0f),
		new Vector3(12.0f, movementHeight, 3.0f),
		new Vector3(14.0f, movementHeight, 3.0f),
		new Vector3(18.0f, movementHeight, 6.0f),
		new Vector3(12.0f, movementHeight, 10.0f),
		new Vector3(7.0f, movementHeight, 14.0f),
		new Vector3(6.0f, movementHeight, 12.0f),
		new Vector3(13.0f, movementHeight, 9.0f),
		new Vector3(13.0f, movementHeight, 7.0f),
		new Vector3(14.0f, movementHeight, 15.0f),
		new Vector3(6.0f, movementHeight, 13.0f),
		new Vector3(6.0f, movementHeight, 5.0f),
		new Vector3(8.0f, movementHeight, 12.0f),
		new Vector3(4.0f, movementHeight, 7.0f),
		new Vector3(14.0f, movementHeight, 13.0f),
		new Vector3(3.0f, movementHeight, 1.0f),
		new Vector3(1.0f, movementHeight, 18.0f),
		new Vector3(11.0f, movementHeight, 8.0f),
		new Vector3(11.0f, movementHeight, 14.0f),
		new Vector3(18.0f, movementHeight, 14.0f),
		new Vector3(13.0f, movementHeight, 6.0f),
		new Vector3(10.0f, movementHeight, 4.0f),
		new Vector3(14.0f, movementHeight, 11.0f),
		new Vector3(12.0f, movementHeight, 12.0f),
		new Vector3(15.0f, movementHeight, 7.0f),
		new Vector3(5.0f, movementHeight, 5.0f),
		new Vector3(7.0f, movementHeight, 14.0f),
		new Vector3(12.0f, movementHeight, 4.0f),
		new Vector3(16.0f, movementHeight, 9.0f),
		new Vector3(13.0f, movementHeight, 9.0f),
		new Vector3(3.0f, movementHeight, 13.0f),
		new Vector3(7.0f, movementHeight, 9.0f),
		new Vector3(1.0f, movementHeight, 8.0f),
		new Vector3(7.0f, movementHeight, 1.0f),
		new Vector3(15.0f, movementHeight, 10.0f),
		new Vector3(6.0f, movementHeight, 18.0f),
		new Vector3(13.0f, movementHeight, 4.0f),
		new Vector3(10.0f, movementHeight, 18.0f),
		new Vector3(9.0f, movementHeight, 2.0f),
		new Vector3(16.0f, movementHeight, 3.0f),
		new Vector3(11.0f, movementHeight, 12.0f),
		new Vector3(17.0f, movementHeight, 15.0f),
		new Vector3(11.0f, movementHeight, 8.0f),
		new Vector3(12.0f, movementHeight, 17.0f),
		new Vector3(14.0f, movementHeight, 3.0f),
		new Vector3(14.0f, movementHeight, 10.0f),
		new Vector3(7.0f, movementHeight, 3.0f),
		new Vector3(15.0f, movementHeight, 14.0f),
		new Vector3(17.0f, movementHeight, 2.0f),
		new Vector3(4.0f, movementHeight, 2.0f),
		new Vector3(7.0f, movementHeight, 16.0f),
		new Vector3(2.0f, movementHeight, 11.0f),
		new Vector3(17.0f, movementHeight, 4.0f),
		new Vector3(11.0f, movementHeight, 17.0f),
		new Vector3(8.0f, movementHeight, 1.0f),
		new Vector3(17.0f, movementHeight, 2.0f),
		new Vector3(9.0f, movementHeight, 4.0f),
		new Vector3(14.0f, movementHeight, 8.0f),
		new Vector3(5.0f, movementHeight, 10.0f),
		new Vector3(9.0f, movementHeight, 16.0f),
		new Vector3(14.0f, movementHeight, 15.0f),
		new Vector3(16.0f, movementHeight, 15.0f)
	};
	#endregion
	
	
	void Start () 
	{
		algorithmLabel = algorithms[algoIndex];
		degreeLabel	= degrees[degreeIndex];
		wallLabel = wallSets[wallIndex];
		readyToGo = true;
		FloorGrid.InitializeGrid();
		start = new AStarObject();
		currentGoal	= new AStarObject();
		endGoal	= new AStarObject(new Vector3(userInputX, movementHeight, userInputZ), zero, zero, zero);
		CastRay(); // Get initial location on the movement grid
	}
	
	
	#region Update
	void Update() 
	{
		
		if(Input.GetKeyUp(KeyCode.Escape)) 
		{
			Application.Quit();
		}
		
		if(Input.GetKeyUp(KeyCode.Q)) 
		{
			if(Time.timeScale == 1) {
				Time.timeScale = 0;
			}
			else {
				Time.timeScale = 1;
			}
		}
		
		if(Input.GetKeyUp(KeyCode.R)) 
		{
			if(begin) 
			{
				//WriteDataFile();
				//GetCoordinateSets();
				pathDataList = new List<int>();
				searchedDataList = new List<int>();
				timeDataList = new List<int>();
				//coordinateList = new List<Vector3>();
				readyToGo = true;
				isMoving = false;
				count = 0;
				pathCount = 0;
				goalCount = 0;
				coordCount = 0;
				HUDgoal = empty;
				timeElapsed = 0;
				transform.position = defaultStart;
				transform.rotation = Quaternion.identity;
				pathNodesList	= new List<Vector3>();
				closedList		= new List<Vector3>();
				openList		= new List<Vector3>();
				start			= new AStarObject();
				currentGoal		= new AStarObject();
				finalPath 		= new List<AStarObject>();
				ObjectCleanup();
				CastRay();
			}
			endGoal = new AStarObject(new Vector3(userInputX, movementHeight, userInputZ), zero, zero, zero);
			begin = !begin;
		}
		
		if(Input.GetKeyUp(KeyCode.F) && !begin) 
		{
			if(degreeIndex == 1) {
				degreeIndex = 0;
			}
			else {
				degreeIndex = 1;
			}
			degreeLabel = degrees[degreeIndex];
		}
		
		if(Input.GetKeyUp(KeyCode.T) && !begin) 
		{
			if(algoIndex < 2) {
				algoIndex++;
			}
			else {
				algoIndex = 0;
			}
			algorithmLabel = algorithms[algoIndex];
		}
		
		
		if(Input.GetKeyUp(KeyCode.G) && !begin) 
		{
			if(wallIndex == 0) {
				wallIndex = 1;
				wallObject1.SetActive(true);
			}
			else if(wallIndex == 1) {
				wallIndex = 2;
				wallObject1.SetActive(false);
				wallObject2.SetActive(true);
			}
			else if(wallIndex == 2) {
				wallIndex = 0;
				wallObject2.SetActive(false);
			}
			wallLabel = wallSets[wallIndex];
			FloorGrid.InitializeGrid();
		}
		
		
		if(!isMoving && readyToGo && begin) 
		{
			if(!MovementAccuracy(endGoal)) 
			{
				if(finalPath.Count == 0) 
				{
					stopwatch = new Stopwatch();
					stopwatch.Start();
					if(algoIndex == 0) 
					{
						if(!AStar()) {
							// Will only occur if there is an issue within AStar()
							UnityEngine.Debug.Log ("ERROR -- returning from AStar");
						}
					}
					
					if(algoIndex == 1) 
					{
						if(!GBFS()) {
							// Will only occur if there is an issue within GBFS()
							UnityEngine.Debug.Log ("ERROR -- returning from GBFS");
						}
					}
					
					if(algoIndex == 2) 
					{
						if(!Dijkstra()) {
							// Will only occur if there is an issue within Dijkstra()
							UnityEngine.Debug.Log ("ERROR -- returning from Dijkstra");
						}
					}
					stopwatch.Stop();
					//UnityEngine.Debug.Log ("Time: " + stopwatch.Elapsed);
					timeDataList.Add((int)stopwatch.ElapsedMilliseconds);
					timeElapsed = (int)stopwatch.ElapsedMilliseconds;
					
					HUDgoal = endGoal.Coords;
					pathCount = finalPath.Count;
					pathDataList.Add(pathCount);
					searchedDataList.Add(totalConsidered);
					//coordinateList.Add(endGoal.Coords);
					coordCount++;
					//UnityEngine.Debug.Log("* * * NEXT GOAL : (" + endGoal.Coords.x + ", " + endGoal.Coords.z + ") * * *");
					//UnityEngine.Debug.Log("count = " + count + " // finalPath[count] = " + finalPath[count] + " // currentGoal = " + currentGoal);
					currentGoal = finalPath[count];
				}
				isMoving = true; //Sets flag to denote that movement is to be taking place
				readyToGo = false; //Sets flag to denote that coordinates have been accepted
			}
			
			foreach(AStarObject ASO in open) {
				if(ASO.Coords != endGoal.Coords) {
					Instantiate(searchedNode, new Vector3(ASO.Coords.x, 0.05f, ASO.Coords.z), Quaternion.identity);
				}
			}
			
			foreach(AStarObject ASO in closed) {
				if(ASO.Coords != endGoal.Coords) {
					Instantiate(searchedNode, new Vector3(ASO.Coords.x, 0.05f, ASO.Coords.z), Quaternion.identity);
				}
			}
			
			foreach(AStarObject ASO in finalPath) {
				if(ASO.Coords == endGoal.Coords) {
					Instantiate(goalX, new Vector3(ASO.Coords.x, 0.05f, ASO.Coords.z), Quaternion.identity);
				}
				else {
					Instantiate(pathMarker, new Vector3(ASO.Coords.x, 0.06f, ASO.Coords.z), Quaternion.identity);
				}
			}
		}


		if(isMoving && !readyToGo) {
			if(MovementAccuracy(currentGoal) && count < finalPath.Count) {
				count++;
				//pathCount--;
				currentGoal = finalPath[count];
			}
			Movement();
		}


		if(MovementAccuracy(endGoal)) {
			ObjectCleanup();
			count		= 0;
			//pathCount	= 0;
			isMoving	= false;
			readyToGo 	= true;
			CastRay();
			finalPath	= new List<AStarObject>();
			//GetRandomCoords();
			GetNextGoalCoords();
		}
		
	}
	#endregion
	
	
	#region Movement
	void CastRay() //Gets the current position in terms of X and Z coordinates
	{ 
		Ray myRay = new Ray(this.gameObject.transform.position, rayCastDirection);
		RaycastHit hit;
		
		if(Physics.Raycast(myRay, out hit, rayCastDistance)) {
			locationObject = hit.transform.gameObject;
			start.Coords = new Vector3(locationObject.transform.position.x, movementHeight, locationObject.transform.position.z);
			//UnityEngine.Debug.Log("X = " + start.Coords.x + " || Z = " + start.Coords.z);
		}
		else {
			UnityEngine.Debug.Log("No raycast hit");
		}
		HUDstart = start.Coords;
	}
	
	
	void Movement() 
	{
		Vector3 moveDirection = currentGoal.Coords - transform.position;
		//Vector3 moveVector = moveDirection.normalized * slothSpeed * Time.deltaTime; // super slow movement
		//Vector3 moveVector = moveDirection.normalized * walkSpeed * Time.deltaTime; // slower movement
		Vector3 moveVector = moveDirection.normalized * runSpeed * Time.deltaTime; // faster movement

		transform.position += moveVector;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 4 * Time.deltaTime);
	}
	
	
	// Is the Friend object within the sufficient marginal distance from the goal point to be considered "there"?
	bool MovementAccuracy(AStarObject goalNode) 
	{
		if((transform.position.x > goalNode.Coords.x - moveAccuracy) &&
		   (transform.position.x < goalNode.Coords.x + moveAccuracy) &&
		   (transform.position.z > goalNode.Coords.z - moveAccuracy) &&
		   (transform.position.z < goalNode.Coords.z + moveAccuracy)) 
		   { return true; } 
		else 
			{ return false; }
	}
	#endregion
	
	
	#region AStar
	bool AStar() {
		open = new List<AStarObject>();
		closed = new List<AStarObject>();
		
		//Insert the starting point into the open consideration list
		open.Add(start);
		open[0].FValue = zero;
		open[0].GValue = zero;
		open[0].HValue = zero;
		
		
		while (open.Count > 0) { // Run this loop so long as any coordinates remain in open
			
			neighbors = new List<AStarObject>(); // Clear the list for the next evaluated node's neighbors
			
			current = open[0]; // Get first item (lowest Fcost) in open
			
			if(current.Coords == endGoal.Coords) { // If the node being evaluated is the goal
				closed.Add(current);
				//UnityEngine.Debug.Log ("current == endGoal");
				ReconstructPath();
				return true;
			}
			
			/*At this point in every iteration of the while loop, open will be in
			ascending order (least F value to greatest F value)*/
			closed.Add(current);
			open.RemoveAt(0); //Remove the first node, and therefore lowest F value, from the open list
			
			/*At any given point in any iteration of the while loop, the neighbors list
			will hold anywhere from 2-8 adjacent nodes, depending on the location of current*/
			if(degreeIndex == 1) {
				PopulateNeighbors1(current);
			}
			else {
				PopulateNeighborsInfinity(current);
			}
			
			
			foreach(AStarObject neighbor in neighbors) {
				AStarObject openNeighbor = new AStarObject();
				int openIndex = 0;
				float newCost			= zero;
				bool openFound = false,
					closedFound	= false;
				
				
				// Is this neighbor already in the closed list?
				foreach(AStarObject closedNode in closed) 
				{
					if(closedNode.Coords == neighbor.Coords) 
					{
						closedFound = true;
						break;
					}
				}
				// If the neighbor is already in the closed list OR is not passable, continue to the next neighbor
				if(closedFound || !FloorGrid.grid[(int)neighbor.Coords.x, (int)neighbor.Coords.z].Pass) 
				{
					//UnityEngine.Debug.Log((int)neighbor.Coords.x + "," + (int)neighbor.Coords.z + " passable? " + FloorGrid.grid[(int)neighbor.Coords.x, (int)neighbor.Coords.z].Pass);
					continue;
				}
				
				
				// Is this neighbor already in the open list?
				foreach(AStarObject openNode in open) 
				{
					if(openNode.Coords == neighbor.Coords) 
					{
						openNeighbor = openNode;
						openIndex = open.IndexOf(openNode);
						openFound = true;
						break;
					}
				}
				
				
				/*Taking cost from start to currently evaluated node (G(current)) and
				adding it to the cost from the currently evaluated node to the currently
				evaluated neighbor node (Distance(current, neighbor)), and doing this
				for each neighbor node to find the lowest cost one*/
				if(degreeIndex == 1) {
					newCost = current.GValue + Distance1(current, neighbor);
				}
				else {
					newCost = current.GValue + DistanceInfinity(current, neighbor);
				}
				
				// If this neighbor is NOT already in the open list
				if(!openFound) 
				{
					neighbor.Parent = current;
					if(degreeIndex == 1) {
						neighbor.HValue = H1(neighbor);
					}
					else {
						neighbor.HValue = HInfinity(neighbor);
					}
					neighbor.GValue = newCost;
					neighbor.FValue = neighbor.GValue + neighbor.HValue;
					
					if(!AStarPriorityInsert(neighbor)) {
						UnityEngine.Debug.Log ("ERROR -- PRIORITY INSERT");
					}
				}
				else if(newCost < openNeighbor.GValue) 
				{
					openNeighbor.Parent = current;
					openNeighbor.GValue = newCost;
					openNeighbor.FValue = openNeighbor.GValue + openNeighbor.HValue;
					
					open.RemoveAt(openIndex);
					
					if(!AStarPriorityInsert(openNeighbor)) {
						UnityEngine.Debug.Log ("ERROR -- PRIORITY INSERT");
					}
				}
				
			}
		}
		
		UnityEngine.Debug.Log ("RETURNING FAIL STATE -- Path does not exist");
		finalPath.Add(new AStarObject(empty, zero, zero, zero)); //Add the empty Vector3 to the list; should be the only element at this point
		return false;
	}
	#endregion
	
	
	#region GBFS
	bool GBFS() 
	{
		open = new List<AStarObject>();
		closed = new List<AStarObject>();
		
		//Insert the starting point into the open consideration list
		open.Add(start);
		open[0].FValue = zero;
		open[0].GValue = zero;
		//open[0].HValue = zero;
		
		
		while (open.Count > 0) // Run this loop so long as any coordinates remain in open
		{ 
			neighbors = new List<AStarObject>(); // Clear the list for the next evaluated node's neighbors
			
			current = open[0]; // Get first item (lowest Fcost) in open
			
			if(current.Coords == endGoal.Coords) // If the node being evaluated is the goal
			{ 
				closed.Add(current);
				//UnityEngine.Debug.Log ("current == endGoal");
				ReconstructPath();
				return true;
			}
			
			/*At this point in every iteration of the while loop, open will be in
			ascending order (least F value to greatest F value)*/
			closed.Add(current);
			open.RemoveAt(0); //Remove the first node, and therefore lowest F value, from the open list
			
			/*At any given point in any iteration of the while loop, the neighbors list
			will hold anywhere from 2-8 adjacent nodes, depending on the location of current*/
			if(degreeIndex == 1) 
			{
				PopulateNeighbors1(current);
			}
			else 
			{
				PopulateNeighborsInfinity(current);
			}
			
			
			foreach(AStarObject neighbor in neighbors) 
			{
				AStarObject openNeighbor 	= new AStarObject();
				int			openIndex 		= 0;
				float		newCost			= zero;
				bool		openFound		= false,
							closedFound		= false;
				
				
				// Is this neighbor already in the closed list?
				foreach(AStarObject closedNode in closed) 
				{
					if(closedNode.Coords == neighbor.Coords) 
					{
						closedFound = true;
						break;
					}
				}
				// If the neighbor is already in the closed list OR is not passable, continue to the next neighbor
				if(closedFound || !FloorGrid.grid[(int)neighbor.Coords.x, (int)neighbor.Coords.z].Pass) {
					//UnityEngine.Debug.Log((int)neighbor.Coords.x + "," + (int)neighbor.Coords.z + " passable? " + FloorGrid.grid[(int)neighbor.Coords.x, (int)neighbor.Coords.z].Pass);
					continue;
				}
				
				
				// Is this neighbor already in the open list?
				foreach(AStarObject openNode in open) 
				{
					if(openNode.Coords == neighbor.Coords) {
						openNeighbor	= openNode;
						openIndex		= open.IndexOf(openNode);
						openFound		= true;
						break;
					}
				}
				
				
				/*Taking cost from start to currently evaluated node (G(current)) and
				adding it to the cost from the currently evaluated node to the currently
				evaluated neighbor node (Distance(current, neighbor)), and doing this
				for each neighbor node to find the lowest cost one*/
				if(degreeIndex == 1) {
					newCost = H1(neighbor);
				}
				else {
					newCost = HInfinity(neighbor);
				}
				
				// If this neighbor is NOT already in the open list
				if(!openFound) 
				{
					neighbor.Parent = current;
					neighbor.HValue = newCost;
					//neighbor.GValue = newCost;
					//neighbor.FValue = neighbor.GValue + neighbor.HValue;
					
					if(!GBFSPriorityInsert(neighbor)) {
						UnityEngine.Debug.Log ("ERROR -- GBFS PRIORITY INSERT");
					}
				}
				else if(newCost < openNeighbor.HValue) 
				{
					openNeighbor.Parent = current;
					openNeighbor.HValue = newCost;
					
					open.RemoveAt(openIndex);
					
					if(!GBFSPriorityInsert(openNeighbor)) {
						UnityEngine.Debug.Log ("ERROR -- GBFS PRIORITY INSERT");
					}
				}
				
			}
		}
		
		UnityEngine.Debug.Log ("RETURNING FAIL STATE -- Path does not exist");
		finalPath.Add(new AStarObject(empty, zero, zero, zero)); //Add the empty Vector3 to the list; should be the only element at this point
		return false;
	}
	#endregion
	
	
	#region Dijkstra
	bool Dijkstra() 
	{
		open = new List<AStarObject>();
		closed = new List<AStarObject>();
		
		//Insert the starting point into the open consideration list
		open.Add(start);
		//open[0].FValue = zero;
		open[0].GValue = zero;
		//open[0].HValue = zero;
		
		
		while (open.Count > 0) { // Run this loop so long as any coordinates remain in open
			
			neighbors = new List<AStarObject>(); // Clear the list for the next evaluated node's neighbors
			
			current = open[0]; // Get first item (lowest Fcost) in open
			
			if(current.Coords == endGoal.Coords) { // If the node being evaluated is the goal
				closed.Add(current);
				ReconstructPath();
				return true;
			}
			
			/*At this point in every iteration of the while loop, open will be in
			ascending order (least F value to greatest F value)*/
			closed.Add(current);
			open.RemoveAt(0); //Remove the first node, and therefore lowest F value, from the open list
			
			/*At any given point in any iteration of the while loop, the neighbors list
			will hold anywhere from 2-8 adjacent nodes, depending on the location of current*/
			if(degreeIndex == 1) {
				PopulateNeighbors1(current);
			}
			else {
				PopulateNeighborsInfinity(current);
			}
			
			
			foreach(AStarObject neighbor in neighbors) 
			{
				AStarObject openNeighbor = new AStarObject();
				int openIndex = 0;
				float newCost = zero;
				bool openFound = false,
				closedFound	= false;
				
				
				// Is this neighbor already in the closed list?
				foreach(AStarObject closedNode in closed) 
				{
					if(closedNode.Coords == neighbor.Coords) 
					{
						closedFound = true;
						break;
					}
				}
				// If the neighbor is already in the closed list OR is not passable, continue to the next neighbor
				if(closedFound || !FloorGrid.grid[(int)neighbor.Coords.x, (int)neighbor.Coords.z].Pass) 
				{
					//UnityEngine.Debug.Log((int)neighbor.Coords.x + "," + (int)neighbor.Coords.z + " passable? " + FloorGrid.grid[(int)neighbor.Coords.x, (int)neighbor.Coords.z].Pass);
					continue;
				}
				
				
				// Is this neighbor already in the open list?
				foreach(AStarObject openNode in open) 
				{
					if(openNode.Coords == neighbor.Coords) 
					{
						openNeighbor = openNode;
						openIndex = open.IndexOf(openNode);
						openFound = true;
						break;
					}
				}
				
				
				/*Taking cost from start to currently evaluated node (G(current)) and
				adding it to the cost from the currently evaluated node to the currently
				evaluated neighbor node (Distance(current, neighbor)), and doing this
				for each neighbor node to find the lowest cost one*/
				if(degreeIndex == 1) {
					newCost = current.GValue + Distance1(current, neighbor);
				}
				else {
					newCost = current.GValue + DistanceInfinity(current, neighbor);
				}
				
				// If this neighbor is NOT already in the open list
				if(!openFound) {
					
					neighbor.Parent = current;
					//neighbor.HValue = H(neighbor);
					neighbor.GValue = newCost;
					//neighbor.FValue = neighbor.GValue + neighbor.HValue;
					
					if(!DijkstraPriorityInsert(neighbor)) {
						UnityEngine.Debug.Log ("ERROR -- DIJKSTRA PRIORITY INSERT");
					}
				}
				else if(newCost < openNeighbor.GValue) {
					openNeighbor.Parent = current;
					openNeighbor.GValue = newCost;
					//openNeighbor.FValue = openNeighbor.GValue + openNeighbor.HValue;
					
					open.RemoveAt(openIndex);
					
					if(!DijkstraPriorityInsert(openNeighbor)) {
						UnityEngine.Debug.Log ("ERROR -- DIJKSTRA PRIORITY INSERT");
					}
				}
			}
		}
		
		UnityEngine.Debug.Log ("RETURNING FAIL STATE -- Path does not exist");
		finalPath.Add(new AStarObject(empty, zero, zero, zero)); //Add the empty Vector3 to the list; should be the only element at this point
		return false;
	}
	#endregion
	
	
	#region Insert functions
	bool AStarPriorityInsert(AStarObject neighbor) 
	{
		bool check = false;
		
		foreach(AStarObject openNode in open) 
		{
			if(neighbor.FValue < openNode.FValue) 
			{
				/*if(neighbor.FValue == openNode.FValue) {
					
					if(neighbor.GValue < openNode.GValue) {
						
						open.Insert(open.IndexOf(openNode), neighbor);
						check = true;
						return check; //If the gVal is less, add before the one just checked, return true
					}
					
				}*/
				
				open.Insert(open.IndexOf(openNode), neighbor);
				check = true;
				return check; //If the fVal is less/equal, add before the one just checked, return true
			}	
		}
		
		if(!check) { //If it's the highest Fcost out of the open list, add to the end, return true
			open.Add(neighbor);
			check = true;
		}
		
		return check;
	}
	
	
	bool GBFSPriorityInsert(AStarObject neighbor) 
	{
		bool check = false;
		
		foreach(AStarObject openNode in open) 
		{
			if(neighbor.HValue < openNode.HValue) 
			{
				/*if(neighbor.FValue == openNode.FValue) {
					
					if(neighbor.GValue < openNode.GValue) {
						open.Insert(open.IndexOf(openNode), neighbor);
						check = true;
						return check; //If the gVal is less, add before the one just checked, return true
					}
				}*/
				
				open.Insert(open.IndexOf(openNode), neighbor);
				check = true;
				return check; //If the fVal is less/equal, add before the one just checked, return true
			}	
		}
		
		if(!check) { //If it's the highest Fcost out of the open list, add to the end, return true
			open.Add(neighbor);
			check = true;
		}
		
		return check;
	}
	
	
	bool DijkstraPriorityInsert(AStarObject neighbor) 
	{
		bool check = false;
		
		foreach(AStarObject openNode in open) 
		{
			if(neighbor.GValue < openNode.GValue) 
			{
				/*if(neighbor.FValue == openNode.FValue) {
					
					if(neighbor.GValue < openNode.GValue) {
						open.Insert(open.IndexOf(openNode), neighbor);
						check = true;
						return check; //If the gVal is less, add before the one just checked, return true
					}
				}*/
				
				open.Insert(open.IndexOf(openNode), neighbor);
				check = true;
				return check; //If the fVal is less/equal, add before the one just checked, return true
			}	
		}
		
		if(!check) { //If it's the highest Fcost out of the open list, add to the end, return true
			open.Add(neighbor);
			check = true;
		}
		
		return check;
	}
	#endregion
	
	
	#region L(1) funtions
	// These G, H, and Distance functions are set up for use with [L(1)] Manhattan distance measurement
	float G1(AStarObject current) { //Distance from start to current
		return 10 * (Mathf.Abs (current.Coords.x - start.Coords.x) + Mathf.Abs (current.Coords.z - start.Coords.z));
	}
	
	float H1(AStarObject current) //Heuristic; ESTIMATED distance from potential next to goal
	{ 
		float dX1 = Mathf.Abs (current.Coords.x - endGoal.Coords.x);
		float dZ1 = Mathf.Abs (current.Coords.z - endGoal.Coords.z);
		float dX2 = Mathf.Abs (endGoal.Coords.x - start.Coords.x);
		float dZ2 = Mathf.Abs (endGoal.Coords.z - start.Coords.z);
		float cross = Mathf.Abs (dX1*dZ2 - dX2*dZ1);
		
		return cross*0.001f + (10 * (Mathf.Abs (endGoal.Coords.x - current.Coords.x) + Mathf.Abs (endGoal.Coords.z - current.Coords.z)));
		//return 10 * (Mathf.Abs (endGoal.Coords.x - current.Coords.x) + Mathf.Abs (endGoal.Coords.z - current.Coords.z));
	}
	
	float Distance1(AStarObject current, AStarObject neighbor) { //Distance from current to a neighbor
		return 10 * (Mathf.Abs (neighbor.Coords.x - current.Coords.x) + Mathf.Abs (neighbor.Coords.z - current.Coords.z));
	}
	
	
	/*This PopulateNeighbors() will populate the neighbors List<> according to [L(1)] Manhattan distance measurement
	It will not always add the same number of nodes; the amount varies between 2 and 4*/
	void PopulateNeighbors1(AStarObject current) 
	{
		if(current.Coords.x > minCoord) {
			neighbors.Add ( FloorGrid.grid[(int)(current.Coords.x - 1), (int)current.Coords.z]		 );	//Add L neighbor
		}
		if(current.Coords.z > minCoord) {
			neighbors.Add ( FloorGrid.grid[(int)current.Coords.x,		(int)(current.Coords.z - 1)] );	//Add D neighbor
		}
		if(current.Coords.x < maxCoord) {
			neighbors.Add ( FloorGrid.grid[(int)(current.Coords.x + 1), (int)current.Coords.z] 		 );	//Add R neighbor
		}
		if(current.Coords.z < maxCoord) {
			neighbors.Add ( FloorGrid.grid[(int)current.Coords.x, 		(int)(current.Coords.z + 1)] );	//Add U neighbor
		}
	}
	#endregion
	
	
	#region L(infinity) functions
	// These G, H, and Distance functions are set up for use with [L(infinity)] Diagonal distance measurement
	
	float GInfinity(AStarObject current) //Distance from start to current
	{ 
		float dX = Mathf.Abs (current.Coords.x - start.Coords.x);
		float dZ = Mathf.Abs (current.Coords.z - start.Coords.z);
		 
		return (vhMoveCost * (dX + dZ)) + ((dMoveCost - (2 * vhMoveCost)) * Mathf.Min(dX, dZ));
		//return Mathf.Max ( Mathf.Abs (start.Coords.x - current.Coords.x), Mathf.Abs (start.Coords.z - current.Coords.z) );
	}
	
	float HInfinity(AStarObject current) //Heuristic; ESTIMATED distance from potential next to goal
	{ 
		float dX = Mathf.Abs (endGoal.Coords.x - current.Coords.x);
		float dZ = Mathf.Abs (endGoal.Coords.z - current.Coords.z);
		float dX1 = Mathf.Abs (current.Coords.x - endGoal.Coords.x);
		float dZ1 = Mathf.Abs (current.Coords.z - endGoal.Coords.z);
		float dX2 = Mathf.Abs (endGoal.Coords.x - start.Coords.x);
		float dZ2 = Mathf.Abs (endGoal.Coords.z - start.Coords.z);
		float cross = Mathf.Abs (dX1*dZ2 - dX2*dZ1);
		
		return cross*0.001f + (vhMoveCost * (dX + dZ)) + ((dMoveCost - (2 * vhMoveCost)) * Mathf.Min(dX, dZ));
		//return (vhMoveCost * (dX + dZ)) + ((dMoveCost - (2 * vhMoveCost)) * Mathf.Min(dX, dZ));
		//return Mathf.Max ( Mathf.Abs (endGoal.Coords.x - current.Coords.x), Mathf.Abs (endGoal.Coords.z - current.Coords.z) );
	}
	
	float DistanceInfinity(AStarObject current, AStarObject neighbor) //Distance from current to a neighbor
	{ 
		float dX = Mathf.Abs (neighbor.Coords.x - current.Coords.x);
		float dZ = Mathf.Abs (neighbor.Coords.z - current.Coords.z);
		
		return (vhMoveCost * (dX + dZ)) + ((dMoveCost - (2 * vhMoveCost)) * Mathf.Min(dX, dZ));
		//return Mathf.Max ( Mathf.Abs (neighbor.Coords.x - current.Coords.x), Mathf.Abs (neighbor.Coords.z - current.Coords.z) );
	}
	
	
	/* This PopulateNeighbors() will populate the neighbors List<> according to [L(infinity)] Diagonal distance measurement
	It will not always add the same number of nodes; the amount varies between 3 and 8*/
	void PopulateNeighborsInfinity(AStarObject current) 
	{
		if(current.Coords.x > minCoord) {
			neighbors.Add ( FloorGrid.grid[(int)(current.Coords.x - 1), (int)current.Coords.z]		 );	//Add L neighbor
		}
		if(current.Coords.x > minCoord && current.Coords.z > minCoord) {
			neighbors.Add ( FloorGrid.grid[(int)(current.Coords.x - 1), (int)(current.Coords.z - 1)] );	//Add DL neighbor
		}
		if(current.Coords.z > minCoord) {
			neighbors.Add ( FloorGrid.grid[(int)current.Coords.x,		(int)(current.Coords.z - 1)] );	//Add D neighbor
		}
		if(current.Coords.z > minCoord && current.Coords.x < maxCoord) {
			neighbors.Add ( FloorGrid.grid[(int)(current.Coords.x + 1), (int)(current.Coords.z - 1)] );	//Add DR neighbor
		}
		if(current.Coords.x < maxCoord) {
			neighbors.Add ( FloorGrid.grid[(int)(current.Coords.x + 1), (int)current.Coords.z] 		 );	//Add R neighbor
		}
		if(current.Coords.x < maxCoord && current.Coords.z < maxCoord) {
			neighbors.Add ( FloorGrid.grid[(int)(current.Coords.x + 1), (int)(current.Coords.z + 1)] );	//Add UR neighbor
		}
		if(current.Coords.z < maxCoord) {
			neighbors.Add ( FloorGrid.grid[(int)current.Coords.x, 		(int)(current.Coords.z + 1)] );	//Add U neighbor
		}
		if(current.Coords.z < maxCoord && current.Coords.x > minCoord) {
			neighbors.Add ( FloorGrid.grid[(int)(current.Coords.x - 1), (int)(current.Coords.z + 1)] );	//Add UL neighbor
		}
	}
	#endregion
	
	
	#region Additional functions
	void ReconstructPath() 
	{
		int count = closed.Count,
			failSafe = 0; // to make sure the while loop doesn't crash the program
		Vector3 coords;
		
		openList = new List<Vector3>();
		closedList = new List<Vector3>();
		pathNodesList = new List<Vector3>();
		
		finalPath.Add(closed[count - 1]); // Add the goal node to finalPath
		coords = finalPath[0].Parent.Coords; // Set coords to goal node coords
		
		while(coords != start.Coords && failSafe < 200) {
		
			foreach(AStarObject node in closed) {
				
				if(node.Coords == coords) {
					finalPath.Insert(0, node);
					coords = node.Parent.Coords;
					break;
				}
			}
			failSafe++;
		}
		
		if(failSafe == 200)
			UnityEngine.Debug.Log("FAILSAFE CAP HIT");
		
		finalPath.Insert(0, start);
		
		
		foreach(AStarObject ASO in finalPath) 
		{
			pathNodesList.Add(ASO.Coords);
			//pathFloats.Add(ASO.FValue);
		}
		
		foreach(AStarObject ASO in closed) 
		{
			closedList.Add(ASO.Coords);
			//closedFloats.Add(ASO.FValue);
		}
		
		foreach(AStarObject ASO in open) 
		{
			openList.Add(ASO.Coords);
			//openFloats.Add(ASO.FValue);
		}
		
		totalConsidered = open.Count + closed.Count;
	}
	
	
	void GetNextGoalCoords() 
	{
		if(wallIndex == 0) {
			endGoal.Coords = NoWalls[goalCount];
		}
		else if(wallIndex == 1) {
			endGoal.Coords = AsstdWalls[goalCount];
		}
		else {
			endGoal.Coords = UTrap[goalCount];
		}
		
		if(goalCount < NoWalls.Count) {
			goalCount++;
		}
	}
	
	
	void GetRandomCoords() {
		int 	xInt = 0, zInt = 0;
		Vector3 goal = new Vector3(zero, movementHeight, zero);
		
		if(wallIndex == 0) {
			while( !FloorGrid.walls0[xInt,zInt] ) {
				xInt = Random.Range(1, 19);
				zInt = Random.Range(1, 19);
			}
		}
		
		if(wallIndex == 1) {
			while( !FloorGrid.walls1[xInt,zInt] ) {
				xInt = Random.Range(1, 19);
				zInt = Random.Range(1, 19);
			}
		}
		
		if(wallIndex == 2) {
			while( !FloorGrid.walls2[xInt,zInt] ) {
				xInt = Random.Range(1, 19);
				zInt = Random.Range(1, 19);
			}
		}
		
		goal.x = (float)xInt;
		goal.z = (float)zInt;
		endGoal.Coords = goal;
	}
	
	
	void ObjectCleanup() {
		pfobjArray = GameObject.FindGameObjectsWithTag("pathmarker");
		gxobjArray = GameObject.FindGameObjectsWithTag("goalx");
		snobjArray = GameObject.FindGameObjectsWithTag("searchednode");
		
		for(int i = 0; i < pfobjArray.Length; i++) {
			Destroy(pfobjArray[i]);
		}
		for(int i = 0; i < gxobjArray.Length; i++) {
			Destroy(gxobjArray[i]);
		}
		for(int i = 0; i < snobjArray.Length; i++) {
			Destroy(snobjArray[i]);
		}
		
	}
	
	
	static public void WriteDataFile() 
	{
		int x;
		//algo_wall_degree
		/*
		"A*",			"GBFS",				"Dijkstra"
		"No Walls",		"Assorted Walls",	"U-Trap"
		"L[Infinity]",	"L[1]"
		
		"AStar_NoWalls_LInf",		"GBFS_NoWalls_LInf",	"Dijkstra_NoWalls_LInf",
		"AStar_AsstdWalls_LInf",	"GBFS_AsstdWalls_LInf",	"Dijkstra_AsstdWalls_LInf",
		"AStar_UTrap_LInf",			"GBFS_UTrap_LInf",		"Dijkstra_UTrap_LInf",
		"AStar_NoWalls_L1",			"GBFS_NoWalls_L1",		"Dijkstra_NoWalls_L1",
		"AStar_AsstdWalls_L1",		"GBFS_AsstdWalls_L1",	"Dijkstra_AsstdWalls_L1",
		"AStar_UTrap_L1",			"GBFS_UTrap_L1",		"Dijkstra_UTrap_L1"
		*/
		
		if(algoIndex == 0) {
			if(wallIndex == 0) {
				if(degreeIndex == 0) {
					x = 0; }
				else {
					x = 9; } }
			else if(wallIndex == 1) {
				if(degreeIndex == 0) {
					x = 3; }
				else {
					x = 12; } }
			else {
				if(degreeIndex == 0) {
					x = 6; }
				else {
					x = 15; } } }
		else if(algoIndex == 1) {
			if(wallIndex == 0) {
				if(degreeIndex == 0) {
					x = 1; }
				else {
					x = 10; } }
			else if(wallIndex == 1) {
				if(degreeIndex == 0) {
					x = 4; }
				else {
					x = 13; } }
			else {
				if(degreeIndex == 0) {
					x = 7; }
				else {
					x = 16; } } }
		else {
			if(wallIndex == 0) {
				if(degreeIndex == 0) {
					x = 2; }
				else {
					x = 11; } }
			else if(wallIndex == 1) {
				if(degreeIndex == 0) {
					x = 5; }
				else {
					x = 14; } }
			else {
				if(degreeIndex == 0) {
					x = 8; }
				else {
					x = 17; } } }
		
		
		/*FileMode.Create overwrites any existing file, which is what we want since the 
		  most updated player history data will always be stored in the PlayerHistory
		  array every time the game launches, or when a game is completed*/
		FileStream fileIO = new FileStream(fileName[x] + ".txt", FileMode.Create, FileAccess.Write);
		
		// Initializes file reader object
		StreamWriter writeFile = new StreamWriter(fileIO);
		
		//string tempStr;
		
		for(int i = 0; i < pathDataList.Count; i++)
			writeFile.WriteLine(pathDataList[i].ToString() + DELIM + searchedDataList[i].ToString() + DELIM + timeDataList[i].ToString());
		
		/*for(int i = 0; i < timeDataList.Count; i++) {
			writeFile.WriteLine(timeDataList[i].ToString());
		}*/
		
		writeFile.Close();
		
		fileIO.Close();
	}
	
	/*
	static public void GetCoordinateSets() {
		FileStream fileIO = new FileStream("Coordinates.txt", FileMode.Create, FileAccess.Write);
		StreamWriter writeFile = new StreamWriter(fileIO); // Initializes file reader object
		
		for(int i = 0; i < coordinateList.Count; i++) {
			writeFile.WriteLine("new Vector3(" + coordinateList[i].x.ToString() + ".0f, movementHeight, " + coordinateList[i].z.ToString() + ".0f)");
		}
		writeFile.Close();
		fileIO.Close(); 
	}
	*/
	
	public static float GetUserInputX() { return userInputX; }
	
	public static float GetUserInputZ() { return userInputZ; }
	
	public static void SetUserInputX(float x) { userInputX = x; }
	
	public static void SetUserInputZ(float z) { userInputZ = z; }
	
	public static void SetHUDgoalX(float x) { HUDgoal.x = x; }
	
	public static void SetHUDgoalZ(float z) { HUDgoal.z = z; }
	
	public static bool GetBeginState() { return begin; }
	
	public static int GetWallIndex() { return wallIndex; }
	
	#endregion
}


