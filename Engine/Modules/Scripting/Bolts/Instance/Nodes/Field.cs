using System;
using UnityEngine;

namespace CairoEngine.Scripting
{
    [Serializable]
    /// Fields are stored with their Type information to determine how they're Rendered and used by the Editor,
    /// and Get and Set functions to evaluate their Values.
    public class Field
    {
        /// <summary>
        /// The Base Field Type this is, controlling how it displays in the Editor
        /// </summary>
        public enum FieldType
        {
            String,
            Number,
            Vector2,
            Vector3,
            Operator,
            GameObject,
            Transform,
            ScriptableObject,
            Object,
            ObjectComponent,
            BehaviourType,
            Template
        }

        /// <summary>
        /// The Type of Component to use if this is an ObjectComponent Field
        /// </summary>
        public enum ComponentType
        {
            AIController,
            Controller,
            PlayerController,
            Entity,
            Pawn,
            Vehicle,
            InventoryItem,
            Item,
            Projectile,
            Weapon,
            KillZone,
            Level,
            SpawnPoint,
            Senser,
            Player,
            Team,
            ScriptHolder
        }

        /// <summary>
        /// The Type of Template to use if this is a Template Field
        /// </summary>
        public enum Template
        {
            BehaviourTypeTemplate,
            DirectorTemplate,
            ControllerTemplate,
            EntityTemplate,
            PawnTemplate,
            VehicleTemplate,
            GameModeTemplate,
            UITemplate,
            DamageTypeTemplate,
            InventoryItemTemplate,
            ProjectileTemplate,
            WeaponTemplate,
            LevelTemplate,
            NeuralNetworkTemplate,
            SenserTemplate,
            ObjectTemplate,
            TeamTemplate,
            ResourcePackage
        }

        /// <summary>
        /// Field Type Information
        /// </summary>
        [HideInInspector] public FieldType fieldType;
        [HideInInspector] public ComponentType componentType;
        [HideInInspector] public Template template;

        /// <summary>
        /// The Value of the Field
        /// </summary>
        private object value;

        public T Get<T>()
        {
            return (T)value;
        }

        public void Set(object value)
        {
            this.value = value;
        }
    }
}
