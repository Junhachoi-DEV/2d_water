using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float j_f;
    public float speed;
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Horizontal"))
        {
            rigid.AddForce(new Vector2(x * Time.deltaTime * speed, 0), ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = new Vector2(0, j_f);
        }
    }
}
