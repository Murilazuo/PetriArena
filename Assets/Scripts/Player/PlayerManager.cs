using UnityEngine;
using System;

[DefaultExecutionOrder(-1)]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] private float life;
    public float points = 0;
    [SerializeField] private float[] pointsToNextUpgrade;
    float nextUpgrade;
    public float maxLife;
    public static PlayerManager instance;
    public BarUi lifeUi, pointsUi;
    public int level;
    public bool canUpgrade = false;
    SoundManager soundManager;
    public static Action OnUpgrade;
    [SerializeField]int[] orbByLevel;

    DisplayLevel displayLevel;

    Animator anim;

    private void Awake()
    {
        instance = this;
        soundManager = SoundManager.soundManager;
        transform.position = new Vector2(Mathf.Round(transform.position.x / 20) * 20, 
            Mathf.Round(transform.position.y / 20) * 20);
    }
    private void Start()
    {
        nextUpgrade = pointsToNextUpgrade[0];
        life = maxLife;
        level = 0;
        displayLevel = FindObjectOfType<DisplayLevel>();
        anim = GetComponent<Animator>();
    }
    internal void SetLife(float damage)
    {
        life += damage;
        
        life = Mathf.Clamp(life, 0, maxLife);

        if(life == 0)
        {
            lifeUi.SetBar(life, maxLife);

            GameManager.instance.EndGame();
            return;
        }

        lifeUi.SetBar(life, maxLife);

        if (!canUpgrade)
        {
            if (damage < 0)
            {
                points += damage * -1;
                
            }


            if(points >= nextUpgrade)
            {
                Upgrade();
            }   
        }
        
        pointsUi.SetBar(points, nextUpgrade);
    }

    private void Upgrade()
    {
        level++;
        int levelId = 1;
        switch (level)
        {
            case 3:
            case 4:
                levelId = 2;
                break;
            case 5:
            case 6:
                levelId = 2;
                break;
            case 7:
            case 8:
                levelId = 2;
                break;
            case 9:
            case 10:
                levelId = 2;
                break;
        }
        anim.SetInteger("Level", levelId);

        if(level < orbByLevel.Length)
        {
            LifeOrbController.AddPercentage(orbByLevel[level]);
        }
        points = 0;

        displayLevel.UpdateLevel();
        soundManager.StopSound("WalkAntibiotic");
        Time.timeScale = 0;
        if(level < pointsToNextUpgrade.Length)
        {
            nextUpgrade = pointsToNextUpgrade[level];
        }
        OnUpgrade?.Invoke();
        canUpgrade = true;
        soundManager.PlaySound("Evolve");
    }

    

    
}
