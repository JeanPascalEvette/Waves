using UnityEngine;
using System.Collections;

public class MeshBuilder : MonoBehaviour {

    GameLogic gLogic;
    private bool isMeshCreated = false;

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
        }
	}
}
