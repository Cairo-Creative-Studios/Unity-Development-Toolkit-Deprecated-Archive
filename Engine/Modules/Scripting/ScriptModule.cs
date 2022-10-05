/*! \addtogroup scriptmodule Script Module
 *  Additional documentation for group 'Script Module'
 *  @{
 */

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UDT.Reflection;

namespace UDT.Scripting
{
    /// <summary>
    /// The Script Module uses an Event Sheet Scripting interface for constructing Gameplay Scenarios
    /// </summary>
    public class ScriptModule
    {
        /// <summary>
        /// Condition ACES grouped under the Types that they're meant to be used for, as a refernece for the Edior.
        /// </summary>
        public static SDictionary<Type, object> conditions = new SDictionary<Type, object>();
        /// <summary>
        /// Action ACES grouped under the Types that they're meant to be used for, as a refernece for the Edior.
        /// </summary>
        public static SDictionary<Type, object> actions = new SDictionary<Type, object>();
        /// <summary>
        /// Trigger ACES grouped under the Types that they're meant to be used for, as a refernece for the Edior.
        /// </summary>
        public static SDictionary<Type, object> triggers = new SDictionary<Type, object>();

        public static string resolve(object instance, string expression)
        {
            return "";
        }

        /// <summary>
        /// Adds the ACES Script to the appropriate Category, so that it can be used in Event Sheets
        /// </summary>
        /// <param name="acesInstance">Aces instance.</param>
        public static void AddAces(object acesInstance)
        {
            string acesName = acesInstance.GetType().Name;
            List<Type> types = (List<Type>)acesInstance.GetField("types");

            foreach (Type type in types)
            {
                switch (acesName.TokenAt(0, '_'))
                {
                    case ("Condition"):
                        conditions.Add(type, acesInstance);
                        break;
                    case ("Action"):
                        actions.Add(type, acesInstance);
                        break;
                    case ("Trigger"):
                        triggers.Add(type, acesInstance);
                        break;
                }
            }
        }
    }
}
