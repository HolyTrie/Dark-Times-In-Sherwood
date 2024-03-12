using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DTIS
{
    public static class Util
    {
        public static PlayerController GetPlayerController()
        {
            // TODO: register the player GO in GameManager instead
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerController _pc);
            return _pc;
        }
        public static Vector2 Vector2DeepCopy(Vector2 vec)
        {
            return new Vector2(vec.x,vec.y);
        }
        public static IEnumerator Wait(float seconds = 1f)
        {
            yield return new WaitForSeconds(seconds);
        }

        public static IEnumerator DestroyGameObjectCountdown(GameObject gameObject, float ttl)
        {
            yield return new WaitForSeconds(ttl);
            GameObject.Destroy(gameObject);
        }

        public static Transform[] NearestNTransforms(IDictionary<int, Transform> transforms, Vector3 refPoint, int N = 1)
        { //https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
            return transforms.Values.ToArray()
                                    .OrderBy(t => (t.position - refPoint).sqrMagnitude)
                                    .Take(N)
                                    .ToArray();
        }

        public static void MimicEntityMovement(Transform Mimic, Transform Target, Vector3 FixedRelativePosition)
        {
            Mimic.position = Target.position + FixedRelativePosition;
        }
    }
}