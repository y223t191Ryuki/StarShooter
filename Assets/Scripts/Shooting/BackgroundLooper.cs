using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    [Header("スクロール速度")]
    public float scrollSpeed = 2f;

    private float spriteWidth;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        // Spriteの横幅を取得
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // 左方向へ移動
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // 一定距離移動したら元の位置へ戻す
        if (transform.position.x <= startPosition.x - spriteWidth)
        {
            transform.position += Vector3.right * spriteWidth * 2f;
        }
    }
}