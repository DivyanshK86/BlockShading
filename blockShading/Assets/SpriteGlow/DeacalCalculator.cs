using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Side
{
    public Vector3 dir;
    public bool occluded;
}

[System.Serializable]
public class Corner
{
    public Vector3 dir;
    public bool dontCheck;
    public bool cornerOccluded;
}

public class DeacalCalculator : MonoBehaviour {
   
    List<Side> sides = new List<Side>();
    List<Corner> corners = new List<Corner>();

    public int shadePixelSize = 10;
    public Color ShadePixelColor = Color.black;


    public void CalCulateDecal(Transform face)
    {
        sides = new List<Side>();
        corners = new List<Corner>();
        InitializeSidesAndCorners();
        CheckSidesOccluders(face);
        SetIfCornerCheckNeeded();
        CheckCornerOccluders(face);
        GenerateTexture(face);
    }

    void InitializeSidesAndCorners()
    {
        for(int i=0; i<4; i++)
        {
            sides.Add(new Side());
            corners.Add(new Corner());
        }

        sides[0].dir = Vector3.forward;
        sides[1].dir = Vector3.right;
        sides[2].dir = Vector3.back;
        sides[3].dir = Vector3.left;

        corners[0].dir = new Vector3(-1, 0, 1);
        corners[1].dir = new Vector3(1, 0, 1);
        corners[2].dir = new Vector3(1, 0, -1);
        corners[3].dir = new Vector3(-1, 0, -1);
    }

    void CheckSidesOccluders(Transform face)
    {
        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            Vector3 currentDirection = sides[i].dir;
            if (Physics.Raycast(face.position - face.forward * 0.6f, face.GetChild(0).TransformDirection(currentDirection) , 0.6f))
            {
                sides[i].occluded = true;
            }
        }
    }

    void SetIfCornerCheckNeeded()
    {
        for(int i=0; i<4; i++)
        {
            if(sides[i].occluded)
            if(i == 3)
            {
                corners[corners.Count - 1].dontCheck = true;
                corners[0].dontCheck = true;
            }
            else
            {
                corners[i].dontCheck = true;
                corners[i+1].dontCheck = true;
            }
        }
    }

    void CheckCornerOccluders(Transform face)
    {
        for (int i = 0; i < 4; i++)
        {
            if(!corners[i].dontCheck)
            {
                RaycastHit hit;
                Vector3 currentDirection = corners[i].dir;
                if (Physics.Raycast(face.position - face.forward * 0.6f, face.GetChild(0).TransformDirection(currentDirection), out hit, 1f))
                {
                    corners[i].cornerOccluded = true;
                }
                else
                {

                }
            }
        }
    }

    void GenerateTexture(Transform face)
    {
        var texture = new Texture2D(shadePixelSize, shadePixelSize, TextureFormat.ARGB32, false);

        for (int i = 0; i < texture.width; i++)
            for (int j = 0; j < texture.height; j++)
                texture.SetPixel(i, j, Color.white);

        //sides

        if (sides[0].occluded)
        {
            for (int i = 0; i < texture.width; i++)
                texture.SetPixel(i, texture.height - 1, ShadePixelColor);
        }

        if (sides[1].occluded)
        {
            for (int j = 0; j < texture.height; j++)
                texture.SetPixel(texture.width - 1, j, ShadePixelColor);
        }

        if (sides[2].occluded)
        {
            for (int i = 0; i < texture.width; i++)
                texture.SetPixel(i, 0, ShadePixelColor);
        }

        if (sides[3].occluded)
        {
            for (int j = 0; j < texture.height; j++)
                texture.SetPixel(0, j, ShadePixelColor);
        }

        //corners

        if (corners[0].cornerOccluded)
            texture.SetPixel(0, texture.height - 1, ShadePixelColor);

        if (corners[1].cornerOccluded)
            texture.SetPixel(texture.width - 1, texture.height - 1, ShadePixelColor);

        if (corners[2].cornerOccluded)
            texture.SetPixel(texture.width - 1, 0, ShadePixelColor);
        if (corners[3].cornerOccluded)
            texture.SetPixel(0, 0, ShadePixelColor);

        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        face.GetComponent<MeshRenderer>().material.mainTexture = texture;
        //face.transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + 180, transform.localRotation.eulerAngles.z);
    }
}
