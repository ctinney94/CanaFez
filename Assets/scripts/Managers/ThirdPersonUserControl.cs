using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(PlayerCharacter))]
	public class ThirdPersonUserControl : MonoBehaviour
	{
		private PlayerCharacter m_Character;
		// A reference to the ThirdPersonCharacter on the object
		private Transform m_Cam;
		// A reference to the main camera in the scenes transform
		private Vector3 m_CamForward;
		// The current forward direction of the camera
		private Vector3 m_Move;
		// the world-relative desired move direction, calculated from the camForward and user input.
		        
		private void Start ()
		{
			// get the transform of the main camera
			if (Camera.main != null) {
				m_Cam = Camera.main.transform;
			}
			// get the third person character ( this should never be null due to require component )
			m_Character = GetComponent<PlayerCharacter>();
		}

		// Fixed update is called in sync with physics
		private void FixedUpdate ()
		{
			if (GameStateManager.instance.GetState() == GameStateManager.GameStates.STATE_GAMEPLAY) {			// read inputs
				float h = Input.GetAxis("Horizontal");
				//bool crouch = Input.GetKey(KeyCode.C);

				// calculate move direction to pass to character
				if (m_Cam != null) {
					// calculate camera relative direction to move:
					m_CamForward = Vector3.Scale(m_Cam.forward,new Vector3 (1, 0, 1)).normalized;
					m_Move = h * m_Cam.right;
				}
				else {
					// we use world-relative directions in the case of no main camera
					m_Move = h * Vector3.right;
				}
#if !MOBILE_INPUT
				// walk speed multiplier
				if (Input.GetKey(KeyCode.LeftShift))
					m_Move *= 0.5f;
#endif

				// pass all parameters to the character control script
				m_Character.Move(m_Move,false);
			}
		}

	}
}
