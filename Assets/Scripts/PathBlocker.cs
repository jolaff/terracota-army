using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathBlocker : MonoBehaviour
{
	public float heightOffset = 10;
	public float defaultHeight;

	MeshRenderer meshRend;
	Animator animator;
	AudioSource audio;


	void Start()
    {
		transform.position = new Vector3(transform.position.x, defaultHeight + heightOffset, transform.position.z);

		meshRend = GetComponent<MeshRenderer>();
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
	}

    void Update()
    {
        if (!Application.isPlaying)
		{
			ChangeColor(Color.red);
		}
		else
		{
			ChangeColor(Color.grey);
		}
    }

	public void FallDown()
	{
		transform.position = new Vector3(transform.position.x, defaultHeight, transform.position.z);

		if (audio != null) audio.Play();
	}

	private void ChangeColor(Color color)
	{
		if (meshRend == null) meshRend = GetComponent<MeshRenderer>();

		if (meshRend.sharedMaterial.color != color)
		{
			meshRend.sharedMaterial.color = color;
		}
	}
}
