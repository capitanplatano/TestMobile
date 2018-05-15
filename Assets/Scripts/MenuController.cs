using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		initSaveGame();
		Advertisement.Initialize("1794951");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Escena1", LoadSceneMode.Single);
	}

	public void AddRank()
	{
		SaveAndLoad.savedGames[0].ranking.Add(new RankRow("Primero rank", 1));
		foreach(RankRow r in SaveAndLoad.savedGames[0].ranking)
		{
			Debug.Log(r.alias + ": " + r.points);
		}
		SaveAndLoad.Save();
	}

	public void showAdd()
	{
		if (Advertisement.IsReady("customVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			
			Advertisement.Show("customVideo",options);
		}
		else
			Debug.Log("Not ready");

	}
	private void HandleShowResult(ShowResult result)
	{
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

	public void tutorialRealizado()
	{
		SaveAndLoad.savedGames[0].tutorialed = true;
		SaveAndLoad.Save();
	}

	public void initSaveGame()
	{
		SaveAndLoad.savedGames = new List<Game>();
		SaveAndLoad.Load();

		if(SaveAndLoad.savedGames.Count > 0)
		{
			SaveAndLoad.selectedIndex = 0;
			/*foreach(RankRow r in SaveAndLoad.savedGames[SaveAndLoad.selectedIndex].ranking)
			{
				Debug.Log(r.alias + " " + r.points.ToString());
			}
			Debug.Log("Tutorial hecho:" + SaveAndLoad.savedGames[SaveAndLoad.selectedIndex].tutorialed);*/
		}
		else
		{
			Debug.Log("Sin archivo de guardado");
			SaveAndLoad.savedGames.Add( new Game());
		}
	}


}
