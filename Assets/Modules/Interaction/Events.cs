using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
	[Serializable]
	public class ComponentEvent : UnityEvent<Component> {
	}

	[Serializable]
	public class ColliderEvent : UnityEvent<Collider> {
	}

	[Serializable]
	public class TargetElementEvent : UnityEvent<TargetElement> {
	}
}
