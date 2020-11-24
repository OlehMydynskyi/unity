using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : BaseGameMenuController
{
    [SerializeField] private Button chooseLvl;
    [SerializeField] private Button reset;

    [SerializeField] private GameObject lvlMenu;
    [SerializeField] private Button closeLvlMenu;

    private int lvl = 1;

    protected override void Start()
    {
        base.Start();
        chooseLvl.onClick.AddListener(UseLvlMenu);
        closeLvlMenu.onClick.AddListener(UseLvlMenu);

        if (PlayerPrefs.HasKey(GamePrefs.LastPlayedLvl.ToString()))
        {
            play.GetComponentInChildren<TMP_Text>().text = "Resume";
            lvl = PlayerPrefs.GetInt(GamePrefs.LastPlayedLvl.ToString());
        }

        play.onClick.AddListener(Play);
        reset.onClick.AddListener(OnResetClicked);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        chooseLvl.onClick.RemoveListener(UseLvlMenu);
        closeLvlMenu.onClick.RemoveListener(UseLvlMenu);
        play.onClick.RemoveListener(Play);
        reset.onClick.RemoveListener(OnResetClicked);
    }

    private void UseLvlMenu ()
    {
        lvlMenu.SetActive(!lvlMenu.activeInHierarchy);
        ChangeMenuStatus();
    }

    private void Play ()
    {
        Debug.Log(PlayerPrefs.GetInt(GamePrefs.LastPlayedLvl.ToString()));
        serviceManager.ChangeLvl(lvl);
    }

    private void OnResetClicked ()
    {
        play.GetComponentInChildren<TMP_Text>().text = "Play";
        serviceManager.ResetProgres();
        lvl = 1;
    }
}
