using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private float moveSpeed = 1f;
	private int maxHealth = 1;
	private int currentHealth;
	private GameObject player;

	private GameObject gameController;

	private int damage = 1;
	private bool paused = false;

	void Start () {
		currentHealth = maxHealth;
		player = GameObject.Find("Player");
		gameController = GameObject.Find("GameController");
	}
	
	// Update is called once per frame
	void Update () {	

	}

	void FixedUpdate() {
		if(!paused)
			transform.position = new Vector2(transform.position.x, transform.position.y - (moveSpeed * Time.deltaTime));	
	}
	public void receiveDamage(int damage)
	{
		currentHealth -= damage;
		if(currentHealth <= 0)
			Die();
	}

	public void Die()
	{
		//gameObject.GetComponent<Animator>().SetBool("Dead", true);
		gameController.GetComponent<GameController>().sumPoints(10);
		Destroy(gameObject);
		
		//Sound effect
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Bullet")
		{
			receiveDamage(other.gameObject.GetComponent<Bullet>().bulletDamage);
			Destroy(other.gameObject);
		}
		else if (other.gameObject.tag == "Player" || other.gameObject.tag == "Spaceship")
		{
			player.GetComponent<CubeController>().takeDamage(damage);
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
