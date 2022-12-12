using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs
{
   public static int LEVEL
    {
        get => PlayerPrefs.GetInt(SystemVariable.LEVEL);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.LEVEL,value);
        }
    }

    public static int COINT
    {
        get => PlayerPrefs.GetInt(SystemVariable.COINT);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.COINT, value);
        }
    }

    public static int RUBI
    {
        get => PlayerPrefs.GetInt(SystemVariable.RUBI);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.RUBI, value);
        }
    }

    public static int LIFE
    {
        get => PlayerPrefs.GetInt(SystemVariable.LIFE);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.LIFE, value);
        }
    }

    public static int SKIN
    {
        get => PlayerPrefs.GetInt(SystemVariable.SKIN);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.SKIN, value);
        }
    }
}
