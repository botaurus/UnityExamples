namespace BirdLib.Ecs
{
    public static class FlagUtils
    {
        /// <summary>
        /// checking 0 for 0 returns false 
        /// </summary>
        /// <param name="checkMe"></param>
        /// <param name="forThis"></param>
        /// <returns></returns>
        public static bool IsSet(int checkMe, int forThis)
        {
            return (checkMe & forThis) != 0;
        }

        public static void Set(ref int toMe, int addThis)
        {
            toMe |= addThis;
        }

        public static void Unset(ref int toMe, int removeThis)
        {
            toMe &= (~removeThis);
        }
    }
}