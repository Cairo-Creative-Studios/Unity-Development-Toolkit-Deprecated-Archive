//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System.Collections.Generic;
using UnityEngine;

namespace CairoEngine
{
    [CreateAssetMenu(menuName = "Cairo Game/Entities/Pawn")]
    /// <summary>
    /// The Data for a Pawn
    /// </summary>
    public class PawnTemplate : EntityTemplate
    {
        //Initialize the Behaviour Class for this Behaviour
        private void OnEnable()
        {
            if(Class == "CairoEngine.Entity")
                this.Class = "CairoEngine.Pawn";
        }
    }
}

