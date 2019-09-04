using System;

namespace HomesicknessVisualiser.Services
{
    public static class IndexCalculator
    {
        public static int getIndex(double bpTemp, double csTemp)
        {
            if (bpTemp > 24 && bpTemp > csTemp) {                                           // Causes for homesickness:      
                return Convert.ToInt32(Math.Pow(bpTemp - csTemp, 2) * (bpTemp - 24));       // - when summers are too warm
            }
            else if (bpTemp < 12 && bpTemp > 0 && csTemp < 0)
            {
                return Convert.ToInt32(Math.Pow(bpTemp - csTemp, 2) * (bpTemp));            // - when winters are cold, but not cold enough for snow
            }
            return 0;
        }
    }
}
