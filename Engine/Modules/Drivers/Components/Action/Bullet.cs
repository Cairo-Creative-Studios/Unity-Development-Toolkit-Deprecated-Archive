//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;

namespace CairoEngine.Drivers
{
    [Serializable]
    public class Bullet : Driver<DriverTemplate_Bullet>
    {
        /// <summary>
        /// Projectile shooter information.
        /// </summary>
        public int team = -1;
        public float life;
        /// <summary>
        /// The damage instegator to spawn once the Damage is Triggered
        /// </summary>
        public DamageInstegator damageInstegator;
        /// <summary>
        /// The Weapon that shot this Projectile
        /// </summary>
        public Weapon weapon;
        public Vector3 direction;
        public Vector3 speed;

        public bool fired = false;
        bool fireTriggered = false;

        [HideInInspector] public Transform originalParent;

        public bool returnToHolder = false;

        void Start()
        {

            originalParent = transform.parent;
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
                    template.scriptContainer.Events["Shot"].Invoke();
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

                bool canMove = true;

                if (template.controllable)
                {
                    if (inputs["Accelerate"] != 0)
                    {
                        speed = new Vector3(0, inputs["MoveVertical"] * template.directionalSpeed * Time.deltaTime, 0);
                        speed = speed.Lerp(direction * template.speed, template.acceleration); 
                    }
                    else
                    {
                        speed = new Vector3(0, inputs["MoveVertical"] * template.directionalSpeed * Time.deltaTime, 0);
                    }

                    transform.eulerAngles += new Vector3(inputs["RotateX"],inputs["RotateY"] * Time.deltaTime, 0);


                }
                else
                {
                    speed = speed.Lerp(direction * template.speed, template.acceleration);
                }

                core.Move(speed);

                life += Time.deltaTime;
                if (life > template.lifespan&&template.lifespan>0)
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
                    template.scriptContainer.Events["Timeout"].Invoke();
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
