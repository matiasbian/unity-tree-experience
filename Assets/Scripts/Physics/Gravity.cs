using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoSingleton<Gravity>
{
    [SerializeField] private float _gravityForce = -9.81f; // Default gravity force
    [SerializeField] private Collider2D _floorCollider; // Collider for the floor

	private List<PhysicObject> _subscribers = new List<PhysicObject>();

    public void Subscribe(PhysicObject physicObject)
    {
        if (physicObject != null && !_subscribers.Contains(physicObject))
        {
            _subscribers.Add(physicObject);
        }
	}

	public void ApplyGravity(Transform subscriberTransform)
    {
        if (subscriberTransform != null)
        {
            Vector2 gravity = new Vector2(0, _gravityForce * Time.deltaTime);
			subscriberTransform.position += Vector3.up * _gravityForce * Time.deltaTime;
        }
	}

    public Collider2D GetFloorCollider()
    {
        return _floorCollider;
	}

	private void Update()
	{
        foreach (PhysicObject subscriber in _subscribers)
        {
            subscriber.ApplyGravity(_gravityForce);
		}
	}
}
