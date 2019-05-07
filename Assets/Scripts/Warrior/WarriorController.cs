﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State { standing, waiting, lurking, wandering, alerted, chasing, killing, dying };

public class WarriorController : MonoBehaviour
{
	public State state;
	public float wanderingRange;
	public float wanderingSpeed;
	public float alertedSpeed;
	public float chasingSpeed;

	Animator animator;
	GameObject player;
	PlayerController playerController;
	WarriorTriggers triggers;
	NavMeshAgent agent;
	

	void Start()
	{
		triggers = GetComponent<WarriorTriggers>();
		agent = GetComponent<NavMeshAgent>();
		animator = transform.GetComponentInChildren<Animator>();
		player = GameObject.Find("Player");
		playerController = player.GetComponent<PlayerController>();
	}

	public void ExecutePlayer()
	{
		state = State.killing;
		playerController.Die();
	}

    void Update()
    {
		agent.speed = state == State.wandering ? wanderingSpeed : state == State.alerted ? alertedSpeed : state == State.chasing ? chasingSpeed : 0;

		if (state == State.chasing && triggers.CanSee(player))
		{
			agent.SetDestination(player.transform.position);
		}
		else if (!playerController.IsWalking && triggers.DistanceFrom(player) <= triggers.hearingDistance && (state == State.wandering || state == State.chasing || state == State.alerted))
		{
			if (state == State.wandering)
				state = State.alerted;

			agent.SetDestination(player.transform.position);
		}
		else if (state == State.wandering && ReachedDestination())
		{
			agent.SetDestination(RandomNavmeshLocation(wanderingRange));
		}

		animator.SetFloat("Speed", agent.velocity.magnitude);
	}


	public Vector3 RandomNavmeshLocation(float radius)
	{
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += transform.position;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
		{
			finalPosition = hit.position;
		}
		return finalPosition;
	}

	public bool ReachedDestination()
	{
		return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
	}

}
