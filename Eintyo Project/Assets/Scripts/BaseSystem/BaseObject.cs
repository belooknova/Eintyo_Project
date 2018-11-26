using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObject : MonoBehaviour {

    //コリダーとリジッドボディ取得
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    public float moveTime = 5.0f;

    public StetusData stetus;
    public bool isMoving = false;
    public float size;

    public LayerMask blockingLayer;

    
    protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        size = GameManager.instance.spSize;

        //StetudDataを保証する
        stetus = GetComponent<StetusData>();
        if (stetus == null)
        {
            this.gameObject.AddComponent<StetusData>();
            stetus = GetComponent<StetusData>();
        }


        
    }

    //移動可能かを判断するメソッド　可能な場合はSmoothMovementへ
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //タイルの幅取得
        

    //現在地を取得
        Vector2 start = transform.position;
        //目的地を取得
        Vector2 end = start + new Vector2(size * xDir, size * yDir);
        //自身のColliderを無効にし、Linecastで自分自身を判定しないようにする
        boxCollider.enabled = false;
        //現在地と目的地との間にblockingLayerのついたオブジェクトが無いか判定
        hit = Physics2D.Linecast(start, end, blockingLayer);
 
        //Colliderを有効に戻す
        boxCollider.enabled = true;
        //何も無ければSmoothMovementへ遷移し移動処理
        if (hit.transform == null && !isMoving)
        {
            StartCoroutine(SmoothMovement(end));
            //移動が成功したことを伝える
            return true;
        }
        //移動に失敗したことを伝える
        return false;
    }
  

    //現在地から目的地(引数end)へ移動するためのメソッド
    protected IEnumerator SmoothMovement(Vector3 end)
    {

        //現在地から目的地を引き、2点間の距離を求める(Vector3型)
        //sqrMagnitudeはベクトルを2乗したあと2点間の距離に変換する(float型)
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        //2点間の距離が0になった時、ループを抜ける
        //Epsilon : ほとんど0に近い数値を表す
        //Vector3 newPosition = (transform.position - end).normalized;

        isMoving = true;
        Vector3 moveDirection = (transform.position - end).normalized;

        while (true)
        {

            transform.position -= moveDirection;// * Time.deltaTime;
            Debug.Log((transform.position - end).normalized);
            yield return null;
        }


            /*
            while (sqrRemainingDistance > float.Epsilon)
            {
                //現在地と移動先の間を1秒間にinverseMoveTime分だけ移動する場合の、
                //1フレーム分の移動距離を算出する
                //Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, size * Time.deltaTime);

                newPosition += (transform.position - end).normalized * Time.deltaTime;
                //Debug.Log("(NEW)"+ size * (transform.position - end).normalized);
                //Debug.Log(newPosition);
                //newPosition = new Vector3(0, 0, 0);
                Debug.Log("ループ中");

                //算出した移動距離分、移動する
                rb2D.MovePosition(newPosition);
                //this.transform.position = newPosition;
                //現在地が目的地寄りになった結果、sqrRemainDistanceが小さくなる
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
                //1フレーム待ってから、while文の先頭へ戻る
                yield return null;
            }
            */
            isMoving = false;
    }




    ///移動を試みるメソッド
      //virtual : 継承されるメソッドに付ける修飾子
      //<T>：ジェネリック機能　型を決めておかず、後から指定する
    protected virtual void AttemptMove(int xDir, int yDir)
    {
        RaycastHit2D hit;
        //Moveメソッド実行 戻り値がtrueなら移動成功、falseなら移動失敗
        bool canMove = Move(xDir, yDir, out hit);
        //Debug.Log("受け付けました");
        //Moveメソッドで確認した障害物が何も無ければメソッド終了
        if (hit.transform == null)
        {
            return;
        }
        
    }
    
}
