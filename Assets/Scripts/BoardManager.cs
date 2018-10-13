using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

/// <summary>
/// ボード管理クラス
/// ボード自動生成に使用
/// </summary>
public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count {
        public int minmum;
        public int maximum;

        public Count (int min, int max) {
            minmum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int row = 8;
    public Count wallCount = new Count (5, 9);
    public Count foodCount = new Count (1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] walltiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();


    /// <summary>
    /// gridPositionsの初期化処理
    /// </summary>
    void InitialiseList() {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++) {
            for (int y = 0; y < row -1; y++) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    /// <summary>
    /// ボードのセットアップ
    /// </summary>
    void BoardSetup() {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++) {
            for (int y = -1; y < row + 1; y++) {
                //ランダムに床のプレハブを設定
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; 
                //外壁の位置ならランダムな外壁を設定
                if (x == -1 || x == columns || y == -1 || y ==row) {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);

            }
        }

    }

    /// <summary>
    /// ランダム座標取得
    /// ・gridPositionsに格納されている座標を１件取得
    /// ・取得した座標をgridPositionsから取り除く
    /// </summary>
    /// <returns>ランダム座標</returns>
    Vector3 RandomPosition() {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    /// <summary>
    /// ランダムな位置にタイルを配置
    /// </summary>
    /// <param name="tileArray">配置されるオブジェクト（配列からランダムに選ばれる）</param>
    /// <param name="minmum">最小の配置される個数</param>
    /// <param name="maximum">最大の配置される個数</param>
    void LayoutObjectAtRandom(GameObject[] tileArray, int minmum, int maximum ) {
        int objectCount = Random.Range(minmum, maximum + 1);
        for (int i = 0; i < objectCount; i++) {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    /// <summary>
    /// シーンのセットアップ
    /// </summary>
    /// <param name="level">難易度（現時点では敵の配置数に影響する）</param>
    public void SetupScene(int level) {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(walltiles, wallCount.minmum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minmum, foodCount.maximum);

        //敵の配置
        int enemyCount = (int)Mathf.Log(level, 2f);
        Debug.Log("enemyCount:" + enemyCount);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //出口の配置
        Instantiate(exit, new Vector3(columns - 1, row - 1, 0f), Quaternion.identity);
    }
}
