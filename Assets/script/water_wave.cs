using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water_wave : MonoBehaviour
{
    public MeshFilter mesh_filter;

    public int column_count; // �������
    public float width = 2f; //��ǥ�� �ʺ�
    public float height = 2f; //��ǥ�� ����
    public float k = 0.025f;  //���ö ���
    public float m = 1;
    public float drag = 0.025f; //����
    public float spread = 0.025f; //�󸶳� ƥ��

    private List<water_column> columns = new List<water_column>(); //����Ʈ�� ����


    private void Start()
    {
        setup();
    }



    void setup()
    {
        columns.Clear();
        float space = width / column_count;
        for (int i = 0; i < column_count+1; i++)
        {
            columns.Add(new water_column(i * space - width * 0.5f, height, k, m, drag));
        }
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < columns.Count; i++)
        {
            columns[i].update_column();
        }

        //�翷 �ĵ�
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
                columns[i+1].velocity += left_deltas[i];
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

        //�簢���� ������ش�. ���ؿ��� ä����
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[columns.Count * 2];
        int v = 0;
        for (int i = 0; i < columns.Count; i++)
        {
            vertices[v] = new Vector2(columns[i].x_pos, columns[i].height);
            vertices[v + 1] = new Vector2(columns[i].x_pos, 0f);

            v += 2;
        }
        //�ﰢ��
        int[] traingles = new int[(columns.Count - 1) * 6];
        int t = 0;
        v = 0;
        for (int i = 0; i < columns.Count -1; i++)
        {
            traingles[t] = v;
            traingles[t + 1] = v + 2;
            traingles[t + 2] = v + 1;
            traingles[t + 3] = v + 1;
            traingles[t + 4] = v + 2;
            traingles[t + 5] = v + 3;

            v += 2;
            v += 6;
        }

        mesh.vertices = vertices;
        mesh.triangles = traingles;
    }



    public class water_column
    {
        //x�� ��ǥ, ���� , ��ǥ����, ���ö ���,����, �ӵ�, ���� 
        public float x_pos, height, target_height, k, m, velocity, drag;

        public water_column(float x_pos, float target_height, float k, float m, float drag)
        {
            this.x_pos = x_pos;
            this.target_height = target_height;
            this.k = k;
            this.m = m;
            this.drag = drag;
        }

        public void update_column()
        {
            // a�� ���ӵ� = f=ma 
            float a = k / m * (height * target_height);
            velocity += a; //���ӵ��� �ӵ� ���
            velocity -= drag * velocity;// ��������
            height += velocity; //���̿� �ӵ� ����
        }
    }
}