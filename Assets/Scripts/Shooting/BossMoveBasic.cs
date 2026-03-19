using UnityEngine;

public class BossMoveBasic : EnemyMovement
{
    public float upperY = 3f;
    public float lowerY = -3f;
    bool goingUp = true;

    protected override void Move()
    {
        float y = transform.position.y;

        if (goingUp)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            if (y >= upperY) goingUp = false;
        }
        else
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            if (y <= lowerY) goingUp = true;
        }
    }
}