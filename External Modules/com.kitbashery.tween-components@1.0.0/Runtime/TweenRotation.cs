using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 MIT License

Copyright (c) 2022 Kitbashery

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.



Need support or additional features? Please visit https://kitbashery.com/
*/

namespace Kitbashery.Tween
{
    /// <summary>
    /// Rotates to a destination rotation following a <see cref="AnimationCurve"/>.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-rotation.html")]
    [DisallowMultipleComponent]
    [AddComponentMenu("Kitbashery/Tween/Tween Rotation")]
    public class TweenRotation : TweenBase
    {
        [Header("Rotation:")]
        public Vector3 target;
        [Tooltip("Spin around target axis.")]
        public bool spin = false;
        [Range(0, 360)]
        public float spinSpeed = 1f;

        private Quaternion initialRot;
        private Vector3 initialTarget;
        private Vector3 modifiedTarget;
        private Quaternion currentTarget;

        private float initialSpinSpeed = 1;
        private bool initialSpinValue = false;

        /// <summary>
        /// The next rotation the transform will be at (usefull for networking/physics).
        /// </summary>
        [HideInInspector]
        public Quaternion nextRot;

        // Start is called before the first frame update
        void Start()
        {
            initialRot = transform.rotation;
            initialTarget = target;
            nextRot = Quaternion.Euler(initialRot.eulerAngles);
            currentTarget = Quaternion.Euler(target);

            initialSpinSpeed = spinSpeed;
            initialSpinValue = spin;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                UnityEditor.Handles.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(target), Vector3.one) * Matrix4x4.Rotate(transform.rotation);
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireArc(Vector3.zero, Vector3.right, Vector3.forward, target.x, 1f);
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.right, target.y, 1f);
                UnityEditor.Handles.color = Color.blue;
                UnityEditor.Handles.DrawWireArc(Vector3.zero, Vector3.forward, Vector3.up, target.z, 1f);

                UnityEditor.Handles.color = new Color(0, 1, 1, 0.25f);
                UnityEditor.Handles.DrawSolidArc(Vector3.zero, Vector3.right, Vector3.forward, target.x, 0.99f);
                UnityEditor.Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.right, target.y, 0.99f);
                UnityEditor.Handles.DrawSolidArc(Vector3.zero, Vector3.forward, Vector3.up, target.z, 0.99f);

                UnityEditor.Handles.Label(Vector3.forward, new GUIContent(target.x + "°"));
                UnityEditor.Handles.Label(Vector3.right, new GUIContent(target.y + "°"));
                UnityEditor.Handles.Label(Vector3.up, new GUIContent(target.z + "°"));
            }
        }
#endif

        public override void UpdateTween()
        {
            if(target != initialTarget)
            {
                UpdateTarget();
            }

            if(spin == false)
            {
                if (lerpTime < curve.keys[curve.length - 1].time)
                {
                    lerpTime += Time.deltaTime;
                    nextRot = Quaternion.Lerp(transform.rotation, currentTarget, curve.Evaluate(lerpTime));
                    transform.rotation = nextRot;
                    onTweenUpdate.Invoke();
                }
                else
                {
                    switch (wrapMode)
                    {
                        case WrapMode.PingPong:

                            if (currentTarget == initialRot)
                            {
                                currentTarget = Quaternion.Euler(target);
                            }
                            else
                            {
                                currentTarget = initialRot;
                            }
                            lerpTime = 0;

                            break;


                        case WrapMode.Loop:

                            transform.rotation = initialRot;
                            lerpTime = 0;

                            break;


                        case WrapMode.Once:

                            transform.rotation = initialRot;
                            tweening = false;

                            break;

                        case WrapMode.ClampForever:

                            transform.rotation = currentTarget;
                            tweening = false;

                            break;

                        case WrapMode.Default:

                            lerpTime = 0;

                            break;

                        default:

                            lerpTime = 0;

                            break;
                    }

                    onTweenComplete.Invoke();
                }
            }
            else
            {
                transform.Rotate(currentTarget.eulerAngles, spinSpeed);
            }
        }

        private void UpdateTarget()
        {
            modifiedTarget = initialRot.eulerAngles + target;
            if (currentTarget != initialRot)
            {
                currentTarget = Quaternion.Euler(modifiedTarget);
            }
        }

        public void RotateToTarget()
        {
            currentTarget = Quaternion.Euler(target);
            tweening = true;
        }

        public void RotateToInitialPos()
        {
            currentTarget = initialRot;
            tweening = true;
        }

        public void SetTransformTarget(Transform t)
        {
            target = t.rotation.eulerAngles;
            currentTarget = t.rotation;
        }

        public void SetRotationTarget(Quaternion t)
        {
            target = t.eulerAngles;
            currentTarget = t;
        }

        public void ResetToInitial()
        {
            spin = initialSpinValue;
            spinSpeed = initialSpinSpeed;
            transform.rotation = initialRot;
            ResetTweenState();
        }
    }
}