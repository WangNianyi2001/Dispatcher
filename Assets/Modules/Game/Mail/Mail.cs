using UnityEngine;
using NaughtyAttributes;

namespace Game {
	public abstract class Mail : ScriptableObject {
		public Force from, to;
		public SealingType sealing;

		[ResizableTextArea] public string content;

		public override string ToString() {
			const int trimLength = 20;
			string trimmed = content.Length > trimLength ? content.Substring(0, 20) + "..." : content;
			return $"{GetType().Name} from {from} to {to}: \"{trimmed}\"";
		}
	}
}