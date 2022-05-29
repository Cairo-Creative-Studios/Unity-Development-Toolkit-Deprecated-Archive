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
        StateMachineModule.EnableStateMachine(gameObject, this);
    }

    #region Game Modes
    public class GameMode_DeathMatch : State
    {
        List<GameObject> TestPool = new List<GameObject>();

        //When entering the Game Mode, we want to pass Gameplay Management off to the Game Mode Manager by starting a Game
        public void Enter()
        {
            //Load the Level that we want to play on
            //LevelModule.LoadLevel("TestLevel");
            //Start the Game Mode
            //GameModeModule.StartGame("DeathMatch");
            //Reset the UI so that we can use it to display Gameplay information on the HUD.
            UIModule.ResetUI();
            //Set the UI Info to what will be used for this Game Mode
            UIModule.SetUI("CairoGame_Gameplay");

            MLModule.CreateNetwork("TestNetwork");
            MLModule.CreateNetwork("TestNetwork");
            MLModule.CreateNetwork("TestNetwork");
            //TestPool.Add(NNModule.CreateSpecimen("MLTest"));
            //TestPool.Add(NNModule.CreateSpecimen("MLTest"));
            //TestPool.Add(NNModule.CreateSpecimen("MLTest"));
        }

        //In Update we will 
        public void Update()
        {
            //foreach(GameObject test in TestPool)
            //{
            //    for(int i = 0; i < TestPool.Count-1; i++)
            //    {

            //        NNModule.SetInput(test, i, UnityEngine.Random.Range(0, 1));
            //        NNModule.SetTarget(test, new float[] { 5.0f, 2.0f });
            //    }
            //}
            //GameModeManager.SpawnDefaultPawn();
        }

        //When a team is Victorious, we want to end the Game after playing the Victory Timeline
        public void Victory()
        {
            GameModeModule.EndGame();
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
