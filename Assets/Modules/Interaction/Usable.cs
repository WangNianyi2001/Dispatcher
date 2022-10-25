using UnityEngine;

namespace Game {
	public class Usable : MonoBehaviour {
		public ComponentEvent onSelect;
		public ComponentEvent onDeselect;
		public ComponentEvent onUse;

		public void OnSelect(Component source) => onSelect.Invoke(source);
		public void OnDeselect(Component source) => onDeselect.Invoke(source);
		public void OnUse(Component source) => onUse.Invoke(source);
	}
}