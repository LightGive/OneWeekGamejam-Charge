using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] UnityEngine.InputSystem.PlayerInput _input = null;
	[SerializeField, Range(0.0f, 1.0f)] float _stickAngleThresholdStart = 0.5f;
	[SerializeField, Range(0.0f, 1.0f)] float _stickAngleThresholdEnd = 0.2f;

	Vector2 _screenCenter = Vector2.zero;
	bool _isAimStart = false;
	public Vector3 VecMove { get; private set; } = Vector3.zero;
	public Vector3 VecDir { get; private set; } = Vector3.zero;
	public UnityEvent OnAimStart { get; private set; } = new UnityEvent();
	public UnityEvent OnFire { get; private set; } = new UnityEvent();

	void Awake()
	{
		_screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
	}

	void Start()
	{

	}

	void Update()
	{

	}
	void OnEnable()
	{
		_input.actions["Move"].performed += OnMove;
		_input.actions["Move"].canceled += OnMove;
		_input.actions["AimStickFire"].performed += OnAimStick;
		_input.actions["AimStickFire"].canceled += OnAimStick;
		_input.actions["AimMouse"].performed += OnAimMouse;
		_input.actions["FireMouse"].performed += OnMouse;
	}

	void OnDisable()
	{
		_input.actions["Move"].performed -= OnMove;
		_input.actions["Move"].canceled -= OnMove;
		_input.actions["AimStickFire"].performed -= OnAimStick;
		_input.actions["AimStickFire"].canceled -= OnAimStick;
		_input.actions["AimMouse"].performed -= OnAimMouse;
		_input.actions["FireMouse"].performed -= OnMouse;
	}

	void OnMove(InputAction.CallbackContext context)
	{
		VecMove = context.ReadValue<Vector2>();
	}

	void OnAimStick(InputAction.CallbackContext context)
	{
		var stickVec = context.ReadValue<Vector2>();
		if (float.IsNaN(stickVec.x) || float.IsNaN(stickVec.y))
		{
			Debug.Log("StickVec is NaN");
			return;
		}
		var stickMag = stickVec.magnitude;
		if (!_isAimStart && stickVec.magnitude >= _stickAngleThresholdStart)
		{
			AimStart();
		}
		else if (_isAimStart && stickMag <= _stickAngleThresholdEnd)
		{
			Fire();
		}
		if (_isAimStart)
		{
			VecDir = -stickVec;
		}
	}

	void OnAimMouse(InputAction.CallbackContext context)
	{
		if (_isAimStart)
		{
			var screenPoint = context.ReadValue<Vector2>();
			var vec = (screenPoint - _screenCenter).normalized;
			VecDir = vec;
		}
	}

	void OnMouse(InputAction.CallbackContext context)
	{
		if (!_isAimStart && context.action.IsPressed())
		{
			AimStart();
		}
		else if (_isAimStart)
		{
			Fire();
		}
	}

	void AimStart()
	{
		_isAimStart = true;
		OnAimStart?.Invoke();
	}

	void Fire()
	{
		_isAimStart = false;
		OnFire?.Invoke();
	}
}
