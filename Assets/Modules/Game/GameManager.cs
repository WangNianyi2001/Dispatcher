using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Linq;
using System.Collections;

namespace Game {
	public class GameManager : MonoBehaviour {
		public static GameManager instance;
		public GameManager() {
			instance = this;
		}

		#region Inspector fields
		public MailSequencer sequencer;
		[Serializable] public struct ReputationRecord {
			public Force force;
			public float reputation;
		}
		[SerializeField, ReorderableList] List<ReputationRecord> reputations;

		[Serializable] public struct Resources {
			public GameObject envelopePrefab;
		}
		public Resources resources;

		[Serializable] public struct Elements {
			public InstantElement door, bed;
			public HoldableElement knife, glue, stamp;
			[NonSerialized] public HoldableElement[] tools;
			public TargetElement envelopeAnchor;
			[ReorderableList, NonSerialized] public Mailbox[] mailBoxes;
		}
		public Elements elements;
		#endregion

		#region Private methods
		Mail dispatched, fetched;
		Envelope envelope;
		void DispatchMail() {
			if(fetched) {
				Debug.LogWarning("A mail is already fetched. Skipping dispatching new mail.");
				return;
			}
			if(dispatched) {
				Debug.LogWarning("A mail is already dispatched. Skipping dispatching new mail.");
				return;
			}
			Mail next = sequencer.Next();
			if(!next) {
				Debug.LogWarning("No more mails can be dispatched.");
				return;
			}
			dispatched = next;
		}
		bool HoldingEnvelope {
			set {
				elements.door.Active = !value;
				foreach(var box in elements.mailBoxes)
					box.element.Active = value;
			}
		}
		IEnumerator HoldEnvelope(Envelope envelope) {
			this.envelope = envelope;

			envelope.holdable.targets.AddRange(elements.mailBoxes.Select(mailBox => mailBox.element));
			envelope.holdable.interactable.onInteract.AddListener(_ => HoldingEnvelope = true);
			envelope.holdable.onPlace.AddListener(_ => HoldingEnvelope = false);

			envelope.target.sources.AddRange(elements.tools);

			yield return new WaitForEndOfFrame();
			envelope.holdable.Hold();
		}

		void GameOver() { }
		#endregion

		#region Public interfaces
		public void AddReputation(Force force, float reputation) {
			if(force == Force.DontCare) {
				Debug.LogWarning("Adding reputation to Force.DontCare, skipping adding reputation.");
				return;
			}
			var record = reputations.Find(r => r.force == force);
			record.reputation += reputation;
			if(reputation <= 0)
				GameOver();
		}

		public void FetchMail() {
			if(fetched) {
				Debug.LogWarning("A mail is already fetched. Skipping fetching.");
				return;
			}
			if(!dispatched) {
				DispatchMail();
				if(!dispatched) {
					Debug.LogWarning("No mail is dispatched, cannot fetch.");
					return;
				}
			}
			fetched = dispatched;
			dispatched = null;
			HoldingEnvelope = true;

			GameObject envelope = Instantiate(resources.envelopePrefab, elements.envelopeAnchor.transform);
			StartCoroutine(HoldEnvelope(envelope.GetComponentInChildren<Envelope>()));

			Debug.Log($"Fetched {fetched}");
		}
		public void PostMail(Force force) {
			if(!fetched) {
				Debug.LogWarning("No mail is fetched, cannot post.");
				return;
			}
			fetched = null;
			HoldingEnvelope = false;
			Debug.Log($"Posted mail to {force}");
		}
		public void PostMail(string forceName) => PostMail((Force)Enum.Parse(typeof(Force), forceName));

		public bool HoldingTool {
			set {
				if(!envelope)
					return;
				envelope.holdable.Active = envelope.target.Active = !value;
			}
		}
		#endregion

		#region Life cycle
		void Start() {
			elements.tools = new HoldableElement[] {
				elements.knife,
				elements.glue,
				elements.stamp,
			};
			foreach(var tool in elements.tools) {
				tool.interactable.onInteract.AddListener(_ => {
					HoldingTool = Protagonist.instance.holding == tool;
				});
				tool.onPlace.AddListener(_ => {
					tool.Unhold();
					HoldingTool = false;
				});
			}
			elements.mailBoxes = FindObjectsOfType<Mailbox>();
			sequencer.Reset();
		}
		#endregion
	}
}