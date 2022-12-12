using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text Life;
    public bool hasBoss;
    public GameObject Dialog;
    public int NumberLifeOfPlayer;
    public GameObject[] Effect;
    public GameObject[] EffectOfPlayer;
    public string _state;
    public GameObject player;

    //Character
    public GameObject insSkin;
    public DBCharacter DBCharacter;
    public Text nameCharacter;
    private int skinSelected;

    void Start()
    {
        SystemVariable.gameController = this;
        _state = stateOfGame.Pause.ToString();
        SystemVariable.LifeOfPlayer = NumberLifeOfPlayer;
        if (Dialog != null)
        {
            Dialog.SetActive(false);
        }

        if (!PlayerPrefs.HasKey(SystemVariable.SKIN))
        {
            skinSelected = 0;
        }
        else
        {
            skinSelected = Prefs.SKIN;
        }
    }

    private void Update()
    {
        UpdateCharacter(skinSelected);
        Life.text = SystemVariable.LifeOfPlayer.ToString();
        EndGame();
    }

    //Process dialog
    public void OpenDialog() => Dialog.SetActive(true);

    public void EndGame()
    {
        if (SystemVariable.quantityCurrentHeart == 0 && SystemVariable.LifeOfPlayer > 0)
        {
            SystemVariable.quantityCurrentHeart = 4;
            SystemVariable.LifeOfPlayer--;
            foreach (GameObject obj in FindObjectOfType<PlayerController>().listHeart)
            {
                obj.SetActive(true);
            }
        }
        if (SystemVariable.quantityCurrentHeart == 0 && SystemVariable.LifeOfPlayer == 0)
        {
            SystemVariable.gameController._state = stateOfGame.GameOver.ToString();
        }
    }

    //Process skin

    public void NextSkin()
    {
        insSkin.GetComponent<Animator>().SetTrigger(stateOfSkin.MoveRight.ToString());
        skinSelected++;
        if(skinSelected >= DBCharacter.CountCharacter())
        {
            skinSelected = 0;
        }
        Prefs.SKIN = skinSelected;
    }

    public void BackSkin()
    {
        insSkin.GetComponent<Animator>().SetTrigger(stateOfSkin.MoveLeft.ToString());
        skinSelected--;
        if (skinSelected < 0)
        {
            skinSelected = DBCharacter.CountCharacter() - 1;
        }
        Prefs.SKIN = skinSelected;
    }

    private void UpdateCharacter(int Idxselected)
    {
        character objSelected = DBCharacter.GetCharacter(Idxselected);
        Color colorSkin = new Color(objSelected.SkinColor.r, objSelected.SkinColor.g, objSelected.SkinColor.b);
        player.GetComponent<SpriteRenderer>().color = colorSkin;
        insSkin.GetComponent<Image>().color = colorSkin;
        nameCharacter.text = objSelected.name;
        nameCharacter.color = colorSkin;
    }
}
