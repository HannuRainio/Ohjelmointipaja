using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof (ThirdPersonCharacter))]
	[RequireComponent(typeof (AICharacterControl))]
	public class MoveTest : MonoBehaviour
	{
		public AICharacterControl character { get; private set; } // the character we are controlling
		// Start is called before the first frame update
		void Start()
		{
		    character = GetComponent<AICharacterControl>();
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetMouseButton(0)) {
				HandleInput();
			}
		}
		
		void HandleInput () {
			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(inputRay, out hit)) {
				moveTo(hit.point);
			}
		}
		void moveTo (Vector3 position) {
			position = transform.InverseTransformPoint(position);
			Debug.Log(position);
		}
	}
}
