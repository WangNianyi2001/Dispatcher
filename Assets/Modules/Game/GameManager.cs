using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

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
			public ContentUI letterUI;
			public ContentUI promptUI;
		}
		public Elements elements;
		#endregion

		#region Private methods
		Mail dispatched, fetched;
		Envelope envelope;
		void DispatchMail() {
			if(fetched) {
				Prompt("A mail is already fetched. Skipping dispatching new mail.");
				return;
			}
			if(dispatched) {
				Prompt("A mail is already dispatched. Skipping dispatching new mail.");
				return;
			}
			Mail next = sequencer.Next();
			if(!next) {
				Prompt("No more mails can be dispatched.");
				return;
			}
			dispatched = next;
		}

		void GameOver() { }
		#endregion

		#region Public interfaces
		public void Prompt(string msg) {
			elements.promptUI.Text = msg;
		}

		public void AddReputation(Force force, float reputation) {
			if(force == Force.DontCare) {
				Prompt("Adding reputation to Force.DontCare, skipping adding reputation.");
				return;
			}
			var record = reputations.Find(r => r.force == force);
			record.reputation += reputation;
			if(reputation <= 0)
				GameOver();
		}

		public void FetchMail() {
			if(fetched) {
				Prompt("A mail is already fetched. Skipping fetching.");
				return;
			}
			if(!dispatched) {
				DispatchMail();
				if(!dispatched) {
					Prompt("No mail is dispatched, cannot fetch.");
					return;
				}
			}
			fetched = dispatched;
			dispatched = null;

			GameObject envelope = Instantiate(resources.envelopePrefab, elements.envelopeAnchor.transform);
			this.envelope = envelope.GetComponentInChildren<Envelope>();
			this.envelope.target.sources.AddRange(elements.tools);
			this.envelope.Sealing = fetched.sealing;

			Prompt($"Fetched {fetched}");
		}
		public void PostMail(Force force) {
			if(!fetched) {
				Prompt("No mail is fetched, cannot post.");
				return;
			}
			fetched = null;
			Destroy(envelope.gameObject);
			envelope = null;
			Prompt($"Posted mail to {force}");
		}
		public void PostMail(string forceName) => PostMail((Force)Enum.Parse(typeof(Force), forceName));

		public void CutOpenEnvelope() {
			if(!envelope || envelope.Open == true)
				return;
			envelope.Open = true;
			envelope.Sealing = SealingType.None;
			Prompt("Cut open envelope.");
		}
		public void GlueEnvelope() {
			if(!envelope)
				return;
			envelope.Open = false;
			envelope.Sealing = SealingType.Glue;
			Prompt("Glued envelope.");
		}
		public void WaxEnvelope() {
			if(!envelope)
				return;
			envelope.Open = false;
			envelope.Sealing = SealingType.Wax;
			Prompt("Wax-sealed envelope.");
		}

		public void EnterUI() {
			Protagonist.instance.Input = false;
		}
		public void QuitUI() {
			Protagonist.instance.Input = true;
		}
		public void Inspect() {
			if(envelope.Sealing == SealingType.None)
				elements.letterUI.Show(fetched.content);
		}

		public bool HoldingTool {
			set {
				if(!envelope)
					return;
				envelope.target.Active = value;
				envelope.instant.Active = !value;
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