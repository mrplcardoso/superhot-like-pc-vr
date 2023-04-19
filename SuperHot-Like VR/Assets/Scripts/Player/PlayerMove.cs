using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Audio;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
	PlayerObject playerObject;
	CharacterController controller;

	Vector3 xVelocity, yVelocity, zVelocity;
	Vector3 velocity { get { return xVelocity + yVelocity + zVelocity; } }

	float walkSpeed = 3f;
	float sprintSpeed => walkSpeed * 1.5f;
	float cachedSpeed;
	float jumpForce = 4.5f;
	float gravity = 10f;

	private void Awake()
	{
		playerObject = GetComponent<PlayerObject>();
		controller = GetComponent<CharacterController>();
	}

	private void Start()
	{
		playerObject.FrameAction += FrameUpdate;
	}

	private void FrameUpdate()
	{
		MoveInput();

		if (velocity.magnitude == 0)
		{ playerObject.moveAction = false; }
		else 
		{ playerObject.moveAction = true; AudioHub.instance.PlayOneTime(AudioList.Steps); }

		controller.Move(velocity * Time.deltaTime);
	}

	void MoveInput()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{ cachedSpeed = sprintSpeed; }
		else
		{ cachedSpeed = walkSpeed; }

		xVelocity = transform.right *
			Input.GetAxisRaw("Horizontal") * cachedSpeed;
		zVelocity = transform.forward *
			Input.GetAxisRaw("Vertical") * cachedSpeed;
	}

	void JumpInput()
	{
		//animator.SetBool("grounded", controller.isGrounded);

		//Aplica a gravidade no personagem
		yVelocity += Vector3.down * gravity * Time.deltaTime;

		//Se ele estiver no ch?o, n?o ? necessario
		//aplicar a gravidade
		if (controller.isGrounded)
		{ yVelocity = Vector3.down; }

		//Aplica o pulo
		if (controller.isGrounded &&
			Input.GetAxisRaw("Jump") >= 1)
		{
			yVelocity = Vector3.up * jumpForce;
		}
	}

	private void OnDisable()
	{
		playerObject.FrameAction -= FrameUpdate;
	}
}
