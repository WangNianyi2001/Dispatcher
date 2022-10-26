using NaughtyAttributes;
using UnityEngine;

namespace Game {
	public class HoldableElement : Element {
		[ReorderableList] public TargetElement[] targets;

		public TargetElementEvent onPlace;

		public void OnPlace(TargetElement targetElement) {
			if(!isActiveAndEnabled)
				return;
			onPlace.Invoke(targetElement);
		}

		public void Hold() {
			Protagonist.instance.Interact(this);
		}
	}
}