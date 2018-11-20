using UnityEngine;
using System.Collections;

namespace ASO {

	public class AStarObject {
		
		#region Variables
		const float zero = 0.0f;
		static Vector3 empty = new Vector3(zero, 2.0f, zero);
		Vector3 coordinates;
		AStarObject parent;
		bool passable;
		float fCost,
			  gCost,
			  hCost;
		#endregion
		
		
		#region Functions
		public AStarObject(Vector3 coord1, AStarObject parentOBJ, float f, float g, float h) {
			coordinates	= coord1;
			parent		= parentOBJ;
			fCost		= f;
			gCost		= g;
			hCost		= h;
		}
		
		public AStarObject(Vector3 coord1, float f, float g, float h) {
			coordinates	= coord1;
			fCost		= f;
			gCost		= g;
			hCost		= h;
		}
		
		public AStarObject() {
			coordinates = empty;
			fCost		= zero;
			gCost		= zero;
			hCost		= zero;
		}
		
		public Vector3 Coords {
			get { return coordinates; }
			set { coordinates = value; }
		}
		
		public AStarObject Parent {
			get { return parent; }
			set { parent = value; }
		}
		
		public float FValue {
			get { return fCost; }
			set { fCost = value; }
		}
		
		public float GValue {
			get { return gCost; }
			set { gCost = value; }
		}
		
		public float HValue {
			get { return hCost; }
			set { hCost = value; }
		}
		
		public bool Pass {
			get { return passable; }
			set { passable = value; }
		}
		#endregion
		
	}

}