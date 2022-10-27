using TMPro;
using UnityEngine;

namespace Game {
	public class ContentUI : MonoBehaviour {
		public TMP_Text text;

		public string Text {
			set => text.text = value;
		}

		public void Show(string text) {
			Text = text;
			gameObject.SetActive(true);
			GameManager.instance.EnterUI();
		}

		public void Quit() {
			gameObject.SetActive(false);
			GameManager.instance.QuitUI();
		}
	}
}