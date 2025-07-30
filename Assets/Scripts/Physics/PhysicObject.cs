using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour
{
	private float _accelerationAcumulated = 0f;
	private float _accelerationDivider = 0f;
    private float _maxAcceleration = -10000f; // Maximum acceleration limit

	private float _remainingBounceTime = 0f; // Remaining bounce time

	[SerializeField] private float BounceForce = 5f; // Force applied during bounce
	[SerializeField] private float BounceDelay = 0.2f; // Delay before bouncing again

	[SerializeField] private float Weight = 1f; // Default weight
	[SerializeField] private float _torqueMultiplier = 10f; 
    [SerializeField] private CircleCollider2D _collider; // Collider to check for collisions

	private float angularVelocity = 0f;
	private float torque = 0f;
	public float angularDamping = 0.48f;

	private bool _isColliding = false;

	public void SetTorque(float dir)
	{
		torque = dir * _torqueMultiplier; // Set torque based on direction
	}

	public void ApplyGravity(float gravity)
    {
        if (_isColliding)
        {
            return; // Skip gravity application if colliding
		}

		if (_accelerationDivider == 0)
        {
            _accelerationDivider = -gravity * 55f;
        }

        _accelerationAcumulated += gravity;

        if (_accelerationAcumulated < _maxAcceleration)
        {
            _accelerationAcumulated = _maxAcceleration; // Clamp to max acceleration
        }
        
        Vector2 newPosition = transform.position + Vector3.up * ((_accelerationAcumulated / _accelerationDivider * Weight) + _remainingBounceTime) * Time.deltaTime;
        transform.position = newPosition;

		if (_remainingBounceTime > 0f)
		{
			_remainingBounceTime -= Time.deltaTime; // Decrease bounce time
			if (_remainingBounceTime <= 0f)
			{
				_remainingBounceTime = 0f; // Reset bounce time
			}
		}

		UpdateRotation();
	}

	private void CollisionEnter(Collider2D collision)
	{
		if (BounceForce <= 0f)
		{
			_isColliding = true; // Set colliding state
			return; // Skip if already bouncing
		}

		_remainingBounceTime = BounceForce; // Reset bounce time
		_accelerationAcumulated = 0f; // Reset accumulated acceleration

		if (BounceForce - BounceDelay <= 0f)
		{
			BounceForce = 0f; // Reset bounce force if it goes below zero
			return;
		}
		BounceForce -= BounceDelay;
	}

	private void Update()
	{
		Collider2D[] results = Physics2D.OverlapCircleAll((Vector2)transform.position + _collider.offset, _collider.radius);
		foreach (var col in results)
		{
			if (col.transform == transform || col.transform.CompareTag(transform.tag))
			{
				continue; // Skip self & other apples
			}

			CollisionEnter(col);
		}
	}

	private void UpdateRotation()
	{
		// Apply torque to angular velocity
		angularVelocity += torque * Time.deltaTime;

		// Apply rotation
		transform.eulerAngles += new Vector3(0, 0, angularVelocity);

		// Dampen the angular velocity over time (simulate friction)
		angularVelocity *= angularDamping;

		// Reset torque after applying it this frame
		torque = 0f;
	}
}
