using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water_wave : MonoBehaviour
{
    public MeshFilter mesh_filter;

    public int column_count =10; // 몇개만들지
    public float width = 2f; //물표면 너비
    public float height = 1f; //물표면 높이
    public float k = 0.025f;  //용수철 상수
    public float m = 1;
    public float drag = 0.025f; //저항
    public float spread = 0.025f; //얼마나 튈지
    public float power = -1f; //파워

    private List<water_column> columns = new List<water_column>(); //리스트로 만듬


    private void Start()
    {
        setup();
    }



    private void setup()
    {
        columns.Clear();
        float space = width / column_count;
        for (int i = 0; i < column_count+1; i++)
        {
            columns.Add(new water_column(i * space - width * 0.5f, height, k, m, drag));
        }
    }

    internal int? WorldToColumn(Vector2 position)
    {
        float space = width / column_count;
        int result = Mathf.RoundToInt((position.x + width*0.5f) / space);
        if(result >= columns.Count || result < 0)
        {
            return null;
        }
        return result;
    }

    private void Update()
    {
        int? column = WorldToColumn(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButtonDown(0) && column.HasValue)
        {
            columns[column.Value].velocity = power;
        }
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < columns.Count; i++)
        {
            columns[i].update_column();
        }

        //양옆 파동
        float[] left_deltas = new float[columns.Count];
        float[] right_deltas = new float[columns.Count];
        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
            {
                left_deltas[i] = (columns[i].height - columns[i - 1].height) * spread;
                columns[i-1].velocity += left_deltas[i];
            }
            if(i<columns.Count - 1)
            {
                right_deltas[i] = (columns[i].height - columns[i + 1].height) * spread;
                columns[i+1].velocity += right_deltas[i];
            }
        }
        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0)
            {
                columns[i - 1].height += left_deltas[i];
            }
            if (i < columns.Count - 1)
            {
                columns[i + 1].height += right_deltas[i];
            }
        }

        //사각형을 만들어준다. 물밑에를 채우기용
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[columns.Count * 2];
        int v = 0;
        for (int i = 0; i < columns.Count; i++)
        {
            vertices[v] = new Vector2(columns[i].x_pos, columns[i].height);
            vertices[v + 1] = new Vector2(columns[i].x_pos, 0f);

            v += 2;
        }

        //삼각형 매쉬
        int[] triangles = new int[(columns.Count - 1) * 6];
        int t = 0;
        v = 0;
        for (int i = 0; i < columns.Count -1; i++)
        {
            triangles[t] = v;
            triangles[t + 1] = v + 2;
            triangles[t + 2] = v + 1;
            triangles[t + 3] = v + 1;
            triangles[t + 4] = v + 2;
            triangles[t + 5] = v + 3;

            v += 2;
            t += 6;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        mesh_filter.mesh = mesh;
    }



    public class water_column
    {
        //x의 좌표, 높이 , 목표높이, 용수철 상수,질량, 속도, 저항 
        public float x_pos, height, target_height, k, m, velocity, drag;

        public water_column(float x_pos, float target_height, float k, float m, float drag)
        {
            this.x_pos = x_pos;
            this.height = target_height;
            this.target_height = target_height;
            this.k = k;
            this.m = m;
            this.drag = drag;
        }

        public void update_column()
        {
            // a는 가속도 = f=ma = f=-km
            float a = -k / m * (height - target_height);
            velocity += a; //가속도에 속도 계산
            velocity -= drag * velocity;// 공기저항
            height += velocity; //높이에 속도 더함
        }
    }
}
