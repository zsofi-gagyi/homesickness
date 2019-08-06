using System;

namespace HomesicknessVisualiser.Services
{
    public static class indexCalculator
    {
        public static int getIndex(double bpTemp, double csTemp)
        {
            if (bpTemp > 24 && bpTemp > csTemp) {
                return Convert.ToInt32(Math.Pow((bpTemp - csTemp), 2) * (bpTemp - 24));
            }
            return 0;
        }
    }
}
