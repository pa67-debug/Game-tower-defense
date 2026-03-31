using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public static BaseHealth instance;

    public int maxHP = 100;
    int currentHP;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        Debug.Log("Base HP: " + currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("GAME OVER");
        }
    }
}