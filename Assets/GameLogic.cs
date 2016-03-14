using UnityEngine;
using System.Collections;
using System.IO;

public class GameLogic : MonoBehaviour {

     private SinWave[] waveList;
    private GameObject pointHolder;

    public int currentWave;

    // Use this for initialization
    void Start () {
            waveList = XMLLoader.LoadFile("Assets/Waves.xml");
            pointHolder = new GameObject("PointHolder");

        
        for (int i = 0; i < 360; i++)
        {
            Vector3 point = new Vector3(0, 0, 0);
            for (int u = 0; u < waveList.Length; u++)
            {
                point += waveList[u].ComputeSinValue((float)i / 180.0f);
            }

            GameObject newPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newPoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            newPoint.transform.parent = pointHolder.transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        
        for (int i = 0; i < 360; i++)
        {
            var currentChild = pointHolder.transform.GetChild(i).transform;
            currentChild.position = new Vector3(0,0,0);
            currentChild.position += waveList[currentWave].ComputeSinValue((float)i / 180.0f);
            //for (int u = 0; u < waveList.Length; u++)
            //{
            //    currentChild.position += waveList[u].ComputeSinValue((float)i / 180.0f);
            //}
        }


    }
}
