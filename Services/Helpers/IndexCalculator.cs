using System;

namespace HomesicknessVisualiser.Services
{
    public static class IndexCalculator
    {
        public static int CalculateIndex(double bpTemp, double csTemp)
        {
            if (bpTemp > 21 && bpTemp > csTemp) {                                           // Causes for homesickness:      
                return Convert.ToInt32(Math.Pow(bpTemp - csTemp, 3) * (bpTemp - 21));       // - when summers are too warm
            }
            else if (bpTemp < 12 && bpTemp > 0 && csTemp < 0)
            {
                return Convert.ToInt32(Math.Pow(bpTemp - csTemp, 2) * (bpTemp));            // - when winters are cold, but not cold enough for snow
            }
            return 0;
        }
    }
}
