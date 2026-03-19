using UnityEngine;
using TMPro;

public class GamemanagerEasy : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject resetButton;
    public GameObject titleButton;
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public int mineCount;
    public TMP_Text mineCountText;
    private int remainingFlags;
    private float MineProbability = 0.10f; // 地雷の出現確率（パーセンテージ）
    public TMP_Text gameOverText;
    public TMP_Text gameClearText;
    public TMP_Text levelText;
    private bool isGameOver = false;
    private int revealedCount = 0;
    private int level = 1;
    
    public float totalTimeLimit = 30f; // 制限時間（秒）- インスペクタで調整可能
    private float remainingTime;
    public TMP_Text timerText; // タイマー表示用UI
    private bool isTimeAttackStarted = false;

    private CellEasy[,] grid;

    void Start()
    {
        grid = new CellEasy[width, height];
        remainingFlags = mineCount;
        gameOverText.gameObject.SetActive(false);
        gameClearText.gameObject.SetActive(false);
        resetButton.SetActive(false);
        titleButton.SetActive(false);
        remainingTime = totalTimeLimit;
        isTimeAttackStarted = false;
        UpdateMineCountUI();
        UpdateLevelUI();
        UpdateTimerUI();
        GenerateGrid();
        CountMines();
        CountAllAdjacentMines();
        AdjustCamera();
        HideAllCells();
    }

    public void OnFlagPlaced(bool placed)
    {
        // 旗が立った場合は減少、外した場合は増加
        if (placed)
            remainingFlags--;
        else
            remainingFlags++;

        UpdateMineCountUI();
    }

    private void UpdateMineCountUI()
    {
        if (mineCountText != null)
        {
            mineCountText.text = $"Mines: {remainingFlags}";
        }
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    void Update()
    {
        if (!isGameOver && isTimeAttackStarted)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                UpdateTimerUI();
                TimeUp();
            }
        }
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject obj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                CellEasy cell = obj.GetComponent<CellEasy>();

                cell.SetCoordinates(x, y);
                cell.hasMine = Random.value < MineProbability; // 10%の確率で地雷
                grid[x, y] = cell;
            }
        }
    }

    void CountMines()
    {
        int count = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].hasMine)
                    count++;
            }
        }
        mineCount = count;
        remainingFlags = count;
        UpdateMineCountUI();
    }

    void CountAllAdjacentMines()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].adjacentMines = CountAdjacentMines(x, y);
            }
        }
    }

    int CountAdjacentMines(int x, int y)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (grid[nx, ny].hasMine)
                        count++;
                }
            }
        }
        return count;
    }

    void HideAllCells()
    {
        foreach (var cell in grid)
        {
            cell.HideText();
        }
    }

    private void TimeUp()
    {
        if (isGameOver) return;
        isGameOver = true;

        // すべてのセルを開く
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].Reveal(false);
            }
        }

        // 結果表示
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = $"Time Up!\nReached Level {level}";
        }

        if (resetButton != null)
        {
            resetButton.SetActive(true);
        }

        if (titleButton != null)
        {
            titleButton.SetActive(true);
        }
    }

    public void OpenCell(int x, int y)
    {
        if (isGameOver) return; // ゲームオーバーなら何もしない

        // 最初のセルを開いたときにタイマー開始
        if (!isTimeAttackStarted)
        {
            isTimeAttackStarted = true;
        }

        if (x < 0 || x >= width || y < 0 || y >= height) return;

        CellEasy cell = grid[x, y];
        if (cell == null) return;

        if (cell.IsRevealed()) return;

        // 既に開かれていなければ開く
        if (!cell.IsRevealed())
        {
            //プレビューのクリア(一旦無効化)
            //ClearAllPreviews();

            cell.MarkRevealed();
            cell.Reveal(true);
            revealedCount++;

            // 開いたセルの上下左右を半透明プレビューで見せる
            PreviewOrthogonalNeighbors(x, y);
        }
        if (cell.hasMine)
        {
            GameOver();
            return;
        }

        if (cell.adjacentMines == 0)
        {
            OpenNeighbors(x, y);
        }

        if (revealedCount == width * height - mineCount)
        {
            GameClear();
        }
    }

    // すべてのセルのプレビューをオフにする
    private void ClearAllPreviews()
    {
        if (grid == null) return;
        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                var c = grid[ix, iy];
                if (c != null)
                {
                    c.SetPreview(false);
                }
            }
        }
    }

    // 指定セルの上下左右のみプレビュー表示する
    private void PreviewOrthogonalNeighbors(int x, int y)
    {
        // up
        if (y + 1 >= 0 && y + 1 < height)
        {
            grid[x, y + 1].SetPreview(true);
        }
        // down
        if (y - 1 >= 0 && y - 1 < height)
        {
            grid[x, y - 1].SetPreview(true);
        }
        // left
        if (x - 1 >= 0 && x - 1 < width)
        {
            grid[x - 1, y].SetPreview(true);
        }
        // right
        if (x + 1 >= 0 && x + 1 < width)
        {
            grid[x + 1, y].SetPreview(true);
        }
    }

    public void GameOver()
    {
        if (isGameOver) return; // 既にゲームオーバーなら何もしない
        isGameOver = true;

        // すべてのセルを開く
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].Reveal(false);
            }
        }

        // ゲームオーバーテキストを表示
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "Game Over!!";
        }

        if (resetButton != null)
        {
            resetButton.SetActive(true);
        }

        if (titleButton != null)
        {
            titleButton.SetActive(true);
        }

    }

    public void GameClear()
    {
        if (isGameOver) return; // 既にゲームオーバーなら何もしない
        isGameOver = true;

        // ゲームクリアテキストを表示
        if (gameClearText != null)
        {
            gameClearText.gameObject.SetActive(true);
            gameClearText.text = "You Win!!";
        }

        Invoke(nameof(NextLevel), 2f); // 2秒後に次のレベルへ
    }
    
    public void NextLevel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        width++;
        height++;

        revealedCount = 0;
        isGameOver = false;
        remainingTime += 15f;
        
        // タイマーは継続（リセットしない）
        UpdateTimerUI();

        level++;
        UpdateLevelUI();

        MineProbability += 0.01f; // 地雷の出現確率を1%増加

        grid = new CellEasy[width, height];
        remainingFlags = mineCount;
        gameOverText.gameObject.SetActive(false);
        gameClearText.gameObject.SetActive(false);
        UpdateMineCountUI();
        GenerateGrid();
        CountMines();
        CountAllAdjacentMines();
        AdjustCamera();
        HideAllCells();

        Debug.Log($"Next Level: {width}x{height}");
    }

    void AdjustCamera()
    {
        // グリッドの中央を計算
        float centerX = (width - 1) / 2f;
        float centerY = (height - 1) / 2f;

        // カメラ取得
        Camera cam = Camera.main;

        // カメラの位置をグリッド中央に
        cam.transform.position = new Vector3(centerX, centerY, -10f);

        // カメラのサイズを自動調整（縦を基準に）
        float aspect = (float)Screen.width / Screen.height;
        float halfHeight = height / 2f;
        float halfWidth = width / 2f / aspect;

        cam.orthographicSize = Mathf.Max(halfHeight, halfWidth);
    }

    private void OpenNeighbors(int x, int y)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    OpenCell(nx, ny);
                }
            }
        }
    }
}