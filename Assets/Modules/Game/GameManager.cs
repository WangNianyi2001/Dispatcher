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
			public ContentUI letterUI, promptUI, newsUI;
			public ReputationUI reputationUI;
		}
		public Elements elements;
		#endregion

		#region Private methods
		Mail dispatched, fetched;
		Envelope envelope;
		void DispatchMail() {
			if(fetched)
				return;
			if(dispatched)
				return;
			Mail next = sequencer.Next();
			if(!next)
				return;
			dispatched = next;
		}

		void GameOver() { }
		#endregion

		#region Public interfaces
		public void Prompt(string msg) {
			elements.promptUI.Text = msg;
		}

		public void AddReputation(Force force, float reputation) {
			if(force == Force.DontCare)
				return;
			var record = reputations.Find(r => r.force == force);
			record.reputation += reputation;
			elements.reputationUI.Set(record.force, record.reputation);
			if(reputation <= 0)
				GameOver();
		}
		public void UpdateReputation() {
			foreach(var f in new Force[] { Force.Gang, Force.Capital, Force.Police, Force.Worker })
				AddReputation(f, 0);
		}

		public void FetchMail() {
			if(fetched) {
				Prompt("请先投递拿到的信件。");
				return;
			}
			if(!dispatched) {
				DispatchMail();
				if(!dispatched) {
					Prompt("没有更多信件了。");
					return;
				}
			}
			fetched = dispatched;
			dispatched = null;

			GameObject envelope = Instantiate(resources.envelopePrefab, elements.envelopeAnchor.transform);
			this.envelope = envelope.GetComponentInChildren<Envelope>();
			this.envelope.target.sources.AddRange(elements.tools);
			this.envelope.Sealing = fetched.sealing;

			Prompt($"拿到了新信件。");
		}
		public void PostMail(Force force) {
			if(!fetched) {
				Prompt("没拿到信件，无法投递。");
				return;
			}

			// Reputation
			var rep = fetched.responsePairs.Find(r => r.sendTo == force) ?? fetched.responsePairs.Find(r => r.sendTo == Force.DontCare);
			if(rep != null) {
				foreach(var res in rep.responses)
					AddReputation(res.effectOn, res.reputation);
			}

			fetched = null;
			Destroy(envelope.gameObject);
			envelope = null;

			Prompt($"投递到{Constant.forceNames[force]}");
		}
		public void PostMail(string forceName) => PostMail((Force)Enum.Parse(typeof(Force), forceName));

		public void CutOpenEnvelope() {
			if(!envelope || envelope.Open == true)
				return;
			envelope.Open = true;
			envelope.Sealing = SealingType.None;
			Prompt("用刀打开了信封。");
		}
		public void GlueEnvelope() {
			if(!envelope)
				return;
			envelope.Open = false;
			envelope.Sealing = SealingType.Glue;
			Prompt("用胶水粘上了信封。");
		}
		public void WaxEnvelope() {
			if(!envelope)
				return;
			envelope.Open = false;
			envelope.Sealing = SealingType.Wax;
			Prompt("用火漆封上了信封。");
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
			UpdateReputation();
		}
		#endregion
	}
}