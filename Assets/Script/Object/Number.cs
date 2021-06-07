using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    private Image bg;
    private Text number_text;

    private MyGrid inGrid; // 這個數字所在的格子

    public NumberStatus status;

    private float SpawnScaleTime = 1;
    private bool IsPlayingSpawnAnim = false;

    private float MergeScaleTime = 1;
    private float MergeScaleTimeBack = 1;
    private bool IsPlayingMergeAnim = false;

    private float movePosTime = 1;
    private bool IsMoving = false;
    private bool IsDestroyOnMoveEnd = false;
    private Vector3 StartMove, EndMovePos;

    public Color[] bg_colors;
    public List<int> number_index;

    public AudioClip MergeClip;

    private void Awake()
    {
        bg = transform.GetComponent<Image>();
        number_text = transform.Find("Text").GetComponent<Text>();
    }

    // 初始化的
    public void Init(MyGrid myGrid)
    {
        myGrid.SetNumber(this);
        // 設置所在的格子
        this.SetGrid(myGrid);
        //給他一個初始化的數字
        this.SetNumber(2);
        status = NumberStatus.Normal;

        //transform.localScale = Vector3.zero;
        PlaySpawnAnim(); //播放動畫

    }

    // 設置格子
    public void SetGrid(MyGrid myGrid)
    {
        this.inGrid = myGrid;
    }

    // 獲取格子
    public MyGrid GetGrid()
    {
        return this.inGrid;
    }

    // 設置數字
    public void SetNumber(int number)
    {
        this.number_text.text = number.ToString();

        this.bg.color = this.bg_colors[number_index.IndexOf(number)];
    }
    // 獲取數字
    public int GetNumber()
    {
        return int.Parse(number_text.text);
    }

    // 把這個數字移動到某一個格子的下面
    public void MoveToGrid(MyGrid myGrid)
    {
        transform.SetParent(myGrid.transform);
        // transform.localPosition = Vector3.zero;
        StartMove = transform.localPosition;
        //EndMovePos = myGrid.transform.position;

        movePosTime = 0;
        IsMoving = true;

        this.GetGrid().SetNumber(null);

        // 設置格子
        myGrid.SetNumber(this);
        this.SetGrid(myGrid);

    }

    // 在移動結束後銷毀
    public void DestroyOnMoveEnd(MyGrid myGrid)
    {
        transform.SetParent(myGrid.transform);
        StartMove = transform.localPosition;

        movePosTime = 0;
        IsMoving = true;
        IsDestroyOnMoveEnd = true;
    }

    // 合併
    public void Merge()
    {
        GamePanel gamePanel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
         // 要增加的分數
        gamePanel.AddScore(this.GetNumber());

        int number = this.GetNumber() * 2;
        this.SetNumber(number);
        if (number == 2048)
        {
            // 遊戲勝利
            gamePanel.GameWin();
        }
        status = NumberStatus.NotMerge;
        // 播放合併動畫
        PlayMergeAnim();
        // 播放音效
        AudioManager._instance.PlaySound(MergeClip);

    }

    //判斷能不能合併
    public bool IsMerge(Number number)
    {
        if (this.GetNumber() == number.GetNumber() && number.status == NumberStatus.Normal)
        {
            return true;
        }
        return false;
    }

    //播放創建動畫
    public void PlaySpawnAnim()
    {
        SpawnScaleTime = 0;
        IsPlayingSpawnAnim = true;
    }

    public void PlayMergeAnim()
    {
        MergeScaleTime = 0;
        MergeScaleTimeBack = 0;
        IsPlayingMergeAnim = true;
    }

    private void Update()
    {
        // 創建動畫
        if (IsPlayingSpawnAnim)
        {
            if (SpawnScaleTime <= 1)
            {
                SpawnScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, SpawnScaleTime);
            }
            else
            {
                IsPlayingSpawnAnim = false;
            }
        }



        // 合併動畫
        if (IsPlayingMergeAnim)
        {
            if (MergeScaleTime <= 1 && MergeScaleTimeBack == 0) // 變大的過程
            {
                MergeScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, MergeScaleTime);
            }
            if (MergeScaleTime >= 1 && MergeScaleTimeBack <= 1)
            {
                MergeScaleTimeBack += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, MergeScaleTimeBack);
            }
            if (MergeScaleTime >= 1 && MergeScaleTimeBack >= 1)
            {
                IsPlayingMergeAnim = false;
            }
        }
        // 移動動畫
        if (IsMoving)
        {
            movePosTime += Time.deltaTime * 5;
            transform.localPosition = Vector3.Lerp(StartMove, Vector3.zero, movePosTime);
            if (movePosTime >= 1)
            {
                IsMoving = false;
                if (IsDestroyOnMoveEnd)
                {
                    GameObject.Destroy(gameObject);
                }
            }

        }


    }


}





