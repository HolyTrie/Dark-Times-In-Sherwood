using System.Collections;
using UnityEngine;

namespace DTIS
{
    public static class Util
    {
        public static IEnumerator Wait(float seconds = 1f)
        {
            yield return new WaitForSeconds(seconds);
        }

        public static class Constants
        {
            static public readonly int MaxActionsMidAir = 2;
        }
    }
}