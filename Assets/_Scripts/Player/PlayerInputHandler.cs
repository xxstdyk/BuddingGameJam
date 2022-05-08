using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
	// inputs
	[Header("Input Axis")]
	[SerializeField] private InputAction _axisAction;
	[Header("Interact")]
	[SerializeField] private InputAction _interactAction;

	// bitflag for currently pressed inputs
	[System.Flags]
	private enum InputState : short
	{
		NONE = 0b0,
		INTERACT = 0b1,
	}

	private Player _player;

	private InputState _inputState;
	private Vector2 _movementInput;

	private void Start()
	{
		_player = GetComponent<Player>();

		// clear inputs
		_inputState = 0;
		_movementInput = Vector2.zero;
	}

	// read inputs
	private void Update()
	{
		if (_interactAction.WasPerformedThisFrame())
			_inputState |= InputState.INTERACT;

		if (Keyboard.current.fKey.wasPressedThisFrame)
			FindObjectOfType<CinemachineStateSwitcher>().SwitchState(CinemachineStateSwitcher.State.FISHING_CAMERA);

		if (Keyboard.current.pKey.wasPressedThisFrame)
			FindObjectOfType<CinemachineStateSwitcher>().SwitchState(CinemachineStateSwitcher.State.PLAYER_CAMERA);

		_movementInput = _axisAction.ReadValue<Vector2>();
	}

	// process inputs
	private void FixedUpdate()
	{
		if (_inputState.HasFlag(InputState.INTERACT))
			_player.Interact();

		_player.DoMove(_movementInput);

		// clear inputs
		_inputState = 0;
		_movementInput = Vector2.zero;
	}

	private void OnEnable()
	{
		_axisAction.Enable();
		_interactAction.Enable();
	}

	private void OnDisable()
	{
		_axisAction.Disable();
		_interactAction.Disable();
	}

}