using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MapModelSelector : MonoBehaviour
{

	public MeshFilter modelU, modelD, modelR, modelL, modelUD, modelRL, modelUR, modelUL, modelDR, modelDL, modelULD, modelRUL, modelDRU, modelLDR, modelUDRL;
	public bool up, down, left, right;
	public int type; // 0: normal, 1: enter, 2: boss
	public Color normalColor, enterColor, bossColor;
	Color mainColor;
	public MeshCollider collider;
	public RoomLogic roomLogic;
	MeshFilter model;

	public GameObject[] roomLayouts = new GameObject[1];


	void Start()
	{

		
		
		model = GetComponent<MeshFilter>();
		mainColor = normalColor;
		Pickmesh();
		//PickColor();
		SetupRoomLogic();
		MakeInterior(4);
		
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
						collider.sharedMesh = modelUDRL.sharedMesh;
					}
					else
					{
						model.mesh = modelDRU.sharedMesh;
						collider.sharedMesh = modelDRU.sharedMesh;
					}
				}
				else if (left)
				{
					model.mesh = modelULD.sharedMesh;
					collider.sharedMesh = modelULD.sharedMesh;
				}
				else
				{
					model.mesh = modelUD.sharedMesh;
					collider.sharedMesh = modelUD.sharedMesh;
				}
			}
			else
			{
				if (right)
				{
					if (left)
					{
						model.mesh = modelRUL.sharedMesh;
						collider.sharedMesh = modelRUL.sharedMesh;
					}
					else
					{
						model.mesh = modelUR.sharedMesh;
						collider.sharedMesh = modelUR.sharedMesh;
					}
				}
				else if (left)
				{
					model.mesh = modelUL.sharedMesh;
					collider.sharedMesh = modelUL.sharedMesh;
				}
				else
				{
					model.mesh = modelU.sharedMesh;
					collider.sharedMesh = modelU.sharedMesh;
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
					collider.sharedMesh = modelLDR.sharedMesh;
				}
				else
				{
					model.mesh = modelDR.sharedMesh;
					collider.sharedMesh = modelDR.sharedMesh;
				}
			}
			else if (left)
			{
				model.mesh = modelDL.sharedMesh;
				collider.sharedMesh = modelDL.sharedMesh;
			}
			else
			{
				model.mesh = modelD.sharedMesh;
				collider.sharedMesh = modelD.sharedMesh;
			}
			return;
		}
		if (right)
		{
			if (left)
			{
				model.mesh = modelRL.sharedMesh;
				collider.sharedMesh = modelRL.sharedMesh;
			}
			else
			{
				model.mesh = modelR.sharedMesh;
				collider.sharedMesh = modelR.sharedMesh;
			}
		}
		else
		{
			model.mesh = modelL.sharedMesh;
			collider.sharedMesh = modelL.sharedMesh;
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

	private void SetupRoomLogic()
    {
        if (!down)
        {
			roomLogic.doors[0] = null;
        }
		
		if (!up)
        {
			roomLogic.doors[1] = null;
        }
		
		if (!left)
        {
			roomLogic.doors[2] = null;
        }
		
		if (!right)
        {
			roomLogic.doors[3] = null;
        }
    }

	private void MakeInterior(int difficulty)
    {
		if(type == 0)
        {
			GameObject decor = Instantiate(roomLayouts[Random.Range(0, roomLayouts.Length - 1)], gameObject.transform.position, Quaternion.identity, gameObject.transform);
			decor.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			decor.transform.eulerAngles = new Vector3(decor.transform.eulerAngles.x + 90, decor.transform.eulerAngles.y, decor.transform.eulerAngles.z);
			decor.GetComponent<RoomLayout>().SpawnEnemies(difficulty);
			

		}
	}

}