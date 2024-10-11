using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaControl : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    [SerializeField] float fSpeed;
    [SerializeField] float fJmpPower;
    bool bJump = false;
    // Start is called before the first frame update
    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //現在のポジションを取得
        Vector2 vPosition = transform.position;


        if (Input.GetKey(KeyCode.A))
        {
            vPosition.x -= Time.deltaTime * fSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vPosition.x += Time.deltaTime * fSpeed;
        }

        //ジャンプ

        if (Input.GetKey(KeyCode.Space) && bJump == false)
        {
            this.rbody2D.AddForce(transform.up * fJmpPower);
            bJump = true;
        }

        //移動後のポジションを代入
        this.transform.position = vPosition;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            bJump = false;
        }
    }
}
