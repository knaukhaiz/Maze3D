using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
	[SerializeField] private FixedJoystick _fixedJoystick;
	public CharacterController controller;
	void Start()
    {
        
    }

	void LateUpdate()
	{
		Vector3 movement = new Vector3(_fixedJoystick.Horizontal, 0f, _fixedJoystick.Vertical);
		controller.Move(movement * speed * Time.deltaTime);
	}
}
