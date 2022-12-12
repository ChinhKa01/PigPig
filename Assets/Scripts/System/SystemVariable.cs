using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemVariable
{
    //Static
    public static GameController gameController;

    public static BossController bossController;
    public static PlayerController playerController;

    public static int quantityCurrentHeart;

    public static int LifeOfPlayer;

    //Const
    public const string LEVEL = "Level";
    public const string COINT = "Coint";
    public const string LIFE = "Life";
    public const string RUBI = "Rubi";
    public const string SKIN = "Skin";
}

//Enum
public enum stateOfPlayer
{
    Appear,
    Run,
    IsJumpingUp,
    IsJumpingDown,
    Teleport,
    Attack,
    Attack2,
    Death
}

public enum stateOfSkin
{
    MoveLeft,
    MoveRight
}

enum stateOfBoss
{
    Walk,
    Attack,
    UseSkill1,
    UseSkill2,
    BeAttacked,
    Death
}
public enum Tag
{
    Player,
    Enemy,
    Rubi,
    Dart
}

public enum stateOfGame
{
    Play,
    Pause,
    GameOver
}

public enum DialogAnim
{
    Open
}

public enum Cards
{
    Open
}


