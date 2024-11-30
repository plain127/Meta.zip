using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour, IPunObservable
{
    public List<Vector3> points = new List<Vector3>();

    public LineRenderer lineRenderer;

    [Header("�׸���")]
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
            // ���� ���� ����
            stream.SendNext(points.Count);

            foreach (Vector3 point in points)
            {
                // ���� ��ǥ ����
                stream.SendNext(point);
            }
        }
        else
        {
            // ���� ���� ����
            int count = (int)stream.ReceiveNext();

            points.Clear();

            for (int i = 0; i < count; i++)
            {
                // ���� ��ǥ�� ����Ʈ�� �߰�
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
