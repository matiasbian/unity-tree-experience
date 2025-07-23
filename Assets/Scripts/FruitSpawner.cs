using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private Apple fruit;
	[SerializeField] private GameObject leaf;

    [SerializeField] private float spawnInterval = 3f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        InstantiateANewFruit();
	}

    private void OnFruitGrabbed(Apple fruit)
    {
        Invoke(nameof(InstantiateANewFruit), spawnInterval);
        fruit.OnGrabbed -= OnFruitGrabbed;
		var instancedLeaf = Instantiate(leaf, transform.position, Quaternion.identity);
        Destroy(instancedLeaf, 1);
	}

    private void InstantiateANewFruit()
    {
		var fruitInstance = Instantiate(fruit, transform.position, Quaternion.identity);
		fruitInstance.OnGrabbed += OnFruitGrabbed;
	}
}
