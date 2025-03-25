using UnityEditor;
using UnityEngine;

public class LevelBuilderTool : EditorWindow
{
    private GameObject selectedBlockPrefab;

    [MenuItem("Tools/Level Builder")]
    public static void ShowWindow()
    {
        GetWindow<LevelBuilderTool>("Level Builder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Choose block wanna place", EditorStyles.boldLabel);

        selectedBlockPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Block Prefab",
            selectedBlockPrefab,
            typeof(GameObject),
            false
        );

        if (selectedBlockPrefab != null)
        {
            GUILayout.Label("→ In scene, hold Shift and Click to place block", EditorStyles.helpBox);
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.shift && e.type == EventType.MouseDown && e.button == 0 && selectedBlockPrefab != null)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 point = hit.point;
                Vector3 gridPos = new Vector3(
                    Mathf.Round(point.x),
                    0f,
                    Mathf.Round(point.z)
                );

                GameObject newBlock = (GameObject)PrefabUtility.InstantiatePrefab(selectedBlockPrefab);
                newBlock.transform.position = gridPos;
                Undo.RegisterCreatedObjectUndo(newBlock, "Place Block");
                e.Use();
            }
        }
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}