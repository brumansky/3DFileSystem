using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour
{
    public Material A;
    public Material B;
    public Material C;
    public Material D;
    public Material E;
    public Material F;
    public Material G;
    public Material H;
    public Material I;
    public Material J;
    public Material K;
    public Material L;
    public Material M;
    public Material N;
    public Material O;
    public Material P;
    public Material Q;
    public Material R;
    public Material S;
    public Material T;
    public Material U;
    public Material V;
    public Material W;
    public Material X;
    public Material Y;
    public Material Z;
    public Material Number;

    public void ColorObject(GameObject target)
    {
        target.GetComponent<MeshRenderer>().material = 
            GetMaterialByLetter(target.name.Substring(0, 1).ToLower());
    }
    public void ColorChildObjects(GameObject target)
    {
        Material mat = GetMaterialByLetter(target.name.Substring(0, 1).ToLower());

        MeshRenderer[] meshes = target.GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshes)
        {
            mesh.material = mat;
        }
    }

    public Material GetMaterialByLetter(String letter)
    {
        switch (letter)
        {
            case "a":
                return A;
            case "b":
                return B;
            case "c":
                return C;
            case "d":
                return D;
            case "e":
                return E;
            case "f":
                return F;
            case "g":
                return G;
            case "h":
                return H;
            case "i":
                return I;
            case "j":
                return J;
            case "k":
                return K;
            case "l":
                return L;
            case "m":
                return M;
            case "n":
                return N;
            case "o":
                return O;
            case "p":
                return P;
            case "q":
                return Q;
            case "r":
                return R;
            case "s":
                return S;
            case "t":
                return T;
            case "u":
                return U;
            case "v":
                return V;
            case "w":
                return W;
            case "x":
                return X;
            case "y":
                return Y;
            case "z":
                return Z;
            default:
                return Number;
        }
    }
}
