using UnityEngine;
using System.Collections;

public class ColumnPool : MonoBehaviour 
{
	public GameObject columnPrefab;									//The column game object.
	public int columnPoolSize = 5;									//How many columns to keep on standby.
	public float spawnRate = 3f;									//How quickly columns spawn.
	public float columnMin = -1f;									//Minimum y value of the column position.
	public float columnMax = 3.5f;									//Maximum y value of the column position.

	private Vector2 objectPoolPosition = new Vector2 (-15,-25);		//A holding position for our unused columns offscreen.
	private float spawnXPosition = 10f;

	private float timeSinceLastSpawned;

	//This spawns columns as long as the game is not over.
	void Update()
	{
		timeSinceLastSpawned += Time.deltaTime;

		if (GameManager.IsGameOver() == false && timeSinceLastSpawned >= spawnRate) 
		{	
			timeSinceLastSpawned = 0f;

			//Set a random y position for the column
			float spawnYPosition = Random.Range(columnMin, columnMax);

            GameObject new_column = Instantiate(columnPrefab, objectPoolPosition, Quaternion.identity);

            //...then set the current column to that position.
            new_column.transform.position = new Vector2(spawnXPosition, spawnYPosition);
		}
	}
}