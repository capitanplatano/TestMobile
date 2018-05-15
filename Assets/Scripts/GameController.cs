using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	private CubeController playerRef;
	private List<float> columnPositions = new List<float>();
	public int timer = 10;
	public int timerCounter = 0;
	private int massiveSpawner = 1;

	public GameObject asteroidRef;
	public GameObject YSpawner;
	private int points = 0;

	public GameObject[] hearts;
	public GameObject[] menues;
	private Game currentGame;
	private bool paused = false;

	void Start () {
		
		playerRef = GameObject.Find("Player").GetComponent<CubeController>();
		foreach(GameObject g in playerRef.positions)
			columnPositions.Add(g.transform.position.x);

		StartCoroutine("SpawnerLogic");
		StartCoroutine("TimePoints");
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(paused) ResumeGame();
			else PauseGame();
		}
	}

	IEnumerator SpawnerLogic()
	{
		while(true)
		{
			if(timerCounter == 0)
			{
				int massiveSpawnerRng = Random.Range(1, massiveSpawner + 1);
				int numInstantiates = Random.Range(1, massiveSpawnerRng + 1);
				List<int> positionsToSpawn = takeRndPositions(numInstantiates, columnPositions.Count);
				
				foreach( int a in positionsToSpawn)
				{
					Instantiate(asteroidRef, new Vector2(columnPositions[a], YSpawner.transform.position.y), new Quaternion(0,0,0,0));
				}
			}

			if(timerCounter == timer)
				timerCounter = 0;
			else
				timerCounter++;

			yield return new WaitForSeconds(0.1F);
		}
	}

	public List<int> takeRndPositions(int num, int length)
	{
		List<int> res = new List<int>();
		if(num >= length)
		{
			for(int i = 0; i < length; i++)
				res.Add(i);
		}
		else
		{
			while(res.Count < num)
			{
				int rInt = Random.Range(0, length);
				if (!res.Contains(rInt))
				{
					res.Add(rInt);
				}
			}

		}

		return res;
	}

	public void sumPoints(int p)
	{
		points += p;
		GameObject.Find("Points").GetComponent<Text>().text = points.ToString("0000000");
	}

	public void setHealth(int currentHearts)
	{
		for(int i = 0; i < hearts.Length; i++)
		{
			if( (i + 1) <= (currentHearts / 2))
				hearts[i].GetComponent<Animator>().SetInteger("Filled", 2);
			else if(currentHearts % 2 > 0 && (i+1) == ((currentHearts + 1) / 2))
				hearts[i].GetComponent<Animator>().SetInteger("Filled", 1);
			else
				hearts[i].GetComponent<Animator>().SetInteger("Filled", 0);
		}
	}

	public void updateBullets(int bullets, int maxBullets)
	{
		GameObject.Find("Bullets").GetComponent<Text>().text = bullets.ToString();
		GameObject.Find("MaxBullets").GetComponent<Text>().text = maxBullets.ToString();
	}

	IEnumerator TimePoints()
	{
		while (true)
		{
			sumPoints(1);
			yield return new WaitForSeconds(1);
		}
	}

	public void addRank(string alias, int points)
	{
		SaveAndLoad.savedGames[SaveAndLoad.selectedIndex].ranking.Add(new RankRow(alias, points));
		SaveAndLoad.Save();
	}

	private void PauseGame()
    {
		paused = true;
        Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
		}
		StopCoroutine("SpawnerLogic");
		StopCoroutine("TimePoints");
    } 

	private void ResumeGame()
    {
		paused = false;
        Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
		}
		StartCoroutine("SpawnerLogic");
		StartCoroutine("TimePoints");
    } 

	public void gameOver()
	{
		menues[0].SetActive(true);
		menues[3].SetActive(true);
		PauseGame();
	}

	public void showAd()
	{
		if (Advertisement.IsReady("customVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleAdverResult };
			
			Advertisement.Show("customVideo", options);
		}
		else
			Debug.Log("Not ready");
	}

	public void HandleAdverResult(ShowResult result){
		switch(result)
		{
			
			case ShowResult.Finished:
				Debug.Log("Finalizado");
			break;
			case ShowResult.Skipped:
				Debug.Log("Saltado");
			break;
			case ShowResult.Failed:
				Debug.Log("Fallo");
			break;
		}
	}

	public void cancelMenuBTN()
	{
		menues[0].SetActive(false);
		menues[1].SetActive(true);
	}

	public void saveRankingBTN()
	{
		string aliasText = GameObject.Find("AliasTXT").GetComponent<InputField>().text;
		if(aliasText.Length > 0)
		{
			addRank(aliasText, points);
		}
		menues[1].SetActive(false);
		menues[2].SetActive(true);
	}

	public void cancelRankBTN()
	{
		menues[1].SetActive(false);
		menues[2].SetActive(true);
	}

	public void retryBTN()
	{
		SceneManager.LoadScene("Escena1", LoadSceneMode.Single);
	}

	public void goToMenuBTN()
	{
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	private void checkDifficulty()
	{
		switch(massiveSpawner)
		{
			case 1:
				if(points > 1000)
					massiveSpawner = 2;
			break;
			case 2:
				if(points > 5000)
					massiveSpawner = 3;
			break;
			case 3:
				if(points > 15000)
					massiveSpawner = 4;
			break;
			case 4:
				if(points > 25000)
					massiveSpawner = 5;
			break;
			default:
			break;
		}
	}
}
