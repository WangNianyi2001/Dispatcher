using NaughtyAttributes;
using System;
using System.Collections.Generic;

namespace Game {
	public class HoldableElement : Element {
		[ReorderableList] public List<TargetElement> targets;

		public TargetElementEvent onPlace;

		public void OnPlace(TargetElement targetElement) {
			if(!isActiveAndEnabled)
				return;
			onPlace.Invoke(targetElement);
		}

		[NonSerialized] public bool holding = false;

		public void Hold() {
			Protagonist.instance.Interact(this);
		}

		public void Unhold() {
			if(Protagonist.instance.holding == this)
				Protagonist.instance.Unhold();
		}
	}
}