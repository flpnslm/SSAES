using System;
using System.IO;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class RoundKey
    {
        private AesMatrix aesMatrix { get; set; }
        private byte[,] keyScheduler { get; set; }

        public RoundKey(AesMatrix aesMatrix)
        {
            this.aesMatrix = aesMatrix;
            this.keyScheduler = new byte[4, 44];
        }

        public byte[,] getAesMatrixCifred()
        {
            this.CreateKeySchedule();
            byte[] newKeyScheduler = new byte[4];
            for (int roundKey = 0; roundKey < 10; roundKey++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i.Equals(0))
                    {
                        this.generateFirstKeyScheduler(newKeyScheduler, roundKey, i);
                    }
                    else
                    {
                        this.generateLastedKeyScheduler(roundKey, i);
                    }
                }
                this.PrintKeyScheduler(roundKey);
                Console.WriteLine();
            }
            return keyScheduler;
        }

        private void generateLastedKeyScheduler(int roundKey, int index)
        {
            var propositionalKeyInt = (roundKey * 4);
            var propositionalKey = new byte[4] {
                this.keyScheduler[0, propositionalKeyInt + index],
                this.keyScheduler[1, propositionalKeyInt + index],
                this.keyScheduler[2, propositionalKeyInt + index],
                this.keyScheduler[3, propositionalKeyInt + index]
            };
            var key = ((roundKey + 1) * 4) + (index - 1);
            var newKeyScheduler = new byte[4] {
                this.keyScheduler[0, key],
                this.keyScheduler[1, key],
                this.keyScheduler[2, key],
                this.keyScheduler[3, key]
            };
            for (int i = 0; i < 4; i++)
            {
                this.keyScheduler[i, key + 1] = Convert.ToByte(propositionalKey[i] ^ newKeyScheduler[i]);
            }
        }

        private void generateFirstKeyScheduler(byte[] newKeyScheduler, int roundKey, int i)
        {
            newKeyScheduler = this.RotateByte(roundKey, i);
            newKeyScheduler = this.SubByte(newKeyScheduler);
            newKeyScheduler = this.XORWithRoundConstant(roundKey, newKeyScheduler);
            newKeyScheduler = this.XORWithPropositionalKeyScheduler(i, roundKey, newKeyScheduler);
            this.SetKeyScheduler(newKeyScheduler, roundKey, i);
        }

        private void SetKeyScheduler(byte[] newKeyScheduler, int roundKey, int indexRound)
        {
            var index = ((roundKey + 1) * 4) + indexRound;
            this.keyScheduler[0, index] = newKeyScheduler[0];
            this.keyScheduler[1, index] = newKeyScheduler[1];
            this.keyScheduler[2, index] = newKeyScheduler[2];
            this.keyScheduler[3, index] = newKeyScheduler[3];
        }
        private AesMatrix PrintKeyScheduler(int roundKey)
        {
            Console.WriteLine(String.Format("*****Round Key {0} ******", roundKey));
            var initialKey = ((roundKey + 1) * 4) - 4;
            AesMatrix matrix = new AesMatrix();
            for (int i = 0; i < 4; i++)

            {
                for (int j = initialKey; j < (roundKey + 1) * 4; j++)
                {
                    Console.Write(this.keyScheduler[i, j].ToString("X2") + " ");
                    if ((roundKey + 1) * 4 == 44) {
                        matrix.matrix[i, j - 40] = this.keyScheduler[i, j];
                    }
                }
                Console.WriteLine();
            }
            return matrix;
        }

        private void CreateKeySchedule()
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    this.keyScheduler[i, j] = this.aesMatrix.matrix[i, j];
                }
            }
        }

        private byte[] XORWithPropositionalKeyScheduler(int index, int keyScheduler, byte[] newKeyScheduler)
        {
            var key = (keyScheduler * 4);
            var propositionalKey = new byte[4] {
                this.keyScheduler[0, key + index],
                this.keyScheduler[1, key + index],
                this.keyScheduler[2, key + index],
                this.keyScheduler[3, key + index]
            };
            for (int i = 0; i < 4; i++)
            {
                newKeyScheduler[i] = Convert.ToByte(newKeyScheduler[i] ^ propositionalKey[i]);
            }
            return newKeyScheduler;
        }

        private byte[] XORWithRoundConstant(int roundKey, byte[] newKeyScheduler)
        {
            newKeyScheduler[0] = Convert.ToByte(
                    newKeyScheduler[0] ^ RoundConstant.GetConstant(roundKey)
                );
            return newKeyScheduler;
        }

        private byte[] SubByte(byte[] newKeyScheduler)
        {
            for (var i = 0; i < newKeyScheduler.Length; i++)
            {
                newKeyScheduler[i] = SBox.Replace(newKeyScheduler[i].ToString("X2"));
            }
            return newKeyScheduler;
        }

        private byte[] RotateByte(int roundKey, int indexRound)
        {
            var index = ((roundKey + 1) * 4) - 1 + (indexRound);
            return new byte[4] {
                this.keyScheduler[1, index],
                this.keyScheduler[2, index],
                this.keyScheduler[3, index],
                this.keyScheduler[0, index]
            };
        }
    }
}
