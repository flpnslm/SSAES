using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            this.textBox1.SelectedText = ofd.FileName;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            this.textBox2.SelectedText = ofd.FileName;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var simpleTextBytes = File.ReadAllBytes(this.textBox1.Text);
            Console.WriteLine("Lendo texto simples da pasta " + this.textBox1.SelectedText);
            var keyBytes = File.ReadAllBytes(this.textBox2.Text);
            Console.WriteLine("Lendo chave da pasta " + this.textBox1.SelectedText);

            var key = new AesMatrix(keyBytes);
            var simpleTextMatrix = new ByteMatrix(simpleTextBytes);
            Console.WriteLine("teste");
            Console.WriteLine(simpleTextMatrix.byteMatrix.Count);
            byte[] bte = new byte[simpleTextMatrix.byteMatrix.Count * 16];
            var loop = 1;
            foreach (var simpleText in simpleTextMatrix.byteMatrix)
            {

                Console.WriteLine("****Chave****");
                key.Print();
                Console.WriteLine("****Texto simples****");
                simpleText.Print();
                var keyScheduler = new RoundKey(key).getAesMatrixCifred();
                var simpleXor = new SimpleXor(key, simpleText).GetSimpleXOR();
                var cifred = new MatrixRoundKey(simpleXor).GetRounds(keyScheduler);
                Console.WriteLine("****Texto cifrado****");
                cifred.Print();
                for (var i = 0; i < 4; i++)
                {
                    for (int b = 0; b < 4; b++)
                    {
                        bte[b * loop] = cifred.matrix[b, i];
                    }
                }
                loop++;
            }
            File.Create(this.textBox3.Text);
            File.WriteAllBytes(this.textBox3.Text, bte);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Texto cifrado salvo na pasta " + this.textBox3.Text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "txt";
            sfd.ShowDialog();
            this.textBox3.SelectedText = sfd.FileName;
        }
    }
}
