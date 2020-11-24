using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuController : BaseGameMenuController
{
    [SerializeField] private Button restart;
    [SerializeField] private Button mainMenu;

    protected override void Start()
    {
        base.Start();
        play.onClick.AddListener(ChangeMenuStatus);
        restart.onClick.AddListener(serviceManager.Restart);
        mainMenu.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            ChangeMenuStatus();
    }

    protected override void OnDestroy()
    {
        play.onClick.RemoveListener(ChangeMenuStatus);
        restart.onClick.RemoveListener(serviceManager.Restart);
        mainMenu.onClick.RemoveListener(GoToMainMenu);
    }
    

    protected override void ChangeMenuStatus()
    {
        base.ChangeMenuStatus();
        Time.timeScale = menu.activeInHierarchy ? 0 : 1;
    }

    public void GoToMainMenu()
    {
        ServiceManager.Instanse.ChangeLvl((int)Scenes.MainMenu);
    }
}
