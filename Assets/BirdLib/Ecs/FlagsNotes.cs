#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BirdLib.Ecs
{
    public static class FlagNotes
    {
        [MenuItem("Bird Lib/Notes/Ecs Flag Notes")]
        public static void RunDemo()
        {
            int demo=0;
            Debug.Log("check 0 for 0: " + FlagUtils.IsSet(demo, 0));
            FlagUtils.Set(ref demo, 1);
            Debug.Log("set 1 into 0 and check for 0: " + FlagUtils.IsSet(demo, 0));
            Debug.Log("set 1 into 0 and check for 1: " + FlagUtils.IsSet(demo, 1));
            demo = 0;
            FlagUtils.Set(ref demo, 3);
            Debug.Log("set 3 into 0 and check for 3: " + FlagUtils.IsSet(demo, 3));
            demo = 3;
            FlagUtils.Set(ref demo, 5);
            Debug.Log($"set 5 into 3 ({demo}) and check for 5: " + FlagUtils.IsSet(demo, 5));
            Debug.Log($"set 5 into 3 ({demo}) and check for 7: " + FlagUtils.IsSet(demo, 8));
            Debug.Log($"set 5 into 3 ({demo}) and check for 3: " + FlagUtils.IsSet(demo, 3));
        }
    }
}
#endif