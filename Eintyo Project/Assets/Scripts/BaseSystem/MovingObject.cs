using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    //moveTimeを計算するのを単純化するための変数
    private float inverseMoveTime;

    //virtual : 継承されるメソッドに付ける修飾子
    protected virtual void Start()
    {
        //BoxCollider2DとRigidbody2Dを何度もGetComponentしなくて済むよう
        //Startメソッドにてキャッシュしておく
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        //デフォルトだと 1f ÷ 0.1f = 10.0f
        inverseMoveTime = 1f / moveTime;
    }

    //移動可能かを判断するメソッド　可能な場合はSmoothMovementへ
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //現在地を取得
        Vector2 start = transform.position;
        //目的地を取得
        Vector2 end = start + new Vector2(xDir, yDir);
        //自身のColliderを無効にし、Linecastで自分自身を判定しないようにする
        boxCollider.enabled = false;
        //現在地と目的地との間にblockingLayerのついたオブジェクトが無いか判定
        hit = Physics2D.Linecast(start, end, blockingLayer);
 
        //Colliderを有効に戻す
        boxCollider.enabled = true;
        //何も無ければSmoothMovementへ遷移し移動処理
        if (hit.transform == null)
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
        while (sqrRemainingDistance > float.Epsilon)
        {
            //現在地と移動先の間を1秒間にinverseMoveTime分だけ移動する場合の、
            //1フレーム分の移動距離を算出する
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            //算出した移動距離分、移動する
            rb2D.MovePosition(newPosition);
            //現在地が目的地寄りになった結果、sqrRemainDistanceが小さくなる
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            //1フレーム待ってから、while文の先頭へ戻る
            yield return null;
        }
    }

    //移動を試みるメソッド
    //virtual : 継承されるメソッドに付ける修飾子
    //<T>：ジェネリック機能　型を決めておかず、後から指定する
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        //ジェネリック用の型引数をComponent型で限定
        where T : Component
    {
        RaycastHit2D hit;
        //Moveメソッド実行 戻り値がtrueなら移動成功、falseなら移動失敗
        bool canMove = Move(xDir, yDir, out hit);
        //Moveメソッドで確認した障害物が何も無ければメソッド終了
        if (hit.transform == null)
        {
            return;
        }
        //障害物があった場合、障害物を型引数の型で取得
        //型が<T>で指定したものと違う場合、取得できない
        T hitComponent = hit.transform.GetComponent<T>();
        //障害物がある場合OnCantMoveを呼び出す
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    //abstract: メソッドの中身はこちらでは書かず、サブクラスにて書く
    //<T>：AttemptMoveと同じくジェネリック機能
    //障害物があり移動ができなかった場合に呼び出される
    protected abstract void OnCantMove<T>(T component) where T : Component;
}
