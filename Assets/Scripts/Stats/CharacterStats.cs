using UnityEngine;

public class CharacterStats : MonoBehaviour {
    public float maxHealth=100;
    public float currentHealth;//{ get; private set; }

    public Stat damage;
    public Stat attackRate;
    public Stat attackDist;
    public Stat armor;
    public Stat speed;

    public bool dead=false;

    void Awake()
    {
        currentHealth = maxHealth;
    }
    

    public virtual void TakeDamage(float damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage,0,int.MaxValue);

        currentHealth -= damage;
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<DamageIndicator>().showIndicator("-"+damage, transform.position + Vector3.up * 1.5f);

        if (currentHealth <= 0)
        {
            Die();
        }

        EnemySample enSample = GetComponent<EnemySample>();
        if (enSample!=null)
        {
            if (enSample.onEnemyHPChange != null) enSample.onEnemyHPChange.Invoke();
        }
    }
    public virtual void Die()
    {
        //if(!dead) GetComponent<Animator>().SetTrigger("Die");
        dead = true;
    }

}
