using System.Collections.Generic;

namespace Game.DamageNumberGraphics
{
    public static class DigitSplitter
    {
        public static List<int> breakdown(int number)
        {
            List<int> digits = new List<int>();

            while (number > 0)
            {
                digits.Add(number % 10);
                number = number / 10;
            }

            digits.Reverse();
            return digits;
        }
    }
}

