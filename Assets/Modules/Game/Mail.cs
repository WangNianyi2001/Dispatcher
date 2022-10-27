using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Game {
	[CreateAssetMenu]
	public class Mail : ScriptableObject {
		public Force from, to;
		public SealingType sealing;

		[ResizableTextArea] public string content;

		public bool end;
		[ShowIf("end"), ResizableTextArea] public string endingRemarks;

		[Serializable] public struct Precedence {
			public Force force;
			public Mail mail;
		}
		[HideIf("end"), ReorderableList] public List<Precedence> precedingMails;
	}
}