using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModelSelector : MonoBehaviour
{

	public MeshFilter modelU, modelD, modelR, modelL, modelUD, modelRL, modelUR, modelUL, modelDR, modelDL, modelULD, modelRUL, modelDRU, modelLDR, modelUDRL;
	public bool up, down, left, right;
	public int type; // 0: normal, 1: enter, 2: boss
	public Color normalColor, enterColor, bossColor;
	Color mainColor;
	MeshFilter model;
	void Start()
	{

		
		
		model = GetComponent<MeshFilter>();
		mainColor = normalColor;
		Pickmesh();
		//PickColor();
	
	}
	void Pickmesh()
	{ //picks correct mesh based on the four door bools
		if (up)
		{
			if (down)
			{
				if (right)
				{
					if (left)
					{
						model.mesh = modelUDRL.sharedMesh;
					}
					else
					{
						model.mesh = modelDRU.sharedMesh;
					}
				}
				else if (left)
				{
					model.mesh = modelULD.sharedMesh;
				}
				else
				{
					model.mesh = modelUD.sharedMesh;
				}
			}
			else
			{
				if (right)
				{
					if (left)
					{
						model.mesh = modelRUL.sharedMesh;
					}
					else
					{
						model.mesh = modelUR.sharedMesh;
					}
				}
				else if (left)
				{
					model.mesh = modelUL.sharedMesh;
				}
				else
				{
					model.mesh = modelU.sharedMesh;
				}
			}
			return;
		}
		if (down)
		{
			if (right)
			{
				if (left)
				{
					model.mesh = modelLDR.sharedMesh;
				}
				else
				{
					model.mesh = modelDR.sharedMesh;
				}
			}
			else if (left)
			{
				model.mesh = modelDL.sharedMesh;
			}
			else
			{
				model.mesh = modelD.sharedMesh;
			}
			return;
		}
		if (right)
		{
			if (left)
			{
				model.mesh = modelRL.sharedMesh;
			}
			else
			{
				model.mesh = modelR.sharedMesh;
			}
		}
		else
		{
			model.mesh = modelL.sharedMesh;
		}
	}

	//void PickColor()
	//{ //changes color based on what type the room is
	//	if (type == 0)
	//	{
	//		mainColor = normalColor;
	//	}
	//	else if (type == 1)
	//	{
	//		mainColor = enterColor;
	//	}
	//	else if (type == 2)
	//	{
	//		mainColor = bossColor;
	//	}
	//	model.color = mainColor;

	//}
}