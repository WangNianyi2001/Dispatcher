using System;
using UnityEngine;

namespace Game {
	public class GameManager : MonoBehaviour {
		[Serializable] public struct Elements {
			public InstantElement door;
			public InstantElement bed;
			public Transform envelopeDesktopAnchor;
		}
		public Elements elements;

		public void Log(string message) {
			Debug.Log(message);
		}
	}
}