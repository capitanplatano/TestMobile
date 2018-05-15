using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RankRow {

	public string alias;

	public int points;

	public RankRow(string alias, int points)
	{
		this.alias = alias;
		this.points = points;
	}
}
