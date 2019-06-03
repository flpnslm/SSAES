using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
  public class ByteMatrix
  {

    public List<AesMatrix> byteMatrix = new List<AesMatrix>();
    
    public ByteMatrix(byte[] bytes)
    {
      if (bytes.Length > 16) {
        Console.WriteLine("aqui");
        this.add(bytes);
      } else {
        this.byteMatrix.Add(new AesMatrix(this.Preencher(bytes)));
      }
    }

    private void add(byte[] bytes) {
      int loop = 0;
      while(loop < bytes.Length) {
        this.byteMatrix.Add(new AesMatrix(this.getBytes(bytes, loop)));
        loop += 16;
      }
      if (loop != bytes.Length) {
        if (loop < bytes.Length) {
          byte[] bte = new byte[Math.Abs(loop - bytes.Length)];
          int lp = 0;
          while(loop < bytes.Length) {
            bte[lp] = bytes[(bytes.Length - Math.Abs(loop - bytes.Length)) + lp];
          }
          this.byteMatrix.Add(new AesMatrix(this.Preencher(bte)));
        }
      } else {
        this.byteMatrix.Add(new AesMatrix(this.Preencher(new byte[0])));
      }
    }

    private byte[] getBytes(byte[] bytes, int qtd) {
      byte[] bte = new byte[16];
      for(int i = 0; i < 16; i++) {
        bte[i] = bytes.Length > qtd + i ? bytes[qtd + i] : Convert.ToByte(qtd);
      }
      return bte;
    }

    private byte[] Preencher(byte[] bytes) {
      int diff = Math.Abs(bytes.Length - 16);
      byte[] bte = new byte[16];
      for (var i = 0; i < bytes.Length; i++) {
        bte[i] = bytes[i];
      }
      for (var i = 0; i < diff; i++) {
        bte[i] = Convert.ToByte(diff);
      }
      return bte;
    }
    
  }
}