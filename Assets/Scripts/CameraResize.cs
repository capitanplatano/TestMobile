using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour {

	private void Start()
	{
		//Resize();	
	}
	private void Resize()
	{

		//check actual vs wish aspect
		float scale = ((float)Screen.width / (float)Screen.height ) / (9f/16f);

		if (scale < 1.0f) 
		{
			Rect rect = GetComponent<Camera>().rect;

			rect.width = 1.0f;
			rect.height = scale;
			rect.x = 0;
			rect.y = (1.0f - scale) / 2.0f;

			GetComponent<Camera>().rect = rect;
		}
		else //lanscape
		{
			float scalewidth = 1.0f / scale;

			Rect rect = GetComponent<Camera>().rect;

			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;

			GetComponent<Camera>().rect = rect;
		}

		//Create background in black
		//CreateBackGround();
	}

	private void CreateBackGround()
	{
		Camera cam = new GameObject().AddComponent<Camera>();
		cam.gameObject.isStatic = true;
		cam.depth = -10;
		cam.cullingMask = 0;
		cam.farClipPlane = 1f;
		cam.orthographic = true;
		cam.backgroundColor = Color.black;
		cam.gameObject.name = "BackGround_Camera";
	}
}
