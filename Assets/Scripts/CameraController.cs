using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private int currentPosition;

    private Vector3[] positionList;
    private Quaternion[] rotationList;

    private GameLogic gLogic;

    // Use this for initialization
    void Start () {
        gLogic = GameObject.Find("GameManager").GetComponent<GameLogic>();
        positionList = new Vector3[] { transform.position, new Vector3(18245, 17684, 22194.06f), new Vector3(2167.666f, 13829.45f, 9980.756f) };
        rotationList = new Quaternion[] { transform.rotation, Quaternion.Euler(new Vector3(82.64277f, 90.1254f, 359.9477f)), Quaternion.Euler(new Vector3(45.68707f, 52.35602f, 359.9915f)) };

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.F2))
        {
            currentPosition = (currentPosition + 1) % positionList.Length;
        }
        if (gLogic.NumerOfTilesPerRow == 1)
        {
            transform.position = new Vector3(3039.746f, 5693.979f, 9048.424f);
            transform.rotation = Quaternion.Euler(32.69535f, 219.0163f, 0.0001349311f);
        }
        else
        {
            transform.position = positionList[currentPosition];
            transform.rotation = rotationList[currentPosition];
        }
	}
}
