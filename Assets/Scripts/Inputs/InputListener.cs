using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

public class InputListener : MonoBehaviour, GameInputs.IPressActions
{
	public static InputListener Instance { get; private set; }
	
	private GameInputs m_Actions;                  // Source code representation of asset.
	private GameInputs.PressActions m_Player;     // Source code representation of action map

	private InteractableFruit _grabbedObject;

	private Vector2 _lastWorldPosition;

	[SerializeField] private PlayerInput _playerInput;


	private void MakeSingleton()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject); // Prevent duplicates
			return;
		}

		Instance = this;
	}

	void Awake()
	{
		MakeSingleton();

		m_Actions = new GameInputs();
		m_Player = m_Actions.Press;
		m_Player.AddCallbacks(this);
	}

	void OnDestroy()
	{
		m_Actions.Dispose();
	}
	
	void OnEnable()
	{
		m_Player.Enable();
	}
	
	void OnDisable()
	{
		m_Player.Disable();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (_grabbedObject != null)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				Vector2 moveInput = context.ReadValue<Vector2>();
				Vector2 worldPosition = Camera.main.ScreenToWorldPoint(moveInput);
				
				if (_lastWorldPosition == Vector2.zero)
				{
					_lastWorldPosition = worldPosition;
				}


				Vector3 newPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);

				_grabbedObject.Pushing(worldPosition, _lastWorldPosition);
				_grabbedObject.Move(newPosition);

				_lastWorldPosition = worldPosition;
			}
		}
	}

	public void OnPressAction(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{
			Vector2 screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
			Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

			RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
			if (hit.collider != null && hit.collider.TryGetComponent<InteractableFruit>(out var interactableFruit))
			{
				_grabbedObject = interactableFruit;
			}
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			if (_grabbedObject == null)
			{
				return;
			}

			_grabbedObject.Released();
			_grabbedObject = null;
		}
	}
}
