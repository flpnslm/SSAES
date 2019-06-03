using System;
using System.IO;

namespace WindowsFormsApp1
{
    public class SimpleXor
    {
        private AesMatrix key;
        private AesMatrix simpleText;
        public SimpleXor(AesMatrix key, AesMatrix simpleText)
        {
            this.key = key;
            this.simpleText = simpleText;
        }

        public AesMatrix GetSimpleXOR()
        {
            var simpleXor = new AesMatrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    simpleXor.matrix[i, j] = Convert.ToByte(
                        this.key.matrix[i, j] ^ this.simpleText.matrix[i, j]
                    );
                }
            }
            return simpleXor;
        }
    }
}
