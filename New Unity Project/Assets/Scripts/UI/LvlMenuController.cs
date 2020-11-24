using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvlMenuController : MonoBehaviour
{
    private Button button;
    [SerializeField] private Scenes scene;

    void Start()
    {
        button = GetComponent<Button>();
        if (!PlayerPrefs.HasKey(GamePrefs.LvlPlayed.ToString() + ((int)scene).ToString()))
        {
            button.interactable = false;
            return;
        }

        button.onClick.AddListener(ChangeLvl);

        GetComponentInChildren<TMP_Text>().text = ((int)scene).ToString();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeLvl()
    {
        ServiceManager.Instanse.ChangeLvl((int)scene);
    }
}
