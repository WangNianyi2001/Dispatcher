using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Rendering.FilterWindow;

namespace Game {
	[RequireComponent(typeof(PlayerInput))]
	public class Protagonist : Player {
		#region Instantiating
		public static Protagonist instance;
		public Protagonist() {
			instance = this;
		}
		#endregion

		#region Movements
		#region Private fields
		new Camera camera;

		Vector3 inputVelocity = Vector3.zero;
		Vector2 inputRotation = Vector2.zero;
		#endregion

		#region Movement overrides
		public override void Rotate(Vector2 rotation) {
			base.Rotate(rotation);

			Vector3 cam = camera.transform.rotation.eulerAngles;
			cam.x += rotation.y;
			if(cam.x >= 180)
				cam.x -= 360;
			cam.x = Mathf.Clamp(cam.x, pitchRange.x, pitchRange.y);
			camera.transform.rotation = Quaternion.Euler(cam);
		}
		#endregion

		#region Input handlers
		public void OnMove(InputValue value) {
			inputVelocity = value.Get<Vector2>();
			inputVelocity.z = inputVelocity.y;
			inputVelocity.y = 0;
		}

		public void OnOrient(InputValue value) {
			inputRotation = value.Get<Vector2>();
		}
		#endregion
		#endregion

		#region Interaction
		[Header("Interaction")]
		public Transform holdAnchor;
		[NonSerialized] public HoldableElement holding;
		struct LousyTransform {
			public Transform parent;
			public Vector3 position;
			public Quaternion rotation;
		}
		LousyTransform original;

		public void Interact(Element element) {
			if(holding) {
				Unhold();
				return;
			}
			if(element is HoldableElement)
				Hold(element as HoldableElement);
		}

		public void Hold(HoldableElement holdable) {
			if(holding)
				return;
			original = new LousyTransform {
				parent = holdable.transform.parent,
				position = holdable.transform.localPosition,
				rotation = holdable.transform.localRotation,
			};
			holding = holdable;
			holding.Active = false;
			holding.transform.parent = holdAnchor;
			holding.transform.localPosition = Vector3.zero;
			holding.transform.localRotation = original.rotation;
		}

		public void Unhold() {
			if(!holding)
				return;
			holding.transform.parent = original.parent;
			holding.transform.localPosition = original.position;
			holding.transform.localRotation = original.rotation;
			holding.Active = true;
			holding = null;
		}

		public void OnInteract() {
			CameraSelector selector = GetComponentInChildren<CameraSelector>();
			if(selector == null)
				return;
			selector.Use();
		}
		#endregion

		#region Life cycle
		new void Start() {
			base.Start();

			camera = GetComponentInChildren<Camera>();
			camera.tag = "MainCamera";

			Cursor.lockState = CursorLockMode.Locked;
		}

		void FixedUpdate() {
			Vector3 velocity = inputVelocity * movementSpeed * Time.fixedDeltaTime;
			velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
			Move(velocity);

			Vector2 rotation = inputRotation * orientingSpeed * Time.fixedDeltaTime;
			rotation.y = -rotation.y;
			Rotate(rotation);
		}
		#endregion
	}
}