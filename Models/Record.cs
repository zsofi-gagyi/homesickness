using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomesicknessVisualiser.Models
{
    public class Record
    {
        [Key]
        public DateTime Time { get; set; }

        public float BpTemperature { get; set; }

        public float CsTemperature { get; set; }

        public int Index { get; set; }
    }
}
