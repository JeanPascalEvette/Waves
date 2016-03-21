using UnityEngine;
using System.Collections;
using System.IO;

public class GameLogic : MonoBehaviour {

    private SinWave[] waveList;
    private Vector3[][] positions;
    private Vector3[][] newPositions;
    private Transform Waves;


    public int currentWave;

    public int numRows = 50;
    public float distance = 10.0f;
    public float Frequency = 1.0f;
    public float speed = 1.0f;
    [SerializeField]
    private int NumerOfTilesPerRow = 10;
    [SerializeField]
    private int NumberWaves = 1;

    [SerializeField]
    private GUISkin aSkin;


    private int[] triangles = null;
    private Vector3[] vertices = null;
    private Vector3[] normalsPos = null;
    private Vector3[] normalsNeg = null;


    private bool displayGUI = false;
    private bool multByPI = false;

    private int GUIXCoord = 0;
    private int GUIYCoord = 0;

    // Use this for initialization
    void Start() {
        numRows += 1; // Most calculation need to be +1'd
        waveList = XMLLoader.LoadFile("Assets/Waves.xml");

        if (Waves == null)
            Waves = GameObject.Find("Waves").transform;
        if (Waves == null)
            Waves = new GameObject("Waves").transform;
        Waves.transform.position = new Vector3(0, 0, 0);


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
                tile.transform.parent = Waves;
            }
            scale.x *= -1;
            position.x += (numRows-1) * distance * originalScale.x;
        }



}


    // Update is called once per frame
    void Update () {
        

        //Waves computation

        for (int x = 0; x < numRows; x++)
        {
            for (int z = 0; z < numRows; z++)
            {
                var currentChild = positions[x][z];
                var currentRelativePos = currentChild / ((numRows-1) * distance) * (NumberWaves*distance/100) * 10.0f * Frequency;
                

                for (int u = 0; u < waveList.Length; u++)
                {
                    //Calculate Y
                    var newPos = 0.0f;
                    newPos += (waveList[u].ComputeSinValue(currentRelativePos.x + Time.time * speed, currentRelativePos.z + Time.time * speed, multByPI) * numRows * distance / 100);
                    

                    //Calculate x/z
                    var angle = waveList[u].angle;
                    if (angle != 90.0f && angle != 360.0f && angle != 180.0f && angle != 270.0f && angle != 0.0f)
                        angle = 0.0f;
                    var newTarget = Quaternion.AngleAxis(angle, Vector3.up) * new Vector3(x, 0, z);
                    if (angle == 270.0f || angle == 180.0f)
                        newTarget.x += numRows - 1;
                    if (angle == 90.0f || angle == 180.0f)
                        newTarget.z += numRows - 1;
                    newPositions[Mathf.RoundToInt(newTarget.x)][Mathf.RoundToInt(newTarget.z)] += new Vector3(0, newPos / waveList.Length, 0);
                }
            }
        }
        
        for (int x = 0; x < numRows; x++)
        {
            for (int z = 0; z < numRows; z++)
            {
                positions[x][z].y = newPositions[x][z].y;
                newPositions[x][z].y = 0.0f;
            }
        }


        triangles = null;
        vertices = null;
        normalsPos = null;
        normalsNeg = null;






        //Inputs
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int u = 0; u < Waves.childCount; u++)
            {
                Waves.GetChild(u).GetComponent<MeshBuilder>().toggleWireframe();
            }
        }
        else if(Input.GetKeyDown(KeyCode.T))
        {
            multByPI = !multByPI;
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            displayGUI = !displayGUI;
        }

        if (displayGUI)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                GUIYCoord = (GUIYCoord + 1) % waveList.Length;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                GUIYCoord = (GUIYCoord - 1);
                if (GUIYCoord < 0)
                    GUIYCoord = waveList.Length - 1;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                GUIXCoord = (GUIXCoord - 1);
                if (GUIXCoord < -1)
                    GUIXCoord = 3;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                GUIXCoord = (GUIXCoord + 2) % 5 - 1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (GUIXCoord == -1)
                {
                    waveList[GUIYCoord].ToggleDisabled();
                }
                else
                {
                    float diff = 0.1f;
                    if (Input.GetKey(KeyCode.LeftShift))
                        diff = 0.5f;
                    switch (GUIXCoord)
                    {
                        case 0:
                            waveList[GUIYCoord].A += diff;
                            break;
                        case 1:
                            waveList[GUIYCoord].B += diff;
                            break;
                        case 2:
                            waveList[GUIYCoord].C += diff;
                            break;
                        case 3:
                            waveList[GUIYCoord].D += diff;
                            break;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (GUIXCoord == -1)
                {
                    waveList[GUIYCoord].ToggleDisabled();
                }
                else
                {
                    float diff = 0.1f;
                    if (Input.GetKey(KeyCode.LeftShift))
                        diff = 0.5f;
                    switch (GUIXCoord)
                    {
                        case 0:
                            waveList[GUIYCoord].A -= diff;
                            break;
                        case 1:
                            waveList[GUIYCoord].B -= diff;
                            break;
                        case 2:
                            waveList[GUIYCoord].C -= diff;
                            break;
                        case 3:
                            waveList[GUIYCoord].D -= diff;
                            break;
                    }
                }
            }
        }



    }



    void OnGUI()
    {

        if (!displayGUI) return;
        var style = new GUIStyle(aSkin.GetStyle("box"));
        var styleHeader = new GUIStyle(aSkin.GetStyle("box"));
        var styleSelected = new GUIStyle(aSkin.GetStyle("box"));
        var styleDisabled = new GUIStyle(aSkin.GetStyle("box"));
        styleHeader.fontStyle = FontStyle.Bold;
        styleHeader.normal.textColor = Color.black;
        style.normal.textColor = Color.green;
        styleSelected.normal.textColor = Color.blue;
        styleDisabled.normal.textColor = Color.red;

        float xCell = 80.0f;
        float xMargin = 10.0f;
        float yCell = 20.0f;
        float yMargin = 5.0f;

        Vector3 CurrentPos = new Vector3(xMargin, yMargin, 0);

        GUI.Label(new Rect(CurrentPos.x, CurrentPos.y, xCell, yCell), "Wave", styleHeader);
        GUI.Label(new Rect(CurrentPos.x + (xCell + xMargin) * 1, CurrentPos.y, xCell, yCell), "A", styleHeader);
        GUI.Label(new Rect(CurrentPos.x + (xCell + xMargin) * 2, CurrentPos.y, xCell, yCell), "B", styleHeader);
        GUI.Label(new Rect(CurrentPos.x + (xCell + xMargin) * 3, CurrentPos.y, xCell, yCell), "C", styleHeader);
        GUI.Label(new Rect(CurrentPos.x + (xCell + xMargin) * 4, CurrentPos.y, xCell, yCell), "D", styleHeader);
        CurrentPos.y += yCell + yMargin;

        for (int y = 0; y < waveList.Length; y++)
        {
            if (GUIXCoord == -1 && GUIYCoord == y)
                GUI.Label(new Rect(CurrentPos.x, CurrentPos.y, xCell, yCell), "Wave " + y, styleSelected);
            else if(waveList[y].IsDisabled())
                GUI.Label(new Rect(CurrentPos.x, CurrentPos.y, xCell, yCell), "Wave " + y, styleDisabled);
            else
                GUI.Label(new Rect(CurrentPos.x, CurrentPos.y, xCell, yCell), "Wave " + y, style);


            for(int x = 1; x <= 4; x++)
            {
                float value = 0f;
                switch(x)
                {
                    case 1: value = waveList[y].A; break;
                    case 2: value = waveList[y].B; break;
                    case 3: value = waveList[y].C; break;
                    case 4: value = waveList[y].D; break;
                }
                if (GUIXCoord == x - 1 && GUIYCoord == y)
                    GUI.Label(new Rect(CurrentPos.x + (xCell + xMargin) * x, CurrentPos.y, xCell, yCell), value.ToString("F1"), styleSelected);
                else if (waveList[y].IsDisabled())
                    GUI.Label(new Rect(CurrentPos.x + (xCell + xMargin) * x, CurrentPos.y, xCell, yCell), value.ToString("F1"), styleDisabled);
                else
                    GUI.Label(new Rect(CurrentPos.x + (xCell + xMargin) * x, CurrentPos.y, xCell, yCell), value.ToString("F1"), style);
            }
            

            CurrentPos.y += yCell + yMargin;
        }
    }



    public Vector3[] GetNormals(float scale)
    {
        if (scale > 0)
            return normalsPos;
        else
            return normalsNeg;
    }

    public void SetNormals(Vector3[] newNormals, float scale)
    {
        if (scale > 0 && normalsPos == null)
            normalsPos = newNormals;
        if (scale < 0 && normalsNeg == null)
            normalsNeg = newNormals;
    }

    public int[] GetTriangles()
    {
        if (triangles != null) return triangles;
        triangles = new int[(numRows*numRows)*3*2];
        int i = 0;
        for (int x = 0; x < numRows-1; x++)
        {
            for (int z = 0; z < numRows-1; z++)
            {
                triangles[i++] = (x * numRows) + z; // 0, 0
                triangles[i++] = (x * numRows) + z + 1; // 0, 1
                triangles[i++] = ((x + 1) * numRows) + z; // 1, 0


                triangles[i++] = ((x + 1) * numRows) + z; // 1, 0
                triangles[i++] = (x * numRows) + z + 1; //  0, 1
                triangles[i++] = ((x + 1) * numRows) + z + 1; // 1, 1

                
            }
        }
        


        return triangles;
    }

    public Vector3[] GetVertices()
    {
        if (vertices != null) return vertices;
        vertices = new Vector3[numRows * numRows * 2];
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
