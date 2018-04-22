using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
enum TemplateGame
{
    zelda, metroid
}

public class ParseAndGenerateMap : EditorWindow
{
    // Parse
    public Texture2D inputMap;
    int tileDimensions = 16; // Tiles are always squares.
    int outputSpritesTextureSize = 256;
    string textureName = "TileSpriteSheet.png";

    // Generate
    TemplateGame templateGame = TemplateGame.zelda;
    TextAsset metroidRooms;
    int roomWidth = 16;
    int roomHeight = 11; // 15 for metroid

    [MenuItem("494/1) Parse and Generate Map", false, 0)]
    public static void Generate()
    {
        EditorWindow.GetWindowWithRect(typeof(ParseAndGenerateMap), new Rect(0, 0, 700, 400));
    }

    void OnGUI()
    {
        // Check for errors
        if (EditorUtilityFunctions.GetRoomPrefab() != null)// If room prefab exists
        {
            GUILayout.Label("Room prefab already exists!\nScript will not rerun in order to ensure this prefab isn't overwritten. Delete the prefab if you want to regenerate the map.", EditorStyles.boldLabel);
        }
        else if (EditorUtilityFunctions.GetTilePrefabs().Count > 0)// If any Tiles exist
        {
            GUILayout.Label("Tile prefabs already exist!\nScript will not rerun in order to ensure these prefabs aren't overwritten. Delete the prefabs if you want to regenerate the map.", EditorStyles.boldLabel);
        }
        else if (GameObject.Find("Level") != null)
        {
            GUILayout.Label("This scene already has a Level GameObject!\nThe script will not continue.", EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Label("Generate Map Into Current Scene", EditorStyles.boldLabel);
            GUILayout.Label("", EditorStyles.boldLabel);
            inputMap = (Texture2D)EditorGUILayout.ObjectField("Input Map Texture:", inputMap, typeof(Texture2D), false);
            templateGame = (TemplateGame)EditorGUILayout.EnumPopup("Game:", templateGame);
            if (templateGame == TemplateGame.metroid)
            {
                metroidRooms = (TextAsset)EditorGUILayout.ObjectField("Room Grouping:", metroidRooms, typeof(TextAsset), false);
            }

            if (inputMap == null)
            {
                GUILayout.Label("Input map texture not provided!\nAdd an input map to run the script!", EditorStyles.boldLabel);
            }
            else if (templateGame == TemplateGame.metroid && metroidRooms == null)
            {
                GUILayout.Label("Room groupings not provided!\nAdd room groupings to continue.", EditorStyles.boldLabel);
            }
            else if (GUILayout.Button("Generate!"))
            {
                Texture2D newSpriteSheet;
                int[,] mapAsTileIndices;
                Parse(out newSpriteSheet, out mapAsTileIndices);

                Generate(newSpriteSheet, mapAsTileIndices);

                Debug.Log("Map Generation Successful! Check your hierarchy view and _GeneratedAssets folder for new content.");
                this.Close();
            }
        }
    }

    public void Parse(out Texture2D newSpriteSheet, out int[,] mapAsTileIndices)
    {
        // Pull in the original Metroid map
        Color32[] mapData = inputMap.GetPixels32(0); // This will take a long time and a LOT of memory!

        // Create a new texture to hold the individual sprites
        Color32[] newData = new Color32[outputSpritesTextureSize * outputSpritesTextureSize];
        Texture2D outputSprites = new Texture2D(outputSpritesTextureSize, outputSpritesTextureSize, TextureFormat.RGBA32, false);

        int mapTilesX = inputMap.width / tileDimensions;
        int mapTilesY = inputMap.height / tileDimensions;
        int[,] indices = new int[mapTilesX, mapTilesY];

        // Create a list of checkSums for the individual sprites
        // CheckSums are used to distinguish two tiles from each other
        List<ulong> checkSums = new List<ulong>();

        // Parse it one 16x16-pixel section at-a-time
        for (int y = 0; y < mapTilesY; y++)
        {
            for (int x = 0; x < mapTilesX; x++)
            {
                Color32[] chunk = GetChunk(x, y, mapData);

                // Convert this section to a checkSum
                ulong checkSum = CheckSum(chunk);

                // Check to see whether the current checkSum matches an already-found one
                int checkSumIndex = checkSums.IndexOf(checkSum);

                // If it doesn't, make a new checkSum and a new entry in the outputSprites Texture2D.
                if (checkSumIndex == -1)
                {
                    checkSums.Add(checkSum);
                    checkSumIndex = checkSums.Count - 1;


                    OutputChunk(chunk, newData, checkSumIndex);
                }
                indices[x, y] = checkSumIndex;
            }
        }

        mapAsTileIndices = indices;

        // Generate and output the Texture2D from the newData
        outputSprites.SetPixels32(newData, 0);
        outputSprites.Apply(true);
        string texturePath = SaveTextureToFile(outputSprites, textureName);

        // Update sprite sheet of texture
        int numTiles = checkSums.Count;
        int tileCountPerSide = outputSpritesTextureSize / tileDimensions;
        List<SpriteMetaData> newSpriteMetaData = new List<SpriteMetaData>();
        for (int i = 0; i < numTiles; i++)
        {
            int x = i % tileCountPerSide;
            int y = i / tileCountPerSide;
            SpriteMetaData smd = new SpriteMetaData();
            smd.pivot = new Vector2(0.5f, 0.5f);
            smd.alignment = 9;
            smd.name = EditorUtilityFunctions.spriteSheetIDPrefix + i.ToString("D3");
            smd.rect = new Rect(x * tileDimensions, (tileCountPerSide - y - 1) * tileDimensions, tileDimensions, tileDimensions);
            newSpriteMetaData.Add(smd);
        }
        AssetDatabase.Refresh();

        Texture2D outputTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        string path = AssetDatabase.GetAssetPath(outputTexture);
        TextureImporter ti = (TextureImporter)AssetImporter.GetAtPath(path);
        ti.isReadable = true;
        ti.spritePixelsPerUnit = 16;
        ti.textureType = TextureImporterType.Sprite;
        ti.spriteImportMode = SpriteImportMode.Multiple;
        ti.spritesheet = newSpriteMetaData.ToArray();
        ti.textureCompression = TextureImporterCompression.Uncompressed;
        ti.filterMode = FilterMode.Point;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        newSpriteSheet = outputTexture;

        AssetDatabase.Refresh();

    }

    public Color32[] GetChunk(int x, int y, Color32[] mapData)
    {
        Color32[] res = new Color32[tileDimensions * tileDimensions];
        x *= tileDimensions;
        y *= tileDimensions;
        int ndx;
        for (int j = 0; j < tileDimensions; j++)
        {
            for (int i = 0; i < tileDimensions; i++)
            {
                ndx = x + i + (y + j) * inputMap.width;
                try
                {
                    res[i + j * tileDimensions] = mapData[ndx];
                }
                catch (System.IndexOutOfRangeException)
                {
                    Debug.Log("GetChunk() Index out of range:" + ndx + "\tLength:" + mapData.Length + "\ti=" + i + "\tj=" + j);
                }
            }
        }
        return res;
    }

    public ulong CheckSum(Color32[] chunk)
    {
        ulong res = 0;

        for (int i = 0; i < chunk.Length; i++)
            res += (ulong)(chunk[i].r * (i + 1) + chunk[i].g * (i + 2) + chunk[i].b * (i + 3));

        return res;
    }

    void OutputChunk(Color32[] chunk, Color32[] toData, int spriteIndex)
    {
        int spl = outputSpritesTextureSize / tileDimensions;
        int x = spriteIndex % spl;
        int y = spriteIndex / spl;
        y = spl - y - 1;
        x *= tileDimensions;
        y *= tileDimensions;

        int ndxND, ndxC;
        for (int i = 0; i < tileDimensions; i++)
        {
            for (int j = 0; j < tileDimensions; j++)
            {
                ndxND = x + i + (y + j) * outputSpritesTextureSize;
                ndxC = i + j * tileDimensions;

                try
                {
                    toData[ndxND] = chunk[ndxC];
                }
                catch (System.IndexOutOfRangeException)
                {
                    Debug.Log("OutputChunk() Index out of range:" + ndxND + "\tLengthND:" + toData.Length + "\tndxC=" + ndxC + "\tLengthC" + chunk.Length + "\ti=" + i + "\tj=" + j);
                }
            }
        }
    }

    string SaveTextureToFile(Texture2D tex, string fileName)
    {
        byte[] bytes = tex.EncodeToPNG();
        string folder = EditorUtilityFunctions.GetGeneratedAssetsFolder();
        string path = folder + fileName;
        File.WriteAllBytes(path, bytes);
        return path;
    }

    void Generate(Texture2D spriteSheet, int[,] mapAsTileIndices)
    {
        // Game specific setup
        if (templateGame == TemplateGame.zelda)
            roomHeight = 11;
        else
            roomHeight = 15;

        // Load Sprites
        var spriteSheetPath = AssetDatabase.GetAssetPath(spriteSheet);
        Sprite[] spriteArray = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();        

        // Read in the map data
        int height = mapAsTileIndices.GetLength(1);
        int width = mapAsTileIndices.GetLength(0);

        // Root parent to store rooms and tiles
        Transform root = new GameObject("Level").transform;

        // Rooms for organization
        Transform[,] rooms = null;
        int numRoomsX = width / roomWidth;
        int numRoomsY = height / roomHeight;
        rooms = new Transform[numRoomsX, numRoomsY];
        string roomPrefabPath = EditorUtilityFunctions.GetGeneratedAssetsFolder() + "Room.prefab";
        GameObject roomInstance = new GameObject("Room");
        GameObject roomPrefab = PrefabUtility.CreatePrefab(roomPrefabPath, roomInstance);
        DestroyImmediate(roomInstance);

        //Metroid rooms are grouped together in halls
        Dictionary<char, GameObject> hallsDict = new Dictionary<char, GameObject>();
        string[] hallsMatrix = null;
        if (templateGame == TemplateGame.metroid)
        {
            hallsMatrix = metroidRooms.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        }
        
        // Now generate all of the rooms
        for (int y = 0; y < numRoomsY; y++)
        {
            for (int x = 0; x < numRoomsX; x++)
            {
                // In zelda, every room is the same size
                if (templateGame == TemplateGame.zelda)
                {
                    GameObject roomGO = (GameObject)PrefabUtility.InstantiatePrefab(roomPrefab);
                    roomGO.transform.position = new Vector2(x * roomWidth, y * roomHeight);
                    roomGO.name = "Room (" + x + "," + y + ")";
                    roomGO.transform.parent = root;
                    rooms[x, y] = roomGO.transform;
                }
                //In metroid, rooms are connected
                else
                {
                    char thisRoomChar = hallsMatrix[numRoomsY - y - 1][x];
                    if (!hallsDict.ContainsKey(thisRoomChar))
                    {
                        GameObject roomGO = (GameObject)PrefabUtility.InstantiatePrefab(roomPrefab);
                        roomGO.transform.position = new Vector2(x * roomWidth, y * roomHeight);
                        roomGO.name = "Room " + thisRoomChar;
                        roomGO.transform.parent = root;
                        rooms[x, y] = roomGO.transform;
                        hallsDict.Add(thisRoomChar, roomGO);
                    }
                    else
                    {
                        rooms[x, y] = hallsDict[thisRoomChar].transform;
                    }
                }

            }
        }

        // Create a new tile prefab
        GameObject tilePrefab = EditorUtilityFunctions.GenerateNewTilePrefab("NONE");

        // Place tiles on the map
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int typeNum = mapAsTileIndices[x, y];
                if (typeNum == 0)
                    continue; // Skip past empty black tile

                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab);
                tile.transform.position = new Vector3(x, y);
                tile.transform.parent = rooms[x / roomWidth, y / roomHeight];
                tile.GetComponent<SpriteRenderer>().sprite = spriteArray[typeNum];
            }
        }

        //Delete Empty rooms
        for (int y = 0; y < numRoomsY; y++)
        {
            for (int x = 0; x < numRoomsX; x++)
            {
                if (rooms[x, y] && rooms[x, y].childCount == 0)
                    DestroyImmediate(rooms[x, y].gameObject);
            }
        }

        // Sort metroid rooms
        if (templateGame == TemplateGame.metroid)
        {
            SortChildrenByName(root);
        }
    }

    void SortChildrenByName(Transform root)
    {
        List<Transform> children = new List<Transform>();
        for (int i = root.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = root.transform.GetChild(i);
            children.Add(child);
            child.parent = null;
        }
        children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
        foreach (Transform child in children)
        {
            child.parent = root.transform;
        }
    }
}