using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
	public float update;

	Text text;
	int counter;
	float deltaTime;

    void Start()
    {
		text = GetComponent<Text>();
    }

    void Update()
    {
		counter++;
		deltaTime += Time.unscaledDeltaTime;

		if (deltaTime >= update)
		{
			text.text = Mathf.Round(counter / deltaTime).ToString();
			counter = 0;
			deltaTime = 0;
		}

	}
}
