using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game {
	public class GameManager : MonoBehaviour {
		[Serializable] public struct Elements {
			public InstantElement door;
			public InstantElement bed;
			public Transform envelopeDesktopAnchor;
		}
		public Elements elements;

		#region Mail sequence
		static IEnumerator<Mail> MakeMailSequence() {
			yield return null;
		}

		IEnumerator<Mail> mailEnumerator;
		bool done = false;

		Mail NextMail() {
			if(done)
				return null;
			done = !mailEnumerator.MoveNext();
			return mailEnumerator.Current;
		}
		#endregion

		#region Pubic interface
		public void DispatchNextMail() {
			Mail next = NextMail();
			if(!next)
				return;
		}
		#endregion

		#region Lyfe cycle
		void Start() {
			mailEnumerator = MakeMailSequence();
		}
		#endregion
	}
}