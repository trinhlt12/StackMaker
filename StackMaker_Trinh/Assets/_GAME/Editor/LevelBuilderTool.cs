using UnityEditor;
using UnityEngine;

public class LevelBuilderTool : EditorWindow
{
    private enum BlockType
    {
        None,
        Ground
    }

    private GameObject groundBlockPrefab;
    private Vector3? lastPlacedPosition = null;
    private BlockType currentBlockType = BlockType.None;

    [MenuItem("Tools/Level Builder")]
    public static void ShowWindow()
    {
        GetWindow<LevelBuilderTool>("Level Builder");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Builder Tool", EditorStyles.boldLabel);

        currentBlockType = (BlockType)EditorGUILayout.EnumPopup("Block Type", currentBlockType);

        GUILayout.Space(10);
        GUILayout.Label("Ground Layer Block", EditorStyles.boldLabel);

        groundBlockPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Ground Prefab",
            groundBlockPrefab,
            typeof(GameObject),
            false
        );
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.alt && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            TryEraseBlockAtMousePosition(e);
        }
        else if (e.shift && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            TryPlaceBlockAtMousePosition(e);
        }

        if (e.type == EventType.MouseUp && e.button == 0)
        {
            lastPlacedPosition = null;
        }
    }

    private void TryPlaceBlockAtMousePosition(Event e)
    {
        GameObject prefabToPlace = GetPrefabForCurrentBlockType();
        if (prefabToPlace == null) return;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;
            Vector3 gridPos = new Vector3(
                Mathf.Floor(point.x) + 0.5f,
                0f,
                Mathf.Floor(point.z) + 0.5f);

            if (lastPlacedPosition == null || Vector3.Distance(gridPos, lastPlacedPosition.Value) > 0.01f)
            {
                GameObject newBlock = (GameObject)PrefabUtility.InstantiatePrefab(prefabToPlace);
                newBlock.transform.position = gridPos;
                Undo.RegisterCreatedObjectUndo(newBlock, "Place Block");

                ApplySettingsForBlockType(newBlock);

                lastPlacedPosition = gridPos;
                e.Use();
            }
        }
    }

    private GameObject GetPrefabForCurrentBlockType()
    {
        switch (currentBlockType)
        {
            case BlockType.Ground:
                return groundBlockPrefab;
            default:
                return null;
        }
    }

    private void ApplySettingsForBlockType(GameObject block)
    {
        switch (currentBlockType)
        {
            case BlockType.Ground:
                ApplyGroundSettings(block);
                break;
        }
    }

    private static void ApplyGroundSettings(GameObject block)
    {
        SetLayerRecursively(block, LayerMask.NameToLayer("Ground"));


        var groundRoot = GameObject.Find("Ground");
        if (groundRoot == null)
        {
            groundRoot = new GameObject("Ground");
            Undo.RegisterCreatedObjectUndo(groundRoot, "Create Ground Root");
        }

        block.transform.SetParent(groundRoot.transform);
    }

    private static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }


    private void TryEraseBlockAtMousePosition(Event e)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;
            Vector3 gridPos = new Vector3(
                Mathf.Floor(point.x) + 0.5f,
                0f,
                Mathf.Floor(point.z) + 0.5f);

            Collider[] colliders = Physics.OverlapBox(gridPos, Vector3.one * 0.45f);

            foreach (var col in colliders)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(col.gameObject))
                {
                    Undo.DestroyObjectImmediate(col.gameObject);
                    e.Use();
                    break;
                }
            }
        }
    }
}