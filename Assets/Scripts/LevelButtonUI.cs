using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonUI : MonoBehaviour
{
    [Header("Level Data")]
    private int level = 0;
    private int star = 0;
    private bool lvlFinished = false;

    [Header("Level Button")]
    public Sprite[] LvlButtonLockedSprites;
    public Sprite[] LvlButtonUnlockedSprites;
    public TextMeshProUGUI levelText;
    [Header("Star Sprite")]
    public GameObject[] stars;

    private Image levelButtonUI;
    private int c;
    // Start is called before the first frame update
    void Start()
    {
        levelButtonUI = GetComponent<Image>();
        c = Random.Range(1, 21);
        int l = Random.Range(1, 21);
        int s = Random.Range(1, 4);
        bool f = false;
        if (l < c)
        {
            f=true;
        }

        SetLevelData(l, s, f);
    }

    //Update Level Button UI
    public void LoadLvlData()
    {
        //Check if it is boss level
        int boss = 0;
        if(level%4 == 0) 
        { 
            boss = 1;
        }

        if(lvlFinished) 
        { 
            levelButtonUI.sprite = LvlButtonUnlockedSprites[boss];
            stars[star - 1].SetActive(true);
        }
        else
        {
            int currentLvl = 10; // This variable is temporary and will be removed after saving data from current lvl
            if(level > c)
            {
                levelButtonUI.sprite = LvlButtonLockedSprites[boss];
            }
            else
            {
                levelButtonUI.sprite = LvlButtonUnlockedSprites[boss];
            }
        }


        if (boss == 0 && lvlFinished)
        {
            levelText.text = level.ToString();
            levelText.gameObject.SetActive(true);
        }

    }

    //Variable Update Function
    public void SetLevelData(int _level, int _star, bool lvlStatus)
    {
        level = _level;
        star = _star;
        lvlFinished = lvlStatus;

        LoadLvlData();
    }

    public int GetLevel()
    {
        return level;
    }
}
