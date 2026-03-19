using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;
    public bool hasMine;
    public int adjacentMines;
    public TextMeshPro text;
    private bool isRevealed = false;
    public bool isFlagged = false;

    private GameManager manager;
    SpriteRenderer spriteRenderer;

    private Color unrevealedColor = Color.gray;
    private Color revealedColor = Color.white;
    private Color flagColor = Color.red;

    void Start()
    {
        // 自分の親のGridManagerを探す（階層によって調整）
        manager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = unrevealedColor;
        text.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isRevealed) return;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPoint);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                if (Input.GetMouseButtonDown(0)) // 右クリックでフラグ
                {
                    if (!isFlagged)
                    {
                        manager.OpenCell(x, y);
                    }
                }

                if (Input.GetMouseButtonDown(1)) // 左クリックで開く
                {
                    ToggleFlag();
                }               
            }
        }        
    }

    public void ToggleFlag()
    {
        if (isRevealed) return;

        isFlagged = !isFlagged;

        if (isFlagged)
        {
            spriteRenderer.color = flagColor;
            text.gameObject.SetActive(true);
            text.text = "F";
            manager.OnFlagPlaced(true);
        }
        else
        {
            spriteRenderer.color = unrevealedColor;
            text.gameObject.SetActive(false);
            text.text = "";
            manager.OnFlagPlaced(false);
        }
    }

    public void Reveal(bool force = false)
    {
        if (isRevealed && !force) return;

        Debug.Log($"Revealed cell at ({x}, {y}), Mine: {hasMine}, Adjacent: {adjacentMines}");

        spriteRenderer.color = revealedColor;

        text.gameObject.SetActive(true);

        if (hasMine)
        {
            text.text = "*";
        }
        else
        {
            text.text = adjacentMines > 0 ? adjacentMines.ToString() : "";
        }
    }

    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void HideText()
    {
        text.text = "";
        text.gameObject.SetActive(false);
        isRevealed = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = unrevealedColor;
        }
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }

    public void MarkRevealed()
    {
        isRevealed = true;
    }
}