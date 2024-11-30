using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour, IPunObservable
{
    public List<Vector3> points = new List<Vector3>();

    public LineRenderer lineRenderer;

    [Header("그리기")]
    public float lineWidth = 0.1f;
    public Color lineColor = Color.red;
    public Material lineMaterial;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;
    }
    
    public void AddPoint(Vector3 point)
    {
        points.Add(point);

        UpdateLineRenderer();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 점의 개수 보냄
            stream.SendNext(points.Count);

            foreach (Vector3 point in points)
            {
                // 점의 좌표 보냄
                stream.SendNext(point);
            }
        }
        else
        {
            // 점의 개수 받음
            int count = (int)stream.ReceiveNext();

            points.Clear();

            for (int i = 0; i < count; i++)
            {
                // 점의 좌표를 리스트에 추가
                points.Add((Vector3)stream.ReceiveNext());
            }

            UpdateLineRenderer();
        }
    }

    public void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
