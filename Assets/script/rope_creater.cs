using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rope_creater : MonoBehaviour
{
    public GameObject rope_prefab;
    public int rope_cnt;
    public Rigidbody2D point_rig;
    FixedJoint2D ex_joint; //���� ����Ʈ

    void Start()
    {
        for (int i = 0; i < rope_cnt; i++)
        {
            FixedJoint2D current_joint = Instantiate(rope_prefab, transform).GetComponent<FixedJoint2D>();
            current_joint.transform.localPosition = new Vector3(0, (i+1) * -0.25f, 0);
            if (i == 0)
            {
                current_joint.connectedBody = point_rig;
            }
            else
            {
                current_joint.connectedBody = ex_joint.GetComponent<Rigidbody2D>();
            }

            ex_joint = current_joint;

            //������ ��
            if(i== rope_cnt - 1)
            {
                current_joint.GetComponent<Rigidbody2D>().mass =10; // ���̰�
                current_joint.GetComponent<SpriteRenderer>().enabled = false; //�Ⱥ��̰�
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
