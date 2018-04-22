using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorUtilityFunctions {

    public static string tilePrefix = "Tile_";
    public static string spriteSheetIDPrefix = "t_";
    static string generatedAssetFolderName = "_GeneratedAssets";


    public static string GetGeneratedAssetsFolder() {
        string rootFolder = "Assets";
        string folderPath = rootFolder + "/" + generatedAssetFolderName;
        if (!AssetDatabase.IsValidFolder(folderPath)) {
            AssetDatabase.CreateFolder(rootFolder, generatedAssetFolderName);
        }
        return folderPath + "/";
    }

    public static GameObject GetRoomPrefab() {
        string[] possiblePrefabs = AssetDatabase.FindAssets("Room t:prefab");
        if (possiblePrefabs.Length == 0) {
            return null;
        }
        else {
            return AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(possiblePrefabs[0]));
        }
    }

    public static List<GameObject> GetTilePrefabs() {
        List<GameObject> toReturn = new List<GameObject>();
        string[] possiblePrefabs = AssetDatabase.FindAssets("Tile_ t:prefab");
        foreach (string possiblePrefab in possiblePrefabs) {
            toReturn.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(possiblePrefab)));
        }

        return toReturn;
    }

    public static GameObject GenerateNewTilePrefab(string type, Sprite prefabSprite = null) {
        string prefabName = EditorUtilityFunctions.tilePrefix + type;
        string prefabPath = EditorUtilityFunctions.GetGeneratedAssetsFolder() + prefabName + ".prefab";
        GameObject tile = new GameObject();
        SpriteRenderer tileSR = tile.AddComponent<SpriteRenderer>();
        if (prefabSprite)
            tileSR.sprite = prefabSprite;
        GameObject newPrefab = PrefabUtility.CreatePrefab(prefabPath, tile);
        UnityEngine.Object.DestroyImmediate(tile);

        return newPrefab;
    }
}
