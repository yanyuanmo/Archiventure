using UnityEngine;
using UnityEditor;
using System.IO;

public class GameObjectHelper: EditorWindow
{
    [MenuItem("Tools/Create Prefabs From PNGs")]
    static void CreatePrefabs()
    {
        // Open folder
        string folderPath = EditorUtility.OpenFolderPanel("Select Folder Containing PNGs", "Assets", "");

        if (string.IsNullOrEmpty(folderPath))
            return;

        // Get all PNG pictures
        string[] pngFiles = Directory.GetFiles(folderPath, "*.png");

        foreach (var file in pngFiles)
        {
            // Create texture
            string relativePath = "Assets" + file.Substring(Application.dataPath.Length);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);

            if (sprite != null) {

                GameObject newGameObject = new GameObject(Path.GetFileNameWithoutExtension(file));
                SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;

                string prefabPath = "Assets/Prefabs/" + Path.GetFileNameWithoutExtension(file) + ".prefab";
                if (!Directory.Exists("Assets/Prefabs"))
                {
                    Directory.CreateDirectory("Assets/Prefabs");
                }
                
                // GameObject to Prefab
                PrefabUtility.SaveAsPrefabAsset(newGameObject, prefabPath);
                
                // Delete temporary GameObject
                DestroyImmediate(newGameObject);
                
            } else {
                Debug.LogWarning($"Failed to load texture from: {relativePath}");
            }
        }

        // Show Prefab
        AssetDatabase.Refresh();
        Debug.Log("Prefabs creation complete!");
    }
}
