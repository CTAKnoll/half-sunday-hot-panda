using UnityEngine;

public class Enemy : MonoBehaviour, Damageable
{
    public int MaxHealth;
    private int Health;
    
    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
