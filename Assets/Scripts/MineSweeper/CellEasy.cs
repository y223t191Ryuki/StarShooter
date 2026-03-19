using UnityEngine;
using TMPro;

public class CellEasy : MonoBehaviour
{
    public int x;
    public int y;
    public bool hasMine;
    public int adjacentMines;
    public TextMeshPro text;
    private bool isRevealed = false;
    public bool isFlagged = false;

    private GamemanagerEasy manager;
    SpriteRenderer spriteRenderer;

    private Color unrevealedColor = Color.gray;
    private Color revealedColor = Color.white;
    private Color flagColor = Color.red;
    private float previewAlpha = 0.35f; // プレビュー時の透過率

    void Start()
    {
        // 自分の親のGridManagerを探す（階層によって調整）
        manager = FindObjectOfType<GamemanagerEasy>();
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

        // マーカーとしても Revealed をセット
        isRevealed = true;

        // 表示は常に不透明の revealedColor
        Color showColor = revealedColor;
        showColor.a = 1f;
        spriteRenderer.color = showColor;

        text.gameObject.SetActive(true);
        // テキストの不透明化（通常表示）
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

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
            Color c = unrevealedColor;
            c.a = 1f;
            spriteRenderer.color = c;
        }

        // テキスト色は元に戻す
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }

    public void MarkRevealed()
    {
        isRevealed = true;
    }

    // 半透明プレビュー表示/非表示
    public void SetPreview(bool on)
    {
        if (isRevealed) return; // 既に開かれているセルは変更しない
        if (isFlagged) return; // フラグがあるセルはプレビューしない

        if (on)
        {
            // 中身をうっすら見せる：revealed の色を薄く
            Color pc = revealedColor;
            pc.a = previewAlpha;
            spriteRenderer.color = pc;

            // テキストは内容が見えるように有効化して薄く表示
            if (hasMine)
            {
                text.text = "*";
            }
            else
            {
                text.text = adjacentMines > 0 ? adjacentMines.ToString() : "";
            }
            text.gameObject.SetActive(true);
            text.color = new Color(text.color.r, text.color.g, text.color.b, previewAlpha);
        }
        else
        {
            // 非表示に戻す
            text.text = "";
            text.gameObject.SetActive(false);
            Color c = unrevealedColor;
            c.a = 1f;
            spriteRenderer.color = c;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        }
    }
}