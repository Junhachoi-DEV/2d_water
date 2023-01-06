using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자료 출처 https://www.youtube.com/watch?v=Ksw1JI7r7WQ&t=18s
public class rope2 : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int segment_count =15;
    public int constraint_loop = 15;
    public float segment_length =0.1f;
    public float rope_width=0.1f;
    public Vector2 gravity = new Vector2(0f, -9.81f); //중력

    [Space(10f)]
    public Transform start_transform;


    List<segment> segments = new List<segment>();


    private void Reset()
    {
        TryGetComponent(out lineRenderer);
    }

    private void Awake()
    {
        Vector2 segment_pos = start_transform.position;
        for (int i = 0; i < segment_count; i++)
        {
            segments.Add(new segment(segment_pos));
            segment_pos.y -= segment_length;
        }
    }


    private void FixedUpdate()
    {
        update_segments();
        for (int i = 0; i < constraint_loop; i++)
        {
            apply_constraint();
            Ad_just_collision();
        }
        draw_rope();
    }


    void draw_rope()
    {
        lineRenderer.startWidth = rope_width;
        lineRenderer.endWidth = rope_width;
        Vector3[] segment_position = new Vector3[segments.Count];
        for (int i = 0; i < segments.Count; i++)
        {
            segment_position[i] = segments[i].current_pos;

        }
        lineRenderer.positionCount = segment_position.Length;
        lineRenderer.SetPositions(segment_position);
    }
    void update_segments()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].velocity = segments[i].current_pos - segments[i].previous_pos;
            segments[i].previous_pos = segments[i].current_pos;
            segments[i].current_pos += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
            segments[i].current_pos += segments[i].velocity;
        }
    }
    void apply_constraint()
    {
        segments[0].current_pos = start_transform.position;
        for (int i = 0; i < segments.Count-1; i++)
        {
            
            float distance = (segments[i].current_pos - segments[i + 1].current_pos).magnitude;//백터 구하는 함수
            float difference = segment_length - distance;
            Vector2 dir = (segments[i + 1].current_pos - segments[i].current_pos).normalized; //방향

            Vector2 movement = dir * difference;
            if(i == 0)
            {
                segments[i + 1].current_pos += movement;
            }
            else
            {
                segments[i].current_pos -= movement * 0.5f;
                segments[i + 1].current_pos += movement * 0.5f;
            }
        }
    }
    //출동 함수
    void Ad_just_collision()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            Vector2 dir = segments[i].current_pos - segments[i].previous_pos;
            //레이캐스트에서 원에서 쏘는 함수가 있다.!
            RaycastHit2D hit = Physics2D.CircleCast(segments[i].current_pos, rope_width * 0.5f, dir.normalized, 0f);

            if (hit)
            {
                segments[i].current_pos = hit.point + hit.normal * rope_width * 0.5f;
                segments[i].previous_pos = segments[i].current_pos;
            }
        }
    }
    public class segment
    {
        public Vector2 previous_pos;
        public Vector2 current_pos;
        public Vector2 velocity;

        public segment(Vector2 _position)
        {
            previous_pos = _position;
            current_pos = _position;
            velocity = Vector2.zero;
        }
    }
}
