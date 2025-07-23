using UnityEngine;

public interface InteractableFruit
{
	public void Grabbed();
	public void Released();
	public void Pushing(Vector2 worldPos, Vector2 lastWorldPos);
	public void Move(Vector2 newPosition);
}
