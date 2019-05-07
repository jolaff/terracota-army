using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


public class AreaTrigger : MonoBehaviour
{
	public UnityEvent triggers;
	public bool oneTime;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			triggers.Invoke();
		}

		if (oneTime)
		{
			Destroy(gameObject);
		}
	}
}
