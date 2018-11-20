using UnityEngine;
using System.Collections;
using ASO;

namespace FloorGridArray {

    public class FloorGrid : MonoBehaviour {
	
		#region Variables
		public const int xDist = 20,
						 zDist = 20;
		public const float moveHeight	= 2.0f,
						   zero			= 0.0f;
		
		//static Vector3 emptyVector = new Vector3(zero, moveHeight, zero);
						   
		public static AStarObject[,] grid;
		public static bool[,] walls0;
		public static bool[,] walls1;
		public static bool[,] walls2;
		#endregion
		
		public static bool InitializeGrid() {
			
			//No Walls
			
			walls0 = new bool[,] {
			// 20 x 20 boolean array - row 0 and column 0 will not be used
			// Z    0     1      2      3      4      5      6      7      8      9      10     11     12     13     14     15     16     17     18     19    // X
				{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}, // 0
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 1
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 2
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 3
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 4
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 5
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 6
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 7
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 8
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 9
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 10
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 11
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 12
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 13
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 14
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 15
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 16
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 17
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 18
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }  // 19
			};
			
		
		
		
			
			// Walls 1 -- assorted walls
			// (1,3)->(9,3), (4,6)->(15,6), (15, 1)->(15, 5), (3,10)->(9,10), (3,11)->(9,11), (3,12)->(9,12), (3,13)->(9,13), (13,11)->(19,11), (4, 16)->(16, 16), (4, 17)->(4, 19)
			
			walls1 = new bool[,] {
			// 20 x 20 boolean array - row 0 and column 0 will not be used
			// Z    0     1      2      3      4      5      6      7      8      9      10     11     12     13     14     15     16     17     18     19    // X
				{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}, // 0
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true,  true,  true,  true,  true,  true }, // 1
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true,  true,  true,  true,  true,  true }, // 2
				{false, true,  true,  false, true,  true,  true,  true,  true,  false, false, false, false, true,  true,  true,  true,  true,  true,  true }, // 3
				{false, true,  true,  false, true,  true,  true,  true,  true,  false, false, false, false, true,  true,  false, false, false, false, false}, // 4
				{false, true,  true,  false, true,  true,  false, true,  true,  false, false, false, false, true,  true,  true,  true,  true,  true,  true }, // 5
				{false, true,  true,  false, true,  true,  false, true,  true,  false, false, false, false, true,  true,  true,  true,  true,  true,  true }, // 6
				{false, true,  true,  false, true,  true,  false, true,  true,  false, false, false, false, true,  true,  true,  false, true,  true,  true }, // 7
				{false, true,  true,  false, true,  true,  false, true,  true,  false, false, false, false, true,  true,  true,  false, true,  true,  true }, // 8
				{false, true,  true,  false, true,  true,  false, true,  true,  false, false, false, false, true,  true,  true,  false, true,  true,  true }, // 9
				{false, true,  true,  false, true,  true,  false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 10
				{false, true,  true,  true,  true,  true,  false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 11
				{false, true,  true,  true,  true,  true,  false, true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 12
				{false, true,  true,  false, false, false, false, true,  true,  true,  true,  false, true,  true,  true,  true,  false, true,  true,  true }, // 13
				{false, true,  true,  false, false, false, false, true,  true,  true,  true,  false, true,  true,  true,  true,  false, true,  true,  true }, // 14
				{false, true,  true,  false, false, false, false, true,  true,  true,  true,  false, true,  true,  true,  true,  false, true,  true,  true }, // 15
				{false, true,  true,  false, false, false, false, true,  true,  true,  true,  false, true,  true,  true,  true,  false, true,  true,  true }, // 16
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true,  true,  false, true,  true,  true }, // 17
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true,  true,  true,  true,  true,  true }, // 18
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true,  true,  true,  true,  true,  true }  // 19
			};
			
			
			
			// Walls 2 -- U-shaped trap
			// (4,4)->(4,15), (4,16)->(16,16), (16,4)->(16,15)
			
			walls2 = new bool[,] {
				// 20 x 20 boolean array - row 0 and column 0 will not be used
				// Z    0     1      2      3      4      5      6      7      8      9      10     11     12     13     14     15     16     17     18     19    // X
				{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}, // 0
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 1
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 2
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 3
				{false, true,  true,  true,  false, false, false, false, false, false, false, false, false, false, false, false, false, true,  true,  true }, // 4
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 5
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 6
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 7
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 8
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 9
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 10
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 11
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 12
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 13
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 14
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true }, // 15
				{false, true,  true,  true,  false, false, false, false, false, false, false, false, false, false, false, false, false, true,  true,  true }, // 16
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 17
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }, // 18
				{false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true }  // 19
			};
			
			
			
			grid = new AStarObject[xDist, zDist];
			
			
			for(int i = 0; i < xDist; i++) {
				for(int j = 0; j < zDist; j++) {
					grid[i,j] = new AStarObject( new Vector3( (float)i, moveHeight, (float)j ), zero, zero, zero );
					
					if(FriendScript.GetWallIndex() == 0) {
						grid[i,j].Pass = walls0[i,j];
					}
					if(FriendScript.GetWallIndex() == 1) {
						grid[i,j].Pass = walls1[i,j];
					}
					if(FriendScript.GetWallIndex() == 2) {
						grid[i,j].Pass = walls2[i,j];
					}
					
				}
			}
			return true;
		}
		
	}

}

