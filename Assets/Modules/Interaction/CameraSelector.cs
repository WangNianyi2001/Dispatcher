using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	public class CameraSelector : MonoBehaviour {
		public new Camera camera;
		public float maxDistance = 10;
		public InputAction useInput;

		[NonSerialized] public List<Usable> lastSelected;

		public void Use() {
			foreach(Usable usable in lastSelected)
				usable.BroadcastMessage("OnUse", this, SendMessageOptions.DontRequireReceiver);
		}

		public void Start() {
			if(camera == null)
				camera = GetComponent<Camera>();
			if(camera == null)
				Debug.LogWarning("Camera selector has no target camera");
			lastSelected = new List<Usable>();
			useInput.performed += (InputAction.CallbackContext _) => Use();
			useInput.Enable();
		}

		public void Update() {
			if(camera == null)
				return;
			Ray ray = camera.ScreenPointToRay(new Vector2(camera.pixelWidth, camera.pixelHeight) / 2);
			var hits = Physics.RaycastAll(ray, maxDistance);
			var currentSelected = hits
				.Select((RaycastHit hit) => hit.collider.GetComponent<Usable>())
				.Where((Usable usable) => usable != null)
				.ToList();
			currentSelected.ForEach((Usable usable) => {
				if(!lastSelected.Contains(usable))
					usable.OnSelect(this);
			});
			lastSelected.ForEach((Usable usable) => {
				if(!currentSelected.Contains(usable))
					usable.OnDeselect(this);
			});
			lastSelected = currentSelected;
		}
	}
}