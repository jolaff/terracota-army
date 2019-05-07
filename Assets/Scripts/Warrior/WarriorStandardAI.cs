using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStandardAI : MonoBehaviour
{
	 
	[Tooltip("time needed to awake")]
	public float timeToAwake;
	[Space(10)]
	[Tooltip("time needed to loose intrest, after the player isnt visible")]
	public float timeToLooseIntrest; 
	[Tooltip("distance needed to execute the player")]
	public float killingDistance;
	[Space(10)]
	[Tooltip("lurk or start wandering immediately?")]
	public bool lurk = false; 
	[Tooltip("when reached, warrior will start chasing the player")]
	public float minLurkingDistance; 

	GameObject player;
	PlayerController playerController;
	WarriorController controller;
	WarriorTriggers triggers;
	float timeOfAwaking;
	float timeOfIntrestLoss;
	Vector3 checkingPosition;
	//MeshRenderer rend;


	void Start()
    {
		controller = GetComponent<WarriorController>();
		triggers = GetComponent<WarriorTriggers>();
		player = player = GameObject.Find("Player");
		playerController = player.GetComponent<PlayerController>();
		//rend = transform.GetChild(0).GetComponent<MeshRenderer>();

		controller.state = State.standing;
    }

    void Update()
    {
		float saturation = 0f;

		if (controller.state == State.standing) saturation = 1f;
		else if (controller.state == State.waiting) saturation = (timeOfAwaking - Time.time) / timeToAwake;

		//rend.material.color = new Color(saturation, 0, 0);

		if (controller.state == State.standing && triggers.DistanceFrom(player) <= triggers.sightDistance && triggers.CanFeel(player))
		{
			controller.state = State.waiting;
			timeOfAwaking = Time.time + timeToAwake;
		}

		if (controller.state == State.waiting && Time.time >= timeOfAwaking)
		{
			//make smarter later
			controller.state = lurk ? State.lurking : State.wandering;
		}

		if (controller.state == State.lurking)
		{
			float distance = triggers.DistanceFrom(player);

			if (distance <= minLurkingDistance && triggers.CanSee(player))
			{
				controller.state = State.chasing;
			}
		}

		if ((controller.state == State.wandering || controller.state == State.alerted || controller.state == State.chasing) && triggers.CanSee(player))
		{
			controller.state = State.chasing;
			timeOfIntrestLoss = Time.time + timeToLooseIntrest;
		}

		if (controller.state == State.alerted && controller.ReachedDestination())
		{
			controller.state = State.wandering;
		}

		/*if ((controller.state == State.wandering || controller.state == State.alerted) && heardSomething)
		{
			controller.state = State.alerted;
			checkingPosition = heardPosition;
		}

		if (controller.state == State.alerted && triggers.CanSee(heardPosition))
		{
			controller.state = State.wandering;
		}*/

		if (controller.state == State.chasing)
		{
			if (Time.time >= timeOfIntrestLoss && controller.ReachedDestination())
				controller.state = State.wandering;

			if (triggers.DistanceFrom(player) <= killingDistance && triggers.CanSee(player))
				controller.ExecutePlayer();
		}
	}
}
