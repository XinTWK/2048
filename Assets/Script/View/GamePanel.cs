using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GamePanel : MonoBehaviour
{
    #region UI控制

    public Text text_score;       //分數
    public Text text_best_score;  //最高分
    public Button btn_last;       //上一步
    public Button btn_restart;    //重新開始
    public Button btn_exit;       //退出

    public WinPanel winPanel;
    public LosePanel losePanel;

    #endregion

    #region  屬性 變數

    public Transform gridParent;  //格子的父物體

    public Dictionary<int, int> grid_config = new Dictionary<int, int>() { { 4, 100 }, { 5, 85 }, { 6, 77 } };

    private int row; //列
    private int col; //行
    public MyGrid[][] grids = null; //所有的格子
    public List<MyGrid> CanCreateNumberGrid = new List<MyGrid>(); //可以創建數字的格子

    public GameObject gridPrefab;
    public GameObject numberPrefab;

    private Vector3 pointerDownPos, pointerUpPos;

    private bool IsNeedCreateNumber = false;

    public int CurrentScore;
    public StepModel LastStepModel;

    public AudioClip bgClip;

    #endregion

    #region 遊戲邏輯

    //初始化格子
    public void InitGrid()
    {

        //獲取格子數量
        int gridNum = PlayerPrefs.GetInt(Const.GameModel, 4);
        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridNum;
        gridLayoutGroup.cellSize = new Vector2(grid_config[gridNum], grid_config[gridNum]);

        //初始化格子
        grids = new MyGrid[gridNum][];

        //創建格子
        row = gridNum;
        col = gridNum;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //創建ij格子
                if (grids[i] == null)
                {
                    grids[i] = new MyGrid[gridNum];
                }
                grids[i][j] = CreateGrid();
            }
        }


    }

    //創建格子
    public MyGrid CreateGrid()
    {
        //實例化格子預製體
        GameObject gameObject = GameObject.Instantiate(gridPrefab, gridParent);

        return gameObject.GetComponent<MyGrid>();
    }

    //創建數字
    public void CreateNumber()
    {
        //找到這個數字所在的格子
        CanCreateNumberGrid.Clear();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].IsHaveNumber())  //判斷這個格子有無數字
                {
                    //如果沒有數字
                    CanCreateNumberGrid.Add(grids[i][j]);
                }
            }
        }
        if (CanCreateNumberGrid.Count == 0)
        {
            return;
        }

        //隨機一個格子
        int index = Random.Range(0, CanCreateNumberGrid.Count);

        //創建數字 把數字放進去
        GameObject gameObj = GameObject.Instantiate(numberPrefab, CanCreateNumberGrid[index].transform);
        gameObj.GetComponent<Number>().Init(CanCreateNumberGrid[index]); // 進行初始化
    }

    public void CreateNumber(MyGrid mygrid, int number)
    {
        GameObject gameObj = GameObject.Instantiate(numberPrefab, mygrid.transform);
        gameObj.GetComponent<Number>().Init(mygrid); // 進行初始化
        gameObj.GetComponent<Number>().SetNumber(number);
    }

    public MoveType CaculateMoveType()
    {
        if (Mathf.Abs(pointerUpPos.x - pointerDownPos.x) > Mathf.Abs(pointerUpPos.y - pointerDownPos.y))
        {
            // 左右移動
            if (pointerUpPos.x - pointerDownPos.x > 0)
            {
                // 向右移動
                // Debug.Log("向右移動");
                return MoveType.RIGHT;
            }
            else
            {
                // 向左移動
                // Debug.Log("向左移動");
                return MoveType.LEFT;
            }
        }
        else
        {
            // 上下移動
            if (pointerUpPos.y - pointerDownPos.y > 0)
            {
                // 向上移動
                // Debug.Log("向上移動");
                return MoveType.TOP;
            }
            else
            {
                // 向下移動
                // Debug.Log("向下移動");
                return MoveType.DOWN;
            }
        }
    }

    public void MoveNumber(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.TOP:

                for (int j = 0; j < col; j++)
                {
                    for (int i = 1; i < row; i++)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {

                            Number number = grids[i][j].GetNumber();

                            // Debug.Log("座標: "+ i+" , "+j);
                            for (int m = i - 1; m >= 0; m--)
                            {

                                Number targetNumber = null;
                                if (grids[m][j].IsHaveNumber())
                                {
                                    targetNumber = grids[m][j].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[m][j]);

                                if (targetNumber != null) // 如果有數字跳出循環
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.DOWN:

                for (int j = 0; j < col; j++)
                {
                    for (int i = row - 2; i >= 0; i--)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {

                            Number number = grids[i][j].GetNumber();

                            // Debug.Log("座標: "+ i+" , "+j);
                            for (int m = i + 1; m < row; m++)
                            {
                                Number targetNumber = null;
                                if (grids[m][j].IsHaveNumber())
                                {
                                    targetNumber = grids[m][j].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[m][j]);

                                if (targetNumber != null) // 如果有數字跳出循環
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.LEFT:
                for (int i = 0; i < row; i++)
                {
                    for (int j = 1; j < col; j++)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {

                            Number number = grids[i][j].GetNumber();

                            // Debug.Log("座標: "+ i+" , "+j);
                            for (int m = j - 1; m >= 0; m--)
                            {
                                Number targetNumber = null;
                                if (grids[i][m].IsHaveNumber())
                                {
                                    targetNumber = grids[i][m].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[i][m]);

                                if (targetNumber != null) // 如果有數字跳出循環
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                break;
            case MoveType.RIGHT:
                for (int i = 0; i < row; i++)
                {
                    for (int j = col - 2; j >= 0; j--)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {

                            Number number = grids[i][j].GetNumber();

                            // Debug.Log("座標: "+ i+" , "+j);
                            for (int m = j + 1; m < col; m++)
                            {
                                Number targetNumber = null;
                                if (grids[i][m].IsHaveNumber())
                                {
                                    targetNumber = grids[i][m].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[i][m]);

                                if (targetNumber != null) // 如果有數字跳出循環
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
        }
    }

    public void HandleNumber(Number current, Number target, MyGrid targetGrid)
    {
        if (target != null)
        {
            // 判斷能不能合併
            if (current.IsMerge(target))
            {
                target.Merge();

                // 銷毀當前數字
                current.GetGrid().SetNumber(null);
                // GameObject.Destroy(current.gameObject);
                current.DestroyOnMoveEnd(target.GetGrid());
                IsNeedCreateNumber = true;
            }
        }
        else
        {
            // 沒有數字
            current.MoveToGrid(targetGrid);
            IsNeedCreateNumber = true;
        }
    }

    public void ResetNumberStatus()
    {
        // 遍歷所有的數字
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grids[i][j].IsHaveNumber())
                {
                    grids[i][j].GetNumber().status = NumberStatus.Normal;
                }
            }
        }
    }

    // 判斷遊戲是否失敗
    public bool IsGameLose()
    {
        // 判斷格子是否滿了
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].IsHaveNumber())
                {
                    return false;
                }
            }
        }
        // 判斷有沒有數字能夠合併
        for (int i = 0; i < row; i += 2)
        {
            for (int j = 0; j < col; j++)
            {
                MyGrid up = IsHaveGrid(i - 1, j) ? grids[i - 1][j] : null;
                MyGrid down = IsHaveGrid(i + 1, j) ? grids[i + 1][j] : null;
                MyGrid left = IsHaveGrid(i, j - 1) ? grids[i][j - 1] : null;
                MyGrid right = IsHaveGrid(i, j + 1) ? grids[i][j + 1] : null;
                //grids[i][j]
                if (up != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(up.GetNumber()))
                    {
                        return false;
                    }

                }
                if (down != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(down.GetNumber()))
                    {
                        return false;
                    }

                }
                if (left != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(left.GetNumber()))
                    {
                        return false;
                    }

                }
                if (right != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(right.GetNumber()))
                    {
                        return false;
                    }

                }
            }
        }

        return true; // 遊戲失敗
    }

    public bool IsHaveGrid(int i, int j)
    {
        if (i >= 0 && i < row && j >= 0 && j < col)
        {
            return true;
        }
        return false;
    }


    #endregion

    #region  腳本週期

    private void Awake()

    {
        // 初始化界面訊息
        InitPanelMessage();

        // 初始化格子
        InitGrid();
        // 創建第一個數字
        CreateNumber();
    }

    #endregion

    #region 事件監聽

    public void OnPointerDown()
    {
        //Debug.Log("按下:" + Input.mousePosition);
        pointerDownPos = Input.mousePosition;
    }

    public void OnPointerUp()
    {
        //Debug.Log("彈起:" + Input.mousePosition);
        pointerUpPos = Input.mousePosition;

        if (Vector3.Distance(pointerUpPos, pointerDownPos) < 50)
        {
            Debug.Log("無效操作!");
            return;
        }
        //對當前的這一步進行保存
        LastStepModel.UpdateData(this.CurrentScore, PlayerPrefs.GetInt(Const.BestScore, 0), grids);
        btn_last.interactable = true;

        MoveType moveType = CaculateMoveType();
        Debug.Log("移動類型: " + moveType);
        MoveNumber(moveType);

        // 產生數字
        if (IsNeedCreateNumber)
        {
            CreateNumber();
        }


        // 把所有數字的狀態 恢復成正常狀態
        ResetNumberStatus();
        IsNeedCreateNumber = false;
        // 判斷遊戲是否結束
        if (IsGameLose())  // 說明遊戲結束
        {
            GameLose();
        }
    }

    //上一步
    public void OnLastClick()
    {
        BackToLastStep();
        btn_last.interactable = false;

    }

    public void OnRestartClick()
    {
        ReStartGame();
    }

    public void OnExitClick()
    {
        ExitGame();
    }

    #endregion

    #region 介面更新

    public void InitPanelMessage()
    {
        this.text_best_score.text = PlayerPrefs.GetInt(Const.BestScore, 0).ToString();
        LastStepModel = new StepModel();
        btn_last.interactable = false;
        // 撥放音樂
        AudioManager._instance.PlayMusic(bgClip);

    }

    public void AddScore(int Score)
    {
        CurrentScore += Score;
        UpdateScore(CurrentScore);

        // 判斷當前分數是否為最高分數
        if (CurrentScore > PlayerPrefs.GetInt(Const.BestScore, 0))
        {
            PlayerPrefs.SetInt(Const.BestScore, CurrentScore);
            UpdateBestScore(CurrentScore);
        }
    }

    public void UpdateScore(int Score)
    {
        this.text_score.text = Score.ToString();
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        UpdateScore(CurrentScore);
    }


    public void UpdateBestScore(int BestScore)
    {
        this.text_best_score.text = BestScore.ToString();
    }

    #endregion

    #region  遊戲流程

    public void BackToLastStep()
    {
        // 分數
        CurrentScore = LastStepModel.score;
        UpdateScore(LastStepModel.score);

        PlayerPrefs.SetInt(Const.BestScore, LastStepModel.bestScore);
        UpdateBestScore(LastStepModel.bestScore);

        // 數字
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (LastStepModel.numbers[i][j] == 0 )
                {
                    if (grids[i][j].IsHaveNumber())
                    {
                        GameObject.Destroy(grids[i][j].GetNumber().gameObject);
                        grids[i][j].SetNumber(null);
                    }
                }
                else if (LastStepModel.numbers[i][j] != 0)
                {
                    if (grids[i][j].IsHaveNumber())
                    {
                        // 修改數字
                        grids[i][j].GetNumber().SetNumber(LastStepModel.numbers[i][j]);
                    }
                    else
                    {
                        // 創建數字
                        CreateNumber(grids[i][j], LastStepModel.numbers[i][j]);
                    }
                }
            }
        }

    }

    // 離開遊戲
    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    // 重新開始
    public void ReStartGame()
    {
        // 數據清空
        btn_last.interactable = false;

        // 清空分數
        ResetScore();

        // 清空數字
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grids[i][j].IsHaveNumber())
                {
                    GameObject.Destroy(grids[i][j].GetNumber().gameObject);
                    grids[i][j].SetNumber(null);
                }
            }
        }
        // 創建一個數字
        CreateNumber();
    }

    public void GameWin()
    {
        Debug.Log("遊戲勝利");
        winPanel.Show();
    }

    public void GameLose()
    {
        Debug.Log("遊戲失敗");
        losePanel.Show();
    }


    #endregion





}
