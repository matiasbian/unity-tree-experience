using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindFirstObjectByType<T>();

				if (_instance == null)
				{
					Debug.Log("Didn't found an instance for " + typeof(T));
					_instance = new GameObject().AddComponent<T>();
					_instance.name = typeof(T).Name;
				}
			}

			return _instance;
		}
	}
}
