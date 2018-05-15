using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public int bulletSpeed = 15;
	public int bulletDamage = 1;
	private float lifetime; 

	private bool paused = false;
	void Start () {
		lifetime = Time.time;
	}
	
	void Update () {
		if(!paused)
		{
			transform.position = new Vector2(transform.position.x, transform.position.y + (bulletSpeed * Time.deltaTime));
			if((lifetime + 5) < Time.time)
				Destroy(gameObject);
		}
	}
	void OnPauseGame ()
	{
		paused = true;
	}


	void OnResumeGame ()
	{
		paused = false;
	}
}
