using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class LevelGeneration : MonoBehaviour {

	public GameObject Cube;
	public GameObject playerPrefab;
	public GameObject ground;
	public GameObject enemy;
	public Transform miniMapParent;
	public bool isBoss = false;
	public bool isReward = false;

	public Vector2 worldSize = new Vector2(8,8);
	Room[,] rooms;
	List<Vector2> takenPositions = new List<Vector2>();
	private int gridSizeX, gridSizeY;
	public int numberOfRooms;
	public GameObject roomWhiteObj;
	public void MakeMap () {
		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
		{ // make sure we dont try to make more rooms than can fit in our grid
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}
		gridSizeX = Mathf.RoundToInt(worldSize.x); //note: these are half-extents
		gridSizeY = Mathf.RoundToInt(worldSize.y);
		CreateRooms(); //lays out the actual map
		SetRoomDoors(); //assigns the doors where rooms would connect
		DrawMap(); //instantiates objects to make up a map
		DrawMiniMap();

		Invoke("BakeMesh", 0.5f);



		Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

	}
	void CreateRooms(){
		//setup
		rooms = new Room[gridSizeX * 2, gridSizeY * 2];
		rooms[gridSizeX,gridSizeY] = new Room(Vector2.zero, 1);
		takenPositions.Insert(0,Vector2.zero);
		Vector2 checkPos = Vector2.zero;
		//magic numbers
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
		//add rooms
		for (int i =0; i < numberOfRooms -1; i++){
			float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
			//grab new position
			checkPos = NewPosition();
			//test new position
			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare){
				int iterations = 0;
				do{
					checkPos = SelectiveNewPosition();
					iterations++;
				}while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
				if (iterations >= 50)
					print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
			}
			//finalize position
			rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new Room(checkPos, 0);
			takenPositions.Insert(0,checkPos);
		}	
	}
	Vector2 NewPosition(){
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
			x = (int) takenPositions[index].x;//capture its x, y position
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
			bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
			if (UpDown){ //find the position bnased on the above bools
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
		return checkingPos;
	}
	Vector2 SelectiveNewPosition(){ // method differs from the above in the two commented ways
		int index = 0, inc = 0;
		int x =0, y =0;
		Vector2 checkingPos = Vector2.zero;
		do{
			inc = 0;
			do{ 
				//instead of getting a room to find an adject empty space, we start with one that only 
				//as one neighbor. This will make it more likely that it returns a room that branches out
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;
			}while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);
			if (UpDown){
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		if (inc >= 100){ // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
			print("Error: could not find position with only one neighbor");
		}
		return checkingPos;
	}
	int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions){
		int ret = 0; // start at zero, add 1 for each side there is already a room
		if (usedPositions.Contains(checkingPos + Vector2.right)){ //using Vector.[direction] as short hands, for simplicity
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.left)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.up)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.down)){
			ret++;
		}
		return ret;
	}
	void DrawMiniMap(){

		//Debug.Log(FindBossRoom());
		foreach (Room room in rooms){
			if (room == null){
				continue; //skip where there is no room
			}
			Vector2 drawPos = room.gridPos;
			drawPos.x *= 25;//aspect ratio of map mesh
			drawPos.y *= 25;
			//create map obj and assign its variables
			MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, new Vector2(drawPos.x + miniMapParent.parent.transform.position.x, drawPos.y + miniMapParent.parent.transform.position.y), Quaternion.identity, miniMapParent).GetComponent<MapSpriteSelector>(); 
			if(room == rooms[(int)FindBossRoom().x, (int)FindBossRoom().y])
            {
				mapper.type = 2;
            }
            else
            {
				mapper.type = room.type;
			}
			mapper.position = new Vector2(drawPos.x + miniMapParent.parent.transform.position.x, drawPos.y + miniMapParent.parent.transform.position.y);
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
		}
	}
	void DrawMap(){

		////Debug.Log(FindBossRoom());
		foreach (Room room in rooms){
			if (room == null){
				continue; //skip where there is no room
			}
			Vector2 drawPos = room.gridPos;
			drawPos.x *= 16;//aspect ratio of map mesh
			drawPos.y *= 16;
			//create map obj and assign its variables
			
			GameObject newRoom = Object.Instantiate(Cube, drawPos, Quaternion.identity); 
			MapModelSelector mapper = newRoom.GetComponent<MapModelSelector>();
			
			
			if(room == rooms[(int)FindBossRoom().x, (int)FindBossRoom().y])
			{
				mapper.type = 2;
			}
			else
			{
				mapper.type = room.type;
			}
			mapper.pos = room.gridPos;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
			newRoom.transform.RotateAround(Vector3.zero, Vector3.right, 90);
			newRoom.transform.position = new Vector3(newRoom.transform.position.x, 0, newRoom.transform.position.z);
		}
	}
	void SetRoomDoors(){
		for (int x = 0; x < ((gridSizeX * 2)); x++){
			for (int y = 0; y < ((gridSizeY * 2)); y++){
				if (rooms[x,y] == null){
					continue;
				}
				Vector2 gridPosition = new Vector2(x,y);
				if (y - 1 < 0){ //check above
					rooms[x,y].doorBot = false;
				}else{
					rooms[x,y].doorBot = (rooms[x,y-1] != null);
				}
				if (y + 1 >= gridSizeY * 2){ //check bellow
					rooms[x,y].doorTop = false;
				}else{
					rooms[x,y].doorTop = (rooms[x,y+1] != null);
				}
				if (x - 1 < 0){ //check left
					rooms[x,y].doorLeft = false;
				}else{
					rooms[x,y].doorLeft = (rooms[x - 1,y] != null);
				}
				if (x + 1 >= gridSizeX * 2){ //check right
					rooms[x,y].doorRight = false;
				}else{
					rooms[x,y].doorRight = (rooms[x+1,y] != null);
				}
			}
		}
	}
	Vector2 FindBossRoom()
    {

		Vector2 bossRoom = new Vector2(gridSizeX, gridSizeY);

		int totalRooms = 0;
		
		for (int x = 0; x < ((gridSizeX * 2)); x++)
		{
			for (int y = 0; y < ((gridSizeY * 2)); y++)
			{
				if (rooms[x, y] == null)
				{
					continue;
				}
				totalRooms++;
			}
		}

		Dictionary<Vector2, int> lar = new Dictionary<Vector2, int>();



		int dist = 0;

		List<Vector2> lookedAtRooms = new List<Vector2>(totalRooms);


		Vector2 nextRoom = new Vector2(gridSizeX,gridSizeY);

		
		lar.Add(nextRoom, dist);

		while (lar.Count < totalRooms)
        {
			////Debug.Log(dist);
			//get rooms furthest away from starting room in list
			foreach (KeyValuePair<Vector2, int> currentRoom in lar)
			{
				nextRoom = currentRoom.Key;
				////Debug.Log(nextRoom);
				if (currentRoom.Value == dist)
				{
					////Debug.Log(nextRoom + " in foreach");

					if (rooms[(int)nextRoom.x + 1, (int)nextRoom.y] != null)
					{
						lookedAtRooms.Add(new Vector2((int)nextRoom.x + 1, (int)nextRoom.y));
					}
					if (rooms[(int)nextRoom.x - 1, (int)nextRoom.y] != null)
					{
						lookedAtRooms.Add(new Vector2((int)nextRoom.x - 1, (int)nextRoom.y));
					}
					if (rooms[(int)nextRoom.x, (int)nextRoom.y + 1] != null)
					{
						lookedAtRooms.Add(new Vector2((int)nextRoom.x, (int)nextRoom.y + 1));
					}
					if (rooms[(int)nextRoom.x, (int)nextRoom.y - 1] != null)
					{
						lookedAtRooms.Add(new Vector2((int)nextRoom.x, (int)nextRoom.y - 1));
					}
				}
			}
			dist++;
            foreach (Vector2 currentRoom in lookedAtRooms)
            {
                if (!lar.ContainsKey(currentRoom))
                {
					lar.Add(currentRoom, dist);
					////Debug.Log(currentRoom + " " + dist);
				}
			}
			lookedAtRooms.Clear();
		}

		int greatestValue = 0;

		foreach(KeyValuePair<Vector2, int> entry in lar)
{
			if(entry.Value > greatestValue)
            {
				greatestValue = entry.Value;
				bossRoom = entry.Key;
            }
		}
		return bossRoom;
	}

	void BakeMesh()
    {
		ground.GetComponent<NavMeshSurface>().BuildNavMesh();

	}

}
