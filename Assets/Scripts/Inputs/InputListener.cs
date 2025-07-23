using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

public class InputListener : MonoBehaviour, GameInputs.IPressActions
{
	private GameInputs m_Actions;                  // Source code representation of asset.
	private GameInputs.PressActions m_Player;     // Source code representation of action map
	void Awake()
	{
		m_Actions = new GameInputs();
		m_Player = m_Actions.Press;
		m_Player.AddCallbacks(this);
	}

	public void OnNewaction(InputAction.CallbackContext context)
	{
		Debug.Log("New action triggered: " + context.action.name);
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
}
