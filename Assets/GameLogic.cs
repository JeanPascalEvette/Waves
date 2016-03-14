using UnityEngine;
using System.Collections;
using System.IO;

public class GameLogic : MonoBehaviour {

    private SinWave[] waveList;
    private Vector3[][] positions;
    private Vector3[][] newPositions;
    private GameObject pointHolder;

    public int currentWave;

    public int numRows = 50;
    public float distance = 10.0f;
    public float surfaceSize = 3.0f;
    public float speed = 1.0f;

    // Use this for initialization
    void Start () {
            waveList = XMLLoader.LoadFile("Assets/Waves.xml");
            pointHolder = new GameObject("PointHolder");


        positions = new Vector3[numRows][];
        newPositions = new Vector3[numRows][];
        for (int x = 0; x < numRows; x++)
        {
            positions[x] = new Vector3[numRows];
            newPositions[x] = new Vector3[numRows];
            for (int z = 0; z < numRows; z++)
            {
                positions[x][z] = new Vector3(x * distance, 0, z * distance);
                newPositions[x][z] = new Vector3(x * distance, 0, z * distance);
            }
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
