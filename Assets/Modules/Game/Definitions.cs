using UnityEngine;

namespace Game {
	public enum Force {
		[InspectorName("Don't Care")] DontCare,
		Gang, Capital, Police, Worker
	}

	public enum SealingType {
		None, Glue, Wax
	}
}