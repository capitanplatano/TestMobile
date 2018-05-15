using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	public GameObject shield;
	// Use this for initialization
	private bool paused = false;

	public float moveSpeed = 1f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if(!paused)
			transform.position = new Vector2(transform.position.x, transform.position.y - (moveSpeed * Time.deltaTime));	
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			switch(transform.tag)
			{
				case "CollectibleHealth":
					sumHealth(other.GetComponent<CubeController>(), 1);
				break;
				case "CollectibleBullet":
					sumBullets(other.GetComponent<CubeController>(), 1);
				break;
				case "CollectibleAttack":
					sumDamage(other.GetComponent<CubeController>(), 1);
				break;
			}
		}
	}

	void sumHealth(CubeController cube, int num)
	{
		//Sound effect
		if(cube.currentHealth + num <= cube.maxHealth)
			cube.currentHealth = cube.currentHealth + num;
	}

	void sumDamage(CubeController cube, int num)
	{
		//Sound effect
		if(cube.bulletDamage + num <= cube.maxBulletDamage)
			cube.bulletDamage = cube.bulletDamage + num;
	}

	void sumBullets(CubeController cube, int num)
	{
		//Sound effect
		cube.maxBullets = cube.maxBullets + num;
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
