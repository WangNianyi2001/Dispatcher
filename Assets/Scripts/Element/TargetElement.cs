using UnityEngine;

namespace Game {
	public class TargetElement : InstantElement {
		public HoldableElement target;

		public void OnHold() {
			if(Protagonist.instance.holding == target) {
				Active = true;
			}
		}

		public void UpHold() {
			Active = false;
		}
	}
}