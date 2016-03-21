using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private int currentPosition;

    private Vector3[] positionList;
    private Quaternion[] rotationList;

    // Use this for initialization
    void Start () {

        positionList = new Vector3[] { transform.position, new Vector3(18245, 17684, 22194.06f), new Vector3(2167.666f, 13829.45f, 9980.756f) };
        rotationList = new Quaternion[] { transform.rotation, Quaternion.Euler(new Vector3(82.64277f, 90.1254f, 359.9477f)), Quaternion.Euler(new Vector3(45.68707f, 52.35602f, 359.9915f)) };

    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.F2))
        {
            currentPosition = (currentPosition + 1) % positionList.Length;
            transform.position = positionList[currentPosition];
            transform.rotation = rotationList[currentPosition];
        }
	}
}
