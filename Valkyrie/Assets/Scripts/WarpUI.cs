using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarpUI : MonoBehaviour
{
    Button accept;
    Player player;
    private void Awake()
    {
        accept = transform.GetChild(0).GetComponent<Button>();
        accept.onClick.AddListener(Warp);
    }
    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
        if (this.enabled)
        {
            gameObject.SetActive(false);
        }
    }
    void Warp()
    {
        GameManager.Inst.IsWarp = true;
        GameManager.Inst.SaveData(player);
        SceneManager.LoadScene(0);
    }
}
