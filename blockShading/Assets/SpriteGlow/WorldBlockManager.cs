using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBlockManager : MonoBehaviour {

    public GameObject quadFacePrefab;

    public Transform cubesHolder;
    public Transform faceHolder;

    DeacalCalculator dc;

	void Start () {

        dc = GetComponent<DeacalCalculator>();

        foreach(Transform child in cubesHolder)
        {
            CheckAndTakeAction(child);
        }

        foreach(Transform child in faceHolder)
        {
            dc.CalCulateDecal(child);
        }
	}
	
    void CheckAndTakeAction(Transform block)
    {
        GenerateBlockFaces(CheckBlockOcclusion(block), block);
    }

    List<Vector3> CheckBlockOcclusion(Transform blockTransform)
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
                if (Physics.Raycast(blockTransform.position, currentDirection, out hit, 0.6f))
                {
/*                    if (hit.collider != null)
                        Debug.Log(currentDirection);*/
                } 
                else
                {
/*                    Debug.Log("not occ : " + currentDirection);
*/
                    FacesNotOccluded.Add(currentDirection);
                }
            }
        return FacesNotOccluded;
    }

    void GenerateBlockFaces(List<Vector3> FacesNotOccluded, Transform blockTransform)
    {
        foreach(Vector3 dir in FacesNotOccluded)
        {
            GameObject face = Instantiate(quadFacePrefab,blockTransform.position + ( dir * 0.5f),Quaternion.LookRotation(-dir)) as GameObject;
            face.transform.SetParent(faceHolder);
        }
        blockTransform.GetComponent<MeshRenderer>().enabled = false;
    }
}
