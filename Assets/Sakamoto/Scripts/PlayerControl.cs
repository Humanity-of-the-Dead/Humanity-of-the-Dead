using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    [Header("移動スピード")]
    [SerializeField] float fSpeed;
    [Header("ジャンプ力")]
    [SerializeField] float fJmpPower;
    bool bJump = false;


    //カメラ関連
    [SerializeField] Camera goCamera;
    //高さ
    float fCameraHeight;
    //幅
    float fCameraWidth;

    //仮ターゲット
    [SerializeField] GameObject[] goObj;

    //プレイヤーパラメーターの取得
    [SerializeField] PlayerParameter playerParameter;

    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();

        // カメラの高さ（orthographicSize）はカメラの中央から上下の距離を表す
        fCameraHeight = 2f * goCamera.orthographicSize;

        // カメラの幅はアスペクト比に基づいて計算する
        fCameraWidth = fCameraHeight * goCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        //現在のポジションを取得
        Vector2 vPosition = this.transform.position;

        //カメラとの距離の絶対値が一定以下ならプレイヤーが動く　画面外に出ないための処置
        //移動
        Vector3 vPosFromCame = this.transform.position - goCamera.transform.position; //カメラ基準のプレイヤーの位置
        //左移動
        if (Input.GetKey(KeyCode.A))
        {
            if (vPosFromCame.x > -fCameraWidth / 2)
            {
                vPosition.x -= Time.deltaTime * fSpeed;
            }
        }
        //右移動
        if (Input.GetKey(KeyCode.D))
        {
            if (fCameraWidth / 2 > vPosFromCame.x)
            {
                vPosition.x += Time.deltaTime * fSpeed;
            }
        }

        //ジャンプ

        if (Input.GetKey(KeyCode.W) && bJump == false)
        {
            this.rbody2D.AddForce(this.transform.up * fJmpPower);
            bJump = true;
        }

        //体が回転しないようにする
        //自分のtransformを取得
        Quaternion quaternion = GetComponent<Transform>().rotation;
        quaternion.z = 0.0f;
        transform.rotation = quaternion; 


        //移動後のポジションを代入
        this.transform.position = vPosition;

        //攻撃関連
        //上半身攻撃
        if(Input.GetKeyDown(KeyCode.I)) {
            for(int i = 0; i < goObj.Length; i++) {
                //仮引数
                UpperBodyAttack(i,goObj[i].gameObject.transform.position, 5.0f);
            }
        }
        //下半身攻撃
        if(Input.GetKeyDown(KeyCode.K)) {
            for (int i = 0; i < goObj.Length; i++)
            {
                //仮引数
                LowerBodyAttack(i,goObj[i].gameObject.transform.position, 8.0f);
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Car"))
        {
            bJump = false;
        }
    }

    //上半身攻撃
    public void UpperBodyAttack(int EnemyNum,Vector3 vTargetPos, float fReach)
    {
        float fAttackReach = Vector3.Distance(vTargetPos,this.transform.position);
        if(fAttackReach < fReach)
        {
            goObj[EnemyNum].GetComponent<EnemyParameters>().TakeDamage(1,0);
            Debug.Log("上半身攻撃成功");
        }
        else
        {
            Debug.Log("上半身攻撃失敗");
        }
    }
    //下半身攻撃
    public void LowerBodyAttack(int EnemyNum, Vector3 vTargetPos, float fReach)
    {
        float fAttackReach = Vector3.Distance(vTargetPos,this.transform.position);
        if(fAttackReach < fReach)
        {
            goObj[EnemyNum].GetComponent<EnemyParameters>().TakeDamage(1, 1);
            Debug.Log("下半身攻撃成功");
        }
        else
        {
            Debug.Log("下半身攻撃失敗");
        }
    }
}
