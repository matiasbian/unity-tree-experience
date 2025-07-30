using System;
using UnityEngine;

public class Apple : MonoBehaviour, InteractableFruit
{
	[SerializeField] private Animator _animator;
	[SerializeField] private PhysicObject _physicObject;

	[SerializeField] private float stretchFactor = 0.1f;
	[SerializeField] private float maxStretch = 3f;
	[SerializeField] private float returnShapeForce = 2f;
	[SerializeField] private float releaseDistance = 3f;

	public Action<Apple> OnGrabbed;

	private Vector2 originalScale;
	private Vector2 originalPosition;

	private bool _grabbed;
	private bool _released;

	private void Start()
	{
		originalScale = transform.localScale;
		originalPosition = transform.position;
	}

	public void Grabbed()
	{
		_animator.SetTrigger("Grabbed");
		_grabbed = true;
		OnGrabbed?.Invoke(this);
	}

	public void Pushing(Vector2 worldPos, Vector2 lastWorldPos)
	{
		if (Vector2.Distance(worldPos, transform.position) > releaseDistance)
		{
			Grabbed();
		}

		else
		{
			_released = false;
			float deltaX = worldPos.x - transform.position.x;

			float scaleChange = Mathf.Clamp(deltaX * stretchFactor, -originalScale.x * 0.5f, originalScale.x * maxStretch);
			float newScaleX = originalScale.x + scaleChange;

			transform.localScale = new Vector2(newScaleX, originalScale.y);

			transform.position = originalPosition + new Vector2(scaleChange * 0.5f, 0f);
		}
	}

	public void Released(Vector2 dragDir)
	{
		_released = true;
		if (!_grabbed)
		{
			return;
		}

		_animator.SetTrigger("Released");

		Gravity.Instance.Subscribe(_physicObject);
		_physicObject.SetTorque(dragDir.x);
	}

	private void Update()
	{
		if (_released)
		{
			float x = Mathf.Lerp(transform.localScale.x, originalScale.x, returnShapeForce * Time.deltaTime);
			transform.localScale = new Vector2(x, originalScale.y);
		}
	}

	public void Move(Vector2 newPosition)
	{
		if (!_grabbed)
		{
			return;
		}

		transform.position = newPosition;
	}
}
