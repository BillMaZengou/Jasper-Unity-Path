using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10, 100)]
    public int resolution = 10;
    Transform[] points;

    private void Awake()
    {
        //Transform point = Instantiate(pointPrefab);
        //point.localPosition = Vector3.right;

        ////Transform point = Instantiate(pointPrefab);
        //point = Instantiate(pointPrefab);
        //point.localPosition = Vector3.right * 2f;
        //int i = 0;
        ////while (i < 10) {
        //while (++i < 10)
        //{
        //    //i++;
        //    Transform point = Instantiate(pointPrefab);
        //    point.localPosition = Vector3.right * i;
        //    //i++;
        //}
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position = Vector3.zero;
        points = new Transform[resolution];

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            //point.localPosition = Vector3.right * ((i + 0.5f) / 5f - 1f);
            position.x = (i + 0.5f) * step - 1f;
            //position.y = position.x * position.x * position.x;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    private void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
            point.localPosition = position;
        }
    }
}
