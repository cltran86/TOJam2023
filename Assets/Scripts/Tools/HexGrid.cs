using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum Orientation
{
    Horizontal,
    Vertical
}
public class HexGrid : MonoBehaviour
{
    [Tooltip("In which direction are tiles lined up straight?")]
    [SerializeField]
    private Orientation gridOrientation;

    [Tooltip("Must be at least (1, 1).")]
    [SerializeField]
    private Vector2Int gridDimensions = Vector2Int.one;

    [Tooltip("The distance between 2 opposite edges of the hexagon.")]
    [SerializeField]
    private float tileDiameter = 1f;

    [Tooltip("A blank tile.")]
    [SerializeField]
    public HexTile[] tilePrefabs;

    private HexTile[,] tiles;

    [SerializeField]
    private Camera minimapCamera;

    public Vector3 GridToWorldCoordinates(Vector2Int gridCoordinates)
    {
        if(gridCoordinates.x < 0 || gridCoordinates.y < 0 || gridCoordinates.x >= gridDimensions.x || gridCoordinates.y >= gridDimensions.y)
            throw new ArgumentOutOfRangeException("Grid Coordinates", gridCoordinates, "Must be within 0 and grid dimensions");

        Vector3 worldCoordinates = transform.position;

        if(gridOrientation == Orientation.Horizontal)
        {
            worldCoordinates.x += (gridCoordinates.x + (gridCoordinates.y % 2 == 0 ? 0 : 0.5f)) * tileDiameter;
            worldCoordinates.z += gridCoordinates.y * tileDiameter * 0.866f;
        }
        else
        {
            worldCoordinates.x += gridCoordinates.x * tileDiameter * 0.866f;
            worldCoordinates.z += (gridCoordinates.y + (gridCoordinates.x % 2 == 0 ? 0 : 0.5f)) * tileDiameter;
        }
        return worldCoordinates;
    }
    public void GenerateTiles()
    {
        if (gridDimensions.x < 1 || gridDimensions.y < 1)
            Debug.LogError("Grid Dimensions must be greater than 0.");

        //  Get any children that currently exist to this transform and organise them

        tiles = new HexTile[gridDimensions.x, gridDimensions.y];

        HexTile newTile;

        for(int z = 0; z != gridDimensions.y; ++z)
        {
            for(int x = 0; x != gridDimensions.x; ++x)
            {
                newTile = Instantiate(tilePrefabs[0], GridToWorldCoordinates(new Vector2Int(x, z)), Quaternion.identity, transform);
                newTile.gameObject.name = "Tile (" + x + ", " + z + ")";
                newTile.gridCoordinates = new Vector2Int(x, z);
            }
        }
    }
    public void ClearTiles()
    {
        if (transform.childCount == 0)
        {
            Debug.LogWarning("There are no tiles to clear.");
            return;
        }

        Transform[] children = transform.GetComponentsInChildren<Transform>();

        for(int i = 1; i != children.Length; ++i)
        {
            if(children[i] != null && children[i].GetComponent<HexTile>())
                DestroyImmediate(children[i].gameObject);
        }
    }
    public void FitMinimapCamera()
    {
        if(!minimapCamera)
        {
            Debug.LogWarning("Minimap Camera has not been set.");
            return;
        }
        Vector3 position = Vector3.up * 10;
        position.x = (gridDimensions.x - 1) * tileDiameter / 2 * (gridOrientation == Orientation.Horizontal ? 1 : 0.866f);
        position.z = (gridDimensions.y - 1) * tileDiameter / 2 * (gridOrientation == Orientation.Vertical ? 1 : 0.866f);

        minimapCamera.transform.position = position;
        minimapCamera.orthographicSize = gridDimensions.y * tileDiameter / 2 * 0.866f;
    }
}
[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    private int selected = 0;

    private GUIContent[] tileOptions;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexGrid hexGrid = (HexGrid)target;

        if (GUILayout.Button("Generate Tiles"))
            hexGrid.GenerateTiles();

        if (GUILayout.Button("Clear Tiles"))
            hexGrid.ClearTiles();

        if (GUILayout.Button("Fit Minimap Camera"))
            hexGrid.FitMinimapCamera();

        tileOptions = new GUIContent[hexGrid.tilePrefabs.Length];

        for(int i = 0; i != tileOptions.Length; ++i)
        {
            tileOptions[i] = new GUIContent(hexGrid.tilePrefabs[i].gameObject.name);
        }

        selected = GUILayout.SelectionGrid(selected, tileOptions, 5, new GUILayoutOption[0]);
    }
}