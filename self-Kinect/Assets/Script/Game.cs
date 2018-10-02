using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour {

    //public static bool GameMode = false;
    //public static int game_result;
    
    class gameresult
    {
        public const int draw = 1;
        public const int win = 2;
        public const int lose = 3;
    }
    class gamepose
    {
        public const int rock = 3;
        public const int paper = 4;
        public const int scissors = 5;
    }
    class gmaemode
    {
        public const bool on = true;
        public const bool off = false;
    }
    
    
    public static int Debedegame(int avatar , int unitychan)
    {
        //rock=0, paper=1, scissors=2
       
            if (avatar == unitychan)
            {
                return gameresult.draw;
            }

            else if ((avatar == gamepose.rock) &&  (unitychan== gamepose.paper))
            {
                return gameresult.lose;
            }
            else if ((avatar == gamepose.rock) && (unitychan == gamepose.scissors))
            {
                return gameresult.win;
            }

            else if ((avatar == gamepose.scissors) && (unitychan == gamepose.rock))
            {
                return gameresult.lose;
            }
            else if ((avatar == gamepose.scissors) &&( unitychan == gamepose.paper))
            {
                return gameresult.win;
            }

            else if ((avatar == gamepose.paper) &&( unitychan == gamepose.scissors))
            {
                return gameresult.lose;
            }
            else if ((avatar == gamepose.paper) && (unitychan == gamepose.rock))
            {
                return gameresult.win;
            }

        return 0;
    }



}
