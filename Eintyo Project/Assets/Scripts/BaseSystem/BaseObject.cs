using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObject : MonoBehaviour {

    //コリダーとリジッドボディ取得
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    public float moveTime = 5.0f;

    public StatusData stetus; //ステータス
    public bool isMoving = false; //移動中かどうか
    public float size; //タイルのサイズ(自動)

    public bool isTurning = false; //ターン中かどうか調べる
 
    public LayerMask blockingLayer;

    
    protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        size = GameManager.instance.spSize;

        //StetudDataを保証する
        stetus = GetComponent<StatusData>();
        if (stetus == null)
        {
            this.gameObject.AddComponent<StatusData>();
            stetus = GetComponent<StatusData>();
        }

        //行動順リストに入れる
        GameManager.instance.ActorList.Add(this.gameObject);
        
    }

    //自分のターンが回って来た時のUpDate()
    public virtual void TurnUpDate()
    {
        
    }

    //次の状態に偏移するためのメソッド(サブクラスでオーバーライドする)
    public virtual void Next_status()
    {

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
    protected virtual IEnumerator SmoothMovement(Vector3 end)
    {
        //一応これにして、問題が起きたら修正する。
        Vector3 moveDirection = (transform.position - end); //進む方向と距離
        isMoving = true; //移動中かどうか
        int moveSplit = 8;
        int i = 0;
        //一マスを8回に分けて進む
        while (i < moveSplit)
        {

            transform.position -= moveDirection / moveSplit;// * Time.deltaTime;
            Debug.Log((transform.position - end).normalized);
            yield return null;
            i += 1;
        }
        isMoving = false;

        Next_status();

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
