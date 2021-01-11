using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
//public class colorItem
//{

//    public Color32 color = Color.black;

//    public float point;
//}

public class MyGradient : BaseMeshEffect
{
    [SerializeField]
    private Color32 topColor = Color.black;
    [SerializeField]
    private Color32 bottomColor = Color.white;
    [Range(0, 90)]  //取 0 到 90 度
    public float k = 0;
    //对变量序列化，方便在unity3d中改变参数 。
    public override void ModifyMesh(VertexHelper vh)
    //覆盖BaseMeshEffect中的方法，该方法为对该组件网络进行操作,VertexHelper储存了该组件的像素信息
    {
        if (!IsActive())
        {
            return;
        }
        k = k % 90;
        //防止k值大于90 使其强制在0~90°之间
        var vertexList = new List<UIVertex>();

        vh.GetUIVertexStream(vertexList);
        //得到组件像素信息并且储存在vertexList中。
        int count = vertexList.Count;

        ApplyGradient(vertexList, 0, vertexList.Count);
        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexList);
    }
    private void ApplyGradient(List<UIVertex> vertexList, int start, int end)
    {
        float bottomY = vertexList[0].position.y;
        float topY = vertexList[0].position.y;
        float topX = vertexList[0].position.x;
        float bottomX = vertexList[0].position.x;
        for (int i = start; i < end; i++)
        {
            float x = vertexList[i].position.x;
            float y = vertexList[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else
            {
                if (y < bottomY)
                {
                    bottomY = y;
                }
            }
            if (x > topX)
            {
                topX = x;
            }
            else
            {
                if (x < bottomX)
                {
                    bottomX = x;
                }
            }
        }
        float uiElementHeight = topY - bottomY;
        float uiElementWeight = topX - bottomX;
        Vector2 bottomPoint = new Vector2(bottomX, bottomY);
        for (int i = start; i < end; ++i)
        {
            UIVertex uiVertex = vertexList[i];
            if (k % 90 == 0)
            {
                uiVertex.color = Color32.Lerp(bottomColor, topColor, (uiVertex.position.x) / uiElementWeight);
            }
            else
            {
                uiVertex.color = Color32.Lerp(bottomColor, topColor, (getPointLength(bottomPoint, uiVertex.position, k))
                    / getMaxLength(bottomPoint, topX, topY, k));
            }
            vertexList[i] = uiVertex;
        }
    }
    // to do 
    //得到位置到最底端的距离
    float getPointLength(Vector2 source, Vector2 taget, float k)
    {
        Vector2 intersectionOfPoint = getIntersection(taget, source, k);
        return getDistance(intersectionOfPoint, source);
    }
    //得到最大长度
    float getMaxLength(Vector2 source, float maxX, float maxY, float k)
    {
        Vector2 intersectionOfPoint = getIntersection(new Vector2(maxX, maxY), source, k);
        return getDistance(intersectionOfPoint, source);
    }
    //得到两个点的距离
    float getDistance(Vector2 taget, Vector2 source)
    {
        return Mathf.Sqrt(Mathf.Pow(taget.x - source.x, 2) + Mathf.Pow(taget.y - taget.y, 2));
    }
    //已知斜率，其中一个点得到方程式的Y
    float getPonitY(float x, float k, Vector2 source)
    {
        return Mathf.Tan(Mathf.Deg2Rad * k) * x + source.y - Mathf.Tan(Mathf.Deg2Rad * k) * source.x;
    }
    //求两个点所在互相垂直点的交点
    Vector2 getIntersection(Vector2 point, Vector2 bottom, float k)
    {
        float x = (point.y - Mathf.Tan((k + 90) * Mathf.Deg2Rad) * point.x - (bottom.y - Mathf.Tan(Mathf.Deg2Rad * k) * bottom.x))
            / (Mathf.Tan(Mathf.Deg2Rad * k) - Mathf.Tan((k + 90) * Mathf.Deg2Rad));
        float y = getPonitY(x, k, bottom);
        return new Vector2(x, y);
    }
}