using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public enum Force {
		[InspectorName("Don't Care")] DontCare,
		Gang, Capital, Police, Worker
	}

	public static class Constant {
		public static Dictionary<Force, string> forceNames = new Dictionary<Force, string> {
			{ Force.DontCare, "随便" },
			{ Force.Gang, "黑帮" },
			{ Force.Capital, "资本" },
			{ Force.Police, "警局" },
			{ Force.Worker, "工人" },
		};
	}

	public enum SealingType {
		None, Glue, Wax
	}
}