using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Calculate : MonoBehaviour
{
    public bool calculate;
    private bool previousCalculate;

    public float width;
    public float height;

    private Vector2 posRefTop;
    private Vector2 posRefBottom;
    public float dxdy;
    private Vector4 result;

    public List<Vector4> results = new List<Vector4>();

    private void Update()
    {
        if(calculate && calculate != previousCalculate)
        {
            results.Clear();
            DefineRefPos();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector2 v2 = new Vector2(i, j);
                    Vector4 v4 = Calcul(v2);
                    results.Add(v4);
                }
            }
        }
        
        previousCalculate = calculate;
    }

    private void DefineRefPos()
    {
        posRefTop.Set(0f, 0f);
        posRefBottom.Set(width / 2f, height);
        dxdy = Mathf.Abs(posRefBottom.x - posRefTop.x) / Mathf.Abs(posRefBottom.y - posRefTop.y);
    }

    private Vector3 Calcul(Vector2 pos)
    {
        float firstResult = Mathf.Abs(posRefBottom.x - pos.x) / Mathf.Abs(posRefBottom.y - pos.y);
        float secondResult = Mathf.Abs(posRefBottom.x - (pos.x + 1)) / Mathf.Abs(posRefBottom.y - pos.y);

        float avgResult = (firstResult + secondResult) / 2f;
        float v4_4;

        //Debug.Log($"dxdy : {dxdy} ; avgResult : {avgResult}");

        if(avgResult < dxdy)
        {
            v4_4 = 1f;
        }
        else
        {
            v4_4 = 0f;
        }

        result.Set(pos.x, pos.y, avgResult, v4_4);
        return result;
    }

}
