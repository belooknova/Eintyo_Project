using UnityEngine;
using System.Collections;

/// <summary>
/// ダンジョン区画情報
/// </summary>
public class DgDivision
{
    /// <summary>
    /// 矩形管理
    /// </summary>
    public class DgRect
    {
        public int Left   = 0; // 左
        public int Top    = 0; // 上
        public int Right  = 0; // 右
        public int Bottom = 0; // 下

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DgRect()
        {
            // 特に何もしない
        }
        /// <summary>
        /// 値をまとめて設定する
        /// </summary>
        /// <param name="left">左</param>
        /// <param name="top">上</param>
        /// <param name="right">右</param>
        /// <param name="bottom">左</param>
        public void Set(int left, int top, int right, int bottom)
        {
            Left   = left;
            Top    = top;
            Right  = right;
            Bottom = bottom;
        }
        /// <summary>
        /// 幅
        /// </summary>
        public int Width
        {
            get { return Right - Left; }
        }
        /// <summary>
        /// 高さ
        /// </summary>
        public int Height
        {
            get { return Bottom - Top; }
        }
        /// <summary>
        /// 面積 (幅 x 高さ)
        /// </summary>
        public int Measure
        {
            get { return Width * Height; }
        }

        /// <summary>
        /// 矩形情報をコピーする
        /// </summary>
        /// <param name="rect">コピー元の矩形情報</param>
        public void Copy(DgRect rect)
        {
            Left   = rect.Left;
            Top    = rect.Top;
            Right  = rect.Right;
            Bottom = rect.Bottom;
        }

        /// <summary>
        /// デバッグ出力
        /// </summary>
        public void Dump()
        {
            Debug.Log(string.Format("<Rect l,t,r,b = {0},{1},{2},{3}> w,h = {4},{5}",
                Left, Top, Right, Bottom, Width, Height));
        }
    }

    /// <summary>
    /// 外周の矩形情報
    /// </summary>
    public DgRect Outer;
    /// <summary>
    /// 区画内に作ったルーム情報
    /// </summary>
    public DgRect Room;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DgDivision()
    {
        Outer = new DgRect();
        Room = new DgRect();
    }
    /// <summary>
    /// デバッグ出力
    /// </summary>
    public void Dump()
    {
        Outer.Dump();
        Room.Dump();
    }
}
