using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTriggers : MonoBehaviour
{
	public float sightDistance; //sightDistance
	public float hearingDistance; //hearDistance
	[Range(0, 360)]
	public float FOV;
	public LayerMask blockingMask;

	Vector3 pos, rot;

	public bool CanFeel(Vector3 target) //when the path between warrior and the target is not blocked
	{
		Vector3 position = transform.position;
		Vector3 direction = (target - position).normalized;
		float distance = Vector3.Distance(position, target);

		bool blocking = Physics.Raycast(transform.position, direction, distance, blockingMask);

		return !blocking;
	}

	public bool CanFeel(GameObject target)
	{
		return CanFeel(target.transform.position);
	}

	public bool CanSee(Vector3 target) //when the path between warrior and the target is not blocked, and target is in the cone of sight
	{
		if (DistanceFrom(target) <= sightDistance)
		{
			Vector3 direction = target - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			if (angle <= FOV / 2 && CanFeel(target))
			{
				return true;
			}
		}

		return false;
	}

	public bool CanSee(GameObject target)
	{
		return CanSee(target.transform.position);
	}

	public float DistanceFrom(Vector3 target)
	{
		return Vector3.Distance(transform.position, target);
	}

	public float DistanceFrom(GameObject target)
	{
		return DistanceFrom(target.transform.position);
	}
	
	void OnDrawGizmosSelected()
	{
		float totalFOV = FOV;

		float rayRange = sightDistance / Mathf.Cos(FOV / 2 * Mathf.Deg2Rad);

		float halfFOV = totalFOV / 2.0f;
		Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
		Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );
		Vector3 leftRayDirection = leftRayRotation * transform.forward;
		Vector3 rightRayDirection = rightRayRotation * transform.forward;
		Gizmos.DrawRay( transform.position, leftRayDirection * rayRange );
		Gizmos.DrawRay( transform.position, rightRayDirection * rayRange );

	}
}
