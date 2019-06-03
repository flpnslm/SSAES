using System;

namespace WindowsFormsApp1
{
  public class AesMatrix
  {
    public byte[,] matrix { get; set; }
    public AesMatrix() {
      this.matrix = new byte[4, 4];
    }
    public AesMatrix(byte[] bytes)
    {
      if (bytes.Length != 16) {
        throw new System.Exception("Quantidade de bytes inválida.");
      }
      this.matrix = new byte[4, 4];
      for(var i = 0; i < bytes.Length; i++)
      {
        this.matrix[i % 4, i / 4] = bytes[i];
      }
    }
    public void Print()
    {
      for(var i = 0; i < this.matrix.Length; i++)
      {
        var column = i % 4;
        var hex = this.matrix[i / 4, column].ToString("X2");
        if (column == 3)
        {
          Console.WriteLine("0x" + hex);
        } else 
        {
          Console.Write("0x" + $"{hex} ");
        }
      }
      Console.WriteLine();
    }
  }
}