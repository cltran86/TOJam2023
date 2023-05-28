using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public void GameOver()
    {
        print("You lose!  Loser!");
    }
}
