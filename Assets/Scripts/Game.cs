using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game { 
 
    public static Game current;
    public bool tutorialed;
	public List<RankRow> ranking;
 
    public Game () {
        ranking = new List<RankRow>();
    }

	
}