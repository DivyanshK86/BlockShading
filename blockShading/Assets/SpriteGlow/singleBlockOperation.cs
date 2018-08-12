using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleBlockOperation : MonoBehaviour {

    public GameObject quadFacePrefab;

    void Start()
    {
        GenerateBlockFaces(CheckBlockOcclusion());
    }

    List<Vector3> CheckBlockOcclusion()
    {
        Vector3[] directionArray = new Vector3[3];
        List<Vector3> FacesNotOccluded = new List<Vector3>();

        directionArray[0] = Vector3.up;
        directionArray[1] = Vector3.right;
        directionArray[2] = Vector3.forward;

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 2; j++)
            {
                int m = 1;
                if (j == 1)
                    m = -1;

                RaycastHit hit;
                Vector3 currentDirection = directionArray[i] * m;
                if (Physics.Raycast(transform.position, currentDirection, 0.6f))
                {

                } 
                else
                {
                    FacesNotOccluded.Add(currentDirection);
                }
            }
        return FacesNotOccluded;
    }

    void GenerateBlockFaces(List<Vector3> FacesNotOccluded)
    {
        foreach(Vector3 dir in FacesNotOccluded)
        {
            GameObject face = Instantiate(quadFacePrefab,transform.position,Quaternion.LookRotation(dir)) as GameObject;
        }
        transform.GetComponent<MeshRenderer>().enabled = false;
    }
}
