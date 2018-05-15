using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

	private Rect leftArea;
	public Rect shootArea;
	private float leftPosition;
	private float rightPosition;
	private bool reloading;
	private bool returningReloading;
	public GameObject[] positions;

	private int currentPosition;
	private float currentPositionX;
	private float previousPositionX;
	public float movingSpeed = 35f;
	private bool moving;
    private float lastTimeBullet;
    public GameObject bullet;
	private Vector2 gunPosition;

	public int currentBullets;
	public int maxBullets = 10;

	private float lastTimeReload;
	public float reloadRefresh = 0.1F;

	public int currentHealth;
	public int maxHealth = 6;
	public int bulletDamage = 1;
	public int maxBulletDamage = 10;

	private float threshold = 0.02f;

	private float shootSpeed = 0.2f;
	private bool paused = false;
	private GameController gc;

	void Start () {
		gc = GameObject.Find("GameController").GetComponent<GameController>();
		leftArea = new Rect(0, 0, Screen.width / 2, Screen.height);
		shootArea = new Rect(GameObject.Find("BTN").transform.position.x - (GameObject.Find("BTN").GetComponent<Renderer>().bounds.size.x/2),
							GameObject.Find("BTN").transform.position.y - (GameObject.Find("BTN").GetComponent<Renderer>().bounds.size.y/2),  
							GameObject.Find("BTN").GetComponent<Renderer>().bounds.size.x,
							GameObject.Find("BTN").GetComponent<Renderer>().bounds.size.y);

		gunPosition = transform.Find("Gun").transform.position;

		setBullets(maxBullets);

		currentHealth = maxHealth;

		leftPosition = GameObject.Find("LeftPosition").transform.position.x;
		rightPosition = GameObject.Find("RightPosition").transform.position.x;
		currentPosition = ((positions.Length-1) / 2);
		
		transform.position = new Vector2(positions[currentPosition].transform.position.x, transform.position.y);

		gc.setHealth(currentHealth);
	}
	
	void Update () {
		if(!paused)
		{
			currentPositionX = positions[currentPosition].transform.position.x;
			if(reloading)
			{
				reload();
			}
			else if(moving)
			{
				if(Mathf.Abs(transform.position.x - currentPositionX) > threshold )
				{
					moveFromTo(previousPositionX, currentPositionX);
				}
				else
					moving = false;			 
			}
			else
			{
				if(Input.GetMouseButton(0))
				{
					if(shootArea.Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
					{
						gunPosition.x = transform.position.x;
						shoot();
					}
					else if(Input.GetMouseButtonDown(0))
					{
						previousPositionX = currentPositionX;
						if(leftArea.Contains(Input.mousePosition))
						{
							if(currentPosition > 0)
							{
								currentPosition--;
								moving = true;
							}
							else
							{
								reloading = true;
							}
						}
						else
						{
							if(currentPosition < (positions.Length - 1))
							{
								currentPosition++;
								moving = true;
							}
							else
							{
								reloading = true;
							}
						}
					}
				}
				//PC Controlls
				else
				{
					if(Input.GetKey(KeyCode.Space))
					{
						gunPosition.x = transform.position.x;
						shoot();
					}
					else 
					{
						
						if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
						{
							previousPositionX = currentPositionX;
							if(currentPosition > 0)
							{
								currentPosition--;
								moving = true;
							}
							else
							{
								reloading = true;
							}
						}
						else if( Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
						{
							previousPositionX = currentPositionX;
							if(currentPosition < (positions.Length - 1))
							{
								currentPosition++;
								moving = true;
							}
							else
							{
								reloading = true;
							}
						}
					}
				}
			}
		}
	}

	public void shoot(){
		if(currentBullets > 0 && (lastTimeBullet + shootSpeed) < Time.time)
		{
			setBullets(currentBullets - 1);
			lastTimeBullet = Time.time;
			GameObject newBullet = Instantiate(bullet, new Vector2(gunPosition.x, gunPosition.y), new Quaternion(0,0,0,0));
			newBullet.GetComponent<Bullet>().bulletDamage = bulletDamage;
			//Sound Effect
		}

	}
	public void reload()
	{
		float loadingPosition = currentPosition == 0 ? leftPosition : rightPosition;
		if(returningReloading)
		{
			if(Mathf.Abs(transform.position.x - currentPositionX) < threshold)
			{
				reloading = false;
				returningReloading = false;
			}
			else
				moveFromTo(loadingPosition, currentPositionX);
		}
		else
		{
			if(Mathf.Abs(transform.position.x - loadingPosition) < threshold)
			{
				if(currentBullets < maxBullets)
					reloadBullet();
				else
					returningReloading = true;
			}
			else
				moveFromTo(currentPositionX, loadingPosition);
		
		}
	}


	public void moveFromTo(float initialPosition, float finalPosition)
	{
		float distance = Mathf.Abs(initialPosition - finalPosition);
		if(transform.position.x > finalPosition)
		{
			if( Mathf.Abs(transform.position.x - finalPosition) < distance / 4) 
				transform.position = new Vector2(transform.position.x - (movingSpeed/6 * Time.deltaTime), transform.position.y);
			else
				transform.position = new Vector2(transform.position.x - (movingSpeed * Time.deltaTime), transform.position.y);
		}
		else
		{
			if( Mathf.Abs(transform.position.x - finalPosition) < distance / 4) 
				transform.position = new Vector2(transform.position.x + (movingSpeed/6 * Time.deltaTime), transform.position.y);
			else
				transform.position = new Vector2(transform.position.x + (movingSpeed * Time.deltaTime), transform.position.y);
		}
	}

	public void reloadBullet()
	{
		if((lastTimeReload + reloadRefresh) < Time.time)
		{
			lastTimeReload = Time.time;
			setBullets(currentBullets + 1);
		}
	}

	public void setBullets(int newBullets){
		currentBullets = newBullets;
		gc.updateBullets(currentBullets, maxBullets);
		//Sound Effect
		//Show bullet Icons
	}

	public void takeDamage(int damage)
	{
		currentHealth -= damage;
		gc.setHealth(currentHealth);
		//Sound effect
		if(currentHealth <= 0)
			Die();
	}

	public void Die(){

		//Sound effect
		gc.gameOver();
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
