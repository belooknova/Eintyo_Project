
using Dungeon;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon{

/// <summary>
/// passCoordinatesを取り出した後の処理を入れる。
/// </summary>
/// <param name="passCoordinates"></param>
/// <returns></returns>
delegate bool CheckPassCoordinateHandler(DgCell passCoordinates);
class DgMap{

    //マップを格納する配列変数
    private DgTileBase[,] map;

    //通路の端である座標。部屋を作成する候補座標となる。
    private List<DgCell> passSideCoordinates;

    //PassSideCoordinatesが使われたかどうか
    private bool isUsedPassSideCoordinates = false;

    //passSideCoordinatesから一度でも使われた座標が移される。
    private List<DgCell> usedPassSideCoordinates;

    //今使われている座標が収められているインデックス値
    private int nowUsingCoordinateIndexValue;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sizeX">マップサイズ</param>
    /// <param name="sizeY">マップサイズ</param>
    public DgMap(int sizeX, int sizeY){
        map = new DgTileBase[sizeY, sizeX];
        passSideCoordinates = new List<DgCell>();
        usedPassSideCoordinates = new List<DgCell>();
    }

    /// <summary>
    /// 指定座標にTileを登録する
    /// </summary>
    /// <param name="coordinates"></param>
    /// <param name="tile"></param>
    public void RegisterTile(DgCell coordinates, DgTileBase tile){
        map[coordinates.Y, coordinates.X] = tile;
    }

    /// <summary>
    /// 指定座標にTileを登録する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="tile"></param>
    public void RegisterTile(int x, int y, DgTileBase tile){
        map[y, x] = tile;
    }

    /// <summary>
    /// 指定座標のTileを除去する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void RemoveTile(int x, int y){
        map[y, x] = null;
    }

    /// <summary>
    /// 指定座標のTileを返す
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public DgTileBase GetTileInAssignedCoordinates(int x, int y){
        // ありえない　if(x < 0 || SizeX < x || y < 0 || SizeY < y){ return null; }
        return map[y, x];
    }

    /// <summary>
    /// passCoordinates取り出し、deligateのやつ実行、deligateのやつちゃんと実行出来たら通路を使用済みに変更
    /// </summary>
    /// <param name="checkPassCoordinateHandler">座標取得後に行う処理。返り値:bool 引数:DgCell</param>
    public DgCell FetchPassCoordinate(CheckPassCoordinateHandler checkPassCoordinateHandler){
        if(Random.Range(0,2) == 0 || usedPassSideCoordinates.Count == 0){
            nowUsingCoordinateIndexValue = Random.Range(0, passSideCoordinates.Count);
            if(checkPassCoordinateHandler(passSideCoordinates[nowUsingCoordinateIndexValue])){
                //使用済み通路リストに移す
                usedPassSideCoordinates.Add(passSideCoordinates[nowUsingCoordinateIndexValue]);
                passSideCoordinates.RemoveAt(nowUsingCoordinateIndexValue);
                return usedPassSideCoordinates[usedPassSideCoordinates.Count - 1];
            } else {
                return null;
            }
        } else {
            //使用済み通路リストから通路座標を取得する
            nowUsingCoordinateIndexValue = Random.Range(0, usedPassSideCoordinates.Count);
            if(checkPassCoordinateHandler(usedPassSideCoordinates[nowUsingCoordinateIndexValue])){
                return usedPassSideCoordinates[nowUsingCoordinateIndexValue];
            } else {
                return null;
            }
        }
    }

    /// <summary>
    /// passSideCoordinateに新しい座標をaddする。
    /// </summary>
    /// <param name="newPassSideCoordinate">addしたい座標</param>
    public void AddPassSideCoordinate(DgCell newPassSideCoordinate){
        passSideCoordinates.Add(newPassSideCoordinate);
    }

    /// <summary>
    /// 部屋設置候補座標を返す。
    /// </summary>
    /// <value></value>
    public int CandidateRoomCoordinatesCount{
        get{
            return (passSideCoordinates.Count + usedPassSideCoordinates.Count);
        }
    }

    /// <summary>
    /// マップのサイズXを返す
    /// </summary>
    /// <returns></returns>
    public int SizeX{
        get{
            return map.GetLength(1);
        }
    }

    /// <summary>
    /// マップのサイズYを返す
    /// </summary>
    /// <returns></returns>
    public int SizeY{
        get{
            return map.GetLength(0);
        }
    }

}


}