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
						model.mesh = modelUDRL.mesh;
					}
					else
					{
						model.mesh = modelDRU.mesh;
					}
				}
				else if (left)
				{
					model.mesh = modelULD.mesh;
				}
				else
				{
					model.mesh = modelUD.mesh;
				}
			}
			else
			{
				if (right)
				{
					if (left)
					{
						model.mesh = modelRUL.mesh;
					}
					else
					{
						model.mesh = modelUR.mesh;
					}
				}
				else if (left)
				{
					model.mesh = modelUL.mesh;
				}
				else
				{
					model.mesh = modelU.mesh;
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
					model.mesh = modelLDR.mesh;
				}
				else
				{
					model.mesh = modelDR.mesh;
				}
			}
			else if (left)
			{
				model.mesh = modelDL.mesh;
			}
			else
			{
				model.mesh = modelD.mesh;
			}
			return;
		}
		if (right)
		{
			if (left)
			{
				model.mesh = modelRL.mesh;
			}
			else
			{
				model.mesh = modelR.mesh;
			}
		}
		else
		{
			model.mesh = modelL.mesh;
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