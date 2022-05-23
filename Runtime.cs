using System;
using System.Collections.Generic;
using UnityEngine;
using CairoEngine;

/// <summary>
/// The Runtime is the main Controller of the Entire Game. 
/// Use it to interface with the <see cref="Engine"/>'s Tools and create the Gameplay experience.
/// The Engine's State Machine is used to control Game State flow.
/// </summary>
public class Runtime : MonoBehaviour
{


    void Start()
    {
        StateMachineManager.EnableStateMachine(gameObject, this);
    }

    #region Game Modes
    public class GameMode_DeathMatch : State
    {
        //When entering the Game Mode, we want to pass Gameplay Management off to the Game Mode Manager by starting a Game
        public void Enter()
        {
            //Load the Level that we want to play on
            LevelManager.LoadLevel("TestLevel");
            //Start the Game Mode
            GameModeManager.StartGame("DeathMatch");
            //Reset the UI so that we can use it to display Gameplay information on the HUD.
            UIManager.ResetUI();
            //Set the UI Info to what will be used for this Game Mode
            UIManager.SetUI("CairoGame_Gameplay");


        }

        //In Update we will 
        public void Update()
        {
            //GameModeManager.SpawnDefaultPawn();
        }

        //When a team is Victorious, we want to end the Game after playing the Victory Timeline
        public void Victory()
        {
            GameModeManager.EndGame();
        }
    }
    #endregion
    #region Menus
    /// <summary>
    /// Controls Events that occur while the Game is on the Title Screen
    /// </summary>
    public class TitleScreen : State
    {

    }

    /// <summary>
    /// Controls Events that occur while the Game is on the Matchmaking Screen
    /// </summary>
    public class Matchmaking : State
    {

    }
    #endregion
}
