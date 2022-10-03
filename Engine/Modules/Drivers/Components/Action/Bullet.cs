//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using Homebrew;
using UnityEngine;

namespace CairoEngine.Drivers
{
    [Serializable]
    public class Bullet : Driver<DriverTemplate_Bullet>
    {

        /// <summary>
        /// The current Lifespan of the Bullet
        /// </summary>
        [Tooltip("The current Lifespan of the Bullet")]
        public float life = 0;
        /// <summary>
        /// The Direction the Bullet is moving in
        /// </summary>
        [Tooltip("The Direction the Bullet is moving in")]
        [Foldout("Properties")]
        public Vector3 direction;
        /// <summary>
        /// The moving Speed of the Bullet
        /// </summary>
        [Tooltip("The moving Speed of the Bullet")]
        [Foldout("Properties")]
        public Vector3 speed = new Vector3(0, 0, 0);
        /// <summary>
        /// The Speed given from Inputs into the Bullet driver
        /// </summary>
        [Tooltip("The Speed given from Inputs into the Bullet driver")]
        [Foldout("Properties")]
        public Vector3 inputSpeed = new Vector3(0, 0, 0);
        /// <summary>
        /// Projectile shooter information.
        /// </summary>
        [Tooltip("Projectile shooter information.")]
        [Foldout("Player")]
        public int team = -1;
        /// <summary>
        /// The Weapon that shot this Projectile
        /// </summary>
        [Tooltip("The Weapon that shot this Projectile")]
        [Foldout("Components")]
        public Weapon weapon;
        /// <summary>
        /// The damage instegator to spawn once the Damage is Triggered
        /// </summary>
        [Tooltip("The damage instegator to spawn once the Damage is Triggered")]
        [Foldout("Components")]
        public DamageInstegator damageInstegator;
        /// <summary>
        /// Whether the Bullet has been Fired
        /// </summary>
        [Tooltip("Whether the Bullet has been Fired")]
        [Foldout("State")]
        public bool fired = false;
        /// <summary>
        /// Whether the Bullet should Return to it's Parent
        /// </summary>
        [Tooltip("Whether the Bullet should Return to it's Parent")]
        [Foldout("State")]
        public bool returnToHolder = false;

        /// <summary>
        /// If the Fire Method has been Triggered
        /// </summary>
        bool fireTriggered = false;
        /// <summary>
        /// The Parent of the Bullet, for Returning To Holder
        /// </summary>
        [HideInInspector] public Transform originalParent;

        void OnEnable()
        {
            if (template!=null&&template.forceDirection)
                direction = template.startDirection;
        }

        void Start()
        {
            originalParent = transform.parent;

            if (template.forceDirection)
                direction = template.startDirection;

            if (direction.x == 0 && direction.y == 0 && direction.z == 0)
            {
                Debug.LogWarning("Direction has not been set for the Bullet Driver of " + gameObject.name + ", the Bullet Driver will not move the Object.");
            }
        }

        /// <summary>
        /// Creates a Damage Instegator 
        /// </summary>
        public void Damage()
        {
            DamageInstegator.Instantiate(damageInstegator, transform.position, core, team);
        }

        void Update()
        {
            if (template.autofire)
                fired = true;

            if (fired)
            {
                if (!fireTriggered)
                {
                    template.scriptContainer.events["Shot"].Invoke();
                    fireTriggered = true;
                    if (!template.setAngle)
                        transform.eulerAngles = template.startingAngle;
                }

                transform.SetParent(null);

                //Handle Display
                if (template.setAngle)
                {
                    transform.eulerAngles = direction;
                }
                else
                {
                    transform.eulerAngles += template.rotation;
                }

                //bool canMove = true;

                Vector3 moveDir = Vector3.zero;
                if (template.controllable)
                {
                    //transform.eulerAngles += new Vector3(inputs["RotateX"],inputs["RotateY"] * Time.deltaTime, 0);

                    float inputDirection = 0;
                    inputSpeed = new Vector3(inputs["MoveHorizontal"], 0, 0);
                    inputDirection = Mathf.Atan2(inputSpeed.normalized.x, inputSpeed.normalized.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

                    moveDir = Quaternion.Euler(0, inputDirection, 0) * Vector3.forward * template.inputMaxSpeed.x;
                }

                speed = speed.Lerp(direction * template.speed, template.acceleration) + new Vector3(0,inputs["MoveVertical"],0);

                if (template.enableGravity)
                    speed += Physics.gravity;

                core.Move(speed + moveDir);

                life += Time.deltaTime;
                if (life > template.lifespan && template.lifespan > 0)
                {
                    Timeout();
                }
            }

            if (returnToHolder)
            {
                transform.position = transform.position.Lerp(originalParent.position,0.3f);

                //Handle Display
                if (template.setAngle)
                {
                    transform.eulerAngles = new Vector3(Mathf.Clamp(transform.position.x - originalParent.position.x, -1, 1), Mathf.Clamp(transform.position.y - originalParent.position.y, -1, 1), Mathf.Clamp(transform.position.z - originalParent.position.z, -1, 1));
                }
                else
                {
                    transform.eulerAngles += template.rotation;
                }
            }
        }

        void Timeout()
        {
            switch (template.timeoutFunction)
            {
                case DriverTemplate_Bullet.TimeoutFunction.Destroy:
                    GameObject.Destroy(gameObject);
                    break;
                case DriverTemplate_Bullet.TimeoutFunction.Return:
                    returnToHolder = true;
                    fired = false;
                    break;
                case DriverTemplate_Bullet.TimeoutFunction.CallEvent:
                    template.scriptContainer.events["Timeout"].Invoke();
                    break;
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if(collider.transform == originalParent && returnToHolder)
            {
                transform.SetParent(collider.transform);
                returnToHolder = false;
                life = 0;
                fireTriggered = false;
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                weapon.ammo++;
            }
        }
    }
}
