﻿using System;

namespace HomesicknessVisualiser.Services
{
    public static class IndexCalculator
    {
        public static int getIndex(double bpTemp, double csTemp)
        {
            if (bpTemp > 21 && bpTemp > csTemp) {                                           // Causes for homesickness:      
                return Convert.ToInt32(Math.Pow(bpTemp - csTemp, 3) * (bpTemp - 21));       // - when summers are too warm
            }
            
            if (bpTemp < 16 && bpTemp > 0 && csTemp < 0)
            {
                return Convert.ToInt32(Math.Pow(bpTemp - csTemp, 2) * (bpTemp));            // - when winters are not cold enough for snow
            }

            return 0;
        }
    }
}
