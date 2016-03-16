﻿using UnityEngine;
using System.Collections;
using System.IO;

public class GameLogic : MonoBehaviour {

    private SinWave[] waveList;
    private Vector3[][] positions;
    private Vector3[][] newPositions;

    public int currentWave;

    public int numRows = 50;
    public float distance = 10.0f;
    public float surfaceSize = 3.0f;
    public float speed = 1.0f;
    [SerializeField]
    private int NumerOfTilesPerRow = 10;

    // Use this for initialization
    void Start() {
        waveList = XMLLoader.LoadFile("Assets/Waves.xml");


        positions = new Vector3[numRows][];
        newPositions = new Vector3[numRows][];

        var halfDist = (numRows-1) * distance / 2;
        for (int x = 0; x < numRows; x++)
        {
            positions[x] = new Vector3[numRows];
            newPositions[x] = new Vector3[numRows];
            for (int z = 0; z < numRows; z++)
            {
                positions[x][z] = new Vector3(-halfDist + x * distance, 0, -halfDist + z * distance);
                newPositions[x][z] = new Vector3(-halfDist + x * distance, 0, -halfDist + z * distance);
            }
        }

        Vector3 position = Vector3.zero;

        var tile = (GameObject)Resources.Load("Prefabs/WavesTile");
        Vector3 scale = tile.transform.localScale;
        Vector3 originalScale = tile.transform.localScale;
        for (int x = 0; x < NumerOfTilesPerRow; x++)
        {
            position.z = 0;
            scale.z = originalScale.z;
            for (int y = 0; y < NumerOfTilesPerRow; y++)
            {
                position.z += (numRows-1) * distance * originalScale.z;
                scale.z *= -1;
                tile = (GameObject)Instantiate(Resources.Load("Prefabs/WavesTile"), position, Quaternion.identity);
                tile.transform.localScale = scale;
            }
            scale.x *= -1;
            position.x += (numRows-1) * distance * originalScale.x;
        }

        
    }
	
	// Update is called once per frame
	void Update () {
        for (int x = 0; x < numRows; x++)
        {
            for (int z = 0; z < numRows; z++)
            {
                var currentChild = newPositions[x][z];
                var currentRelativePos = currentChild / (numRows * distance) * 2.0f * surfaceSize;
                
                for (int u = 0; u < waveList.Length; u++)
                {
                    var angle = waveList[u].angle;
                    if (angle != 90.0f && angle != 360.0f && angle != 180.0f && angle != 270.0f && angle != 0.0f)
                        angle = 0.0f;
                    var newPos = 0.0f;
                    newPos += (waveList[u].ComputeSinValue(currentRelativePos.x + Time.time * speed, currentRelativePos.z + Time.time * speed) * numRows * distance);
                    var newTarget = Quaternion.AngleAxis(angle, Vector3.up) * new Vector3(x, 0, z);
                    if (angle == 270.0f || angle == 180.0f)
                        newTarget.x += numRows - 1;
                    if (angle == 90.0f || angle == 180.0f)
                        newTarget.z += numRows - 1;
                    newPositions[Mathf.RoundToInt(newTarget.x)][Mathf.RoundToInt(newTarget.z)] += new Vector3(0, newPos, 0);
                }
            }
        }

        int i = 0;
        for (int x = 0; x < numRows; x++)
        {
            for (int z = 0; z < numRows; z++)
            {
                newPositions[x][z].y /= waveList.Length;
                positions[x][z].y = newPositions[x][z].y;
                newPositions[x][z].y = 0.0f;
            }
        }



        
    }

    public int[] GetTriangles()
    {
        int[] triangles = new int[(numRows*numRows)*3*2];
        int i = 0;
        for (int x = 0; x < numRows - 1; x++)
        {
            for (int z = 0; z < numRows - 1; z++)
            {
                triangles[i++] = (x * numRows) + z;
                triangles[i++] = (x * numRows) + z + 1;
                triangles[i++] = ((x + 1) * numRows) + z;
            }
        }


        for (int x = 0; x < numRows - 1; x++)
        {
            for (int z = 0; z < numRows - 1; z++)
            {
                triangles[i++] = (x * numRows) + z + 1;
                triangles[i++] = ((x + 1) * numRows) + z + 1;
                triangles[i++] = ((x + 1) * numRows) + z;
            }
        }



        return triangles;
    }

    public Vector3[] GetVertices()
    {
        Vector3[] vertices = new Vector3[numRows * numRows * 2];
        int i = 0;
        for (int x = 0; x < numRows; x++)
        {
            for (int z = 0; z < numRows; z++)
            {
                vertices[i++] = positions[x][z];
            }
        }
        for (int x = 0; x < numRows; x++)
        {
            for (int z = 0; z < numRows; z++)
            {
                vertices[i++] = positions[x][z];
            }
        }
        return vertices;
    }
}