using System;
using System.IO;

namespace WindowsFormsApp1
{
    public class MatrixRoundKey
    {
        private AesMatrix crifredText;
        public MatrixRoundKey(AesMatrix crifredText)
        {
            this.crifredText = crifredText;
        }

        public AesMatrix GetRounds(byte[,] keyScheduler)
        {
            for (int i = 0; i < 9; i++)
            {
                this.SubBytes();
                Console.WriteLine(String.Format("****SubBytes-Round {0}****", i + 1));
                this.crifredText.Print();
                this.ShiftRows();
                Console.WriteLine(String.Format("****ShiftRows-Round {0}****", i + 1));
                this.crifredText.Print();
                this.MixColumn(i);
                Console.WriteLine(String.Format("****MixedColumns-Round {0}****", i + 1));
                this.crifredText.Print();
                this.addRoundKey(keyScheduler, i);
                Console.WriteLine(String.Format("****addRoundKey-Round {0}****", i + 1));
                this.crifredText.Print();
            }
            this.SubBytes();
            Console.WriteLine(String.Format("****SubBytes-Round {0}****", 10));
            this.crifredText.Print();
            this.ShiftRows();
            Console.WriteLine(String.Format("****ShiftRows-Round {0}****", 10));
            this.crifredText.Print();
            this.addRoundKey(keyScheduler, 9);
            Console.WriteLine(String.Format("****addRoundKey-Round {0}****", 10));
            this.crifredText.Print();
            return this.crifredText;
        }

        private void addRoundKey(byte[,] keyScheduler, int roundKey)
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    this.crifredText.matrix[j, i] = Convert.ToByte(
                        this.crifredText.matrix[j, i] ^
                        keyScheduler[j, (((roundKey + 1) * 4) + i)]
                    );
                }
            }
        }

        private void MixColumn(int roundKey)
        {
            byte[,] mixColumn = new byte[4, 4];
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    var result = (
                        nvl(this.crifredText.matrix[0, i], this.getMatrixMixColumn(j)[0], i, j, roundKey, 1) ^
                        nvl(this.crifredText.matrix[1, i], this.getMatrixMixColumn(j)[1], i, j, roundKey, 2) ^
                        nvl(this.crifredText.matrix[2, i], this.getMatrixMixColumn(j)[2], i, j, roundKey, 3) ^
                        nvl(this.crifredText.matrix[3, i], this.getMatrixMixColumn(j)[3], i, j, roundKey, 4)
                    );
                    mixColumn[j, i] = Convert.ToByte(result);
                }
            }
            this.crifredText.matrix = mixColumn;
        }

        private int nvl(int val1, int val2, int i, int j, int roundKey, int vt)
        {
            var val1Result = (ValidateWidthValue(TableL.Replace(val1.ToString("X2")) + TableL.Replace(val2.ToString("X2")))).ToString("X2");

            if (val1.Equals(0) || val2.Equals(0))
            {
                return 0;
            }
            if (val1.Equals(1))
            {
                return val2;
            }
            if (val2.Equals(1))
            {
                return val1;
            }
            var vr = TableE.Replace(ValidateWidthValue(TableL.Replace(val1.ToString("X2")) + TableL.Replace(val2.ToString("X2"))).ToString("X2"));
            return vr;
        }

        private int ValidateWidthValue(int value)
        {
            if (value > 255)
            {
                return value - 255;
            }
            return value;
        }

        private int[] getMatrixMixColumn(int line)
        {
            switch (line)
            {
                case 0:
                    return new int[4] { 2, 3, 1, 1 };
                case 1:
                    return new int[4] { 1, 2, 3, 1 };
                case 2:
                    return new int[4] { 1, 1, 2, 3 };
                case 3:
                    return new int[4] { 3, 1, 1, 2 };
            }
            return null;
        }

        public void SubBytes()
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    this.crifredText.matrix[i, j] = SBox.Replace(this.crifredText.matrix[i, j].ToString("X2"));
                }
            }
        }

        private void ShiftRows()
        {
            var v1 = this.crifredText.matrix[1, 0];
            this.crifredText.matrix[1, 0] = this.crifredText.matrix[1, 1];
            this.crifredText.matrix[1, 1] = this.crifredText.matrix[1, 2];
            this.crifredText.matrix[1, 2] = this.crifredText.matrix[1, 3];
            this.crifredText.matrix[1, 3] = v1;

            var v11 = this.crifredText.matrix[2, 0];
            var v2 = this.crifredText.matrix[2, 1];
            this.crifredText.matrix[2, 0] = this.crifredText.matrix[2, 2];
            this.crifredText.matrix[2, 1] = this.crifredText.matrix[2, 3];
            this.crifredText.matrix[2, 2] = v11;
            this.crifredText.matrix[2, 3] = v2;

            var v111 = this.crifredText.matrix[3, 0];
            var v22 = this.crifredText.matrix[3, 1];
            var v3 = this.crifredText.matrix[3, 2];
            this.crifredText.matrix[3, 0] = this.crifredText.matrix[3, 3];
            this.crifredText.matrix[3, 1] = v111;
            this.crifredText.matrix[3, 2] = v22;
            this.crifredText.matrix[3, 3] = v3;
        }
    }
}
