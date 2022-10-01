using System;
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
    /// Interpolates between two <see cref="Material"/>s following a <see cref="AnimationCurve"/>.
    /// </summary>
    [HelpURL("https://kitbashery.com/docs/tween-components/tween-material.html")]
    [AddComponentMenu("Kitbashery/Tween/Tween Material")]
    [RequireComponent(typeof(Renderer))]
    public class TweenMaterial : TweenBase
    {
        [Header("Material:")]
        [Tooltip("The index of the material of TweenBase.rend to tween from.")]
        [Min(0)]
        public int index = 0;
        [Tooltip("The starting material.")]
        public Material initialMaterial;
        [Tooltip("The target material to tween to. (Call UpdateTargetMaterial to change during runtime).")]
        public Material targetMaterial;

        private Material currentMaterial;
        private bool pingPong = false;

        void Start()
        {
            if (rend == null)
            {
                rend = GetComponent<Renderer>();
            }

            if (index > rend.materials.Length)
            {
                Debug.LogWarning("|Tween Material|: Index is set to a higher value than its renderer has materials! Setting index to 0.", gameObject);
                index = 0;
            }

            if (rend.materials[index] != null)
            {
                rend.materials[index] = new Material(rend.sharedMaterials[index]);
            }
            else
            {
                Debug.LogWarningFormat(gameObject, "|Tween Material|: Failed to find a material at {0} on its renderer. This component will not function.", index);
            }

            if(targetMaterial == null || initialMaterial == null)
            {
                Debug.LogWarning("|Tween Material|: Missing an initial or target material. This component may not function.", gameObject);
            }

            UpdateTargetMaterial();
        }

        public override void UpdateTween()
        {
            if (rend.materials[index] != null || targetMaterial == null)
            {
                if (lerpTime < curve.keys[curve.length - 1].time)
                {
                    lerpTime += Time.deltaTime;
                    rend.materials[index].Lerp(initialMaterial, currentMaterial, curve.Evaluate(lerpTime));

                    onTweenUpdate.Invoke();
                }
                else
                {
                    switch (wrapMode)
                    {
                        case WrapMode.PingPong:

                            pingPong = !pingPong;
                            if (pingPong == true)
                            {
                                currentMaterial = initialMaterial;
                            }
                            else
                            {
                                currentMaterial = targetMaterial;
                            }

                            //ReverseCurve();
                            lerpTime = 0;

                            break;


                        case WrapMode.Loop:

                            rend.materials[index] = initialMaterial;
                            lerpTime = 0;

                            break;


                        case WrapMode.Once:

                            rend.materials[index] = initialMaterial;
                            tweening = false;

                            break;

                        case WrapMode.ClampForever:

                            rend.materials[index] = targetMaterial;
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
                Debug.LogWarning("|Tween Material|: Missing a target material or a material on its renderer.", gameObject);
            }
        }

        public void UpdateTargetMaterial()
        {
            currentMaterial = targetMaterial;
        }

        public void ResetToInitial()
        {
            rend.materials[index] = initialMaterial;
            ResetTweenState();
        }
    }
}