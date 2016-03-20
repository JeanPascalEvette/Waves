using UnityEngine;
using System.Collections;

public class MeshBuilder : MonoBehaviour {

    GameLogic gLogic;
    private bool isMeshCreated = false;
    private bool isWireFrame = false;

	// Use this for initialization
	void Start () {
        gLogic = GameObject.Find("GameManager").GetComponent<GameLogic>();
    }
	
	// Update is called once per frame
	void Update () {

        if(!isMeshCreated)
        {
            isMeshCreated = true;

            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.vertices = gLogic.GetVertices();
            mesh.triangles = gLogic.GetTriangles();
        }
        else
        {

            var mesh = GetComponent<MeshFilter>().mesh;
            mesh.vertices = gLogic.GetVertices();

            //This makes sure to calculate the normals only once - improving performance greatly
            if (gLogic.GetNormals(transform.localScale.z) == null)
            {
                mesh.RecalculateNormals();
                gLogic.SetNormals(mesh.normals, transform.localScale.z);
            }
            else
                mesh.normals = gLogic.GetNormals(transform.localScale.z);
            if (!isWireFrame)
            {
                float minHeight = GetComponent<MeshRenderer>().bounds.max.y * .7f;
                GetComponent<MeshRenderer>().material.SetFloat("_HeightMin", minHeight);
            }

        }
	}

    public void toggleWireframe()
    {
        if(isWireFrame)
            GetComponent<MeshRenderer>().material = (Material)Resources.Load("Materials/CustomWaveMaterial");
        else
            GetComponent<MeshRenderer>().material = (Material)Resources.Load("Materials/Wireframe");

        isWireFrame = !isWireFrame;
    }
}
