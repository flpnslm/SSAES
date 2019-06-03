using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class RoundConstant
    {
        public Dictionary<int, byte> roundConstants { get; set; }
        public static RoundConstant roundConstant { get; set; }


        public RoundConstant() {
            roundConstants = new Dictionary<int, byte>();
            this.Initializer();
        }

        public static int GetConstant(int iterator) {
            if (roundConstant == null) {
                roundConstant = new RoundConstant();
            }
            return roundConstant.roundConstants[iterator];
        }

        private void Initializer() {
            roundConstants.Add(0, Convert.ToByte("01"));
            roundConstants.Add(1, Convert.ToByte("02"));
            roundConstants.Add(2, Convert.ToByte("04"));
            roundConstants.Add(3, Convert.ToByte("08"));
            roundConstants.Add(4, Convert.ToByte("16"));
            roundConstants.Add(5, Convert.ToByte("32"));
            roundConstants.Add(6, Convert.ToByte("64"));
            roundConstants.Add(7, Convert.ToByte("128"));
            roundConstants.Add(8, Convert.ToByte("27"));
            roundConstants.Add(9, Convert.ToByte("54"));
        }

    }
}
