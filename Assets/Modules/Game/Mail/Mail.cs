using UnityEngine;
using NaughtyAttributes;
using System;
using System.Collections.Generic;

namespace Game {
	public abstract class Mail : ScriptableObject {
		public Force from, to;
		public SealingType sealing;

		[ResizableTextArea] public string content;

		[Serializable] public class ResponsePair {
			public Force sendTo;
			[Serializable] public class Response {
				public Force effectOn;
				public float reputation;
			}
			[ReorderableList] public List<Response> responses;
		}
		[ReorderableList] public List<ResponsePair> responsePairs;

		public override string ToString() {
			const int trimLength = 20;
			string trimmed = content.Length > trimLength ? content.Substring(0, 20) + "..." : content;
			return $"{GetType().Name} from {from} to {to}: \"{trimmed}\"";
		}
	}
}