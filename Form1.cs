using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordBox
{
    public partial class Form1 : Form
    {
        private string[] words;
        private System.Windows.Forms.Label[] wordLabels;
        private const int width = 15;
        private const int height = 15;
        private System.Windows.Forms.TextBox[,] textboxes;
        public Form1()
        {
            InitializeComponent();

            this.textboxes = new TextBox[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var textBox1 = new System.Windows.Forms.TextBox();
                    textBox1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    textBox1.Location = new System.Drawing.Point(10 + (x * 56), 10 + (y * 35));
                    textBox1.Name = $"textBox{x}_{y}";
                    textBox1.Size = new System.Drawing.Size(56, 35);
                    textBox1.CharacterCasing = CharacterCasing.Upper;
                    textBox1.MaxLength = 1;
                    textBox1.TabIndex = 0;
                    textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                    textBox1.KeyUp += TextBox1_KeyUp;
                    this.Controls.Add(textBox1);
                    this.textboxes[x, y] = textBox1;
                }
            }

            this.words = new string[24] { "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta", "Iota", "Kappa", "Lamda", "Mu", "Nu", "Xi", "Omicron", "Pi", "Rho", "Sigma", "Tau", "Upsilon", "Phi", "Chi", "Psi", "Omega" };
            this.wordLabels = new Label[24];
            for (int w = 0; w < words.Length; w++)
            {
                var label1 = new System.Windows.Forms.Label();
                label1.AutoSize = true;
                label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                if (w > 11)
                    label1.Location = new System.Drawing.Point(1025, 9 + (30 * (w - 12)));
                else
                    label1.Location = new System.Drawing.Point(875, 9 + (30 * w));

                label1.Name = $"label_{words[w]}";
                label1.Size = new System.Drawing.Size(67, 30);
                label1.TabIndex = 0;
                label1.Text = words[w];
                this.Controls.Add(label1);
                this.wordLabels[w] = label1;
            }

        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            for (int w = 0; w < words.Length; w++)
            {
                if (WordIsInGrid(words[w]))
                {
                    this.wordLabels[w].Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.wordLabels[w].BackColor = this.BackColor;
                }
                else
                {
                    this.wordLabels[w].Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.wordLabels[w].BackColor = Color.GreenYellow;
                }
            }
        }

        /// <summary>
        /// Walk the grid looking for this word
        /// </summary>
        /// <param name="wordToFind"></param>
        /// <returns></returns>
        private bool WordIsInGrid(string wordToFind)
        {
            // Check each possible start position
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (InGrid(wordToFind.ToUpper(), x, y, new List<string>())) return true;
                }
            }

            // We didn't find it
            return false;
        }

        private bool InGrid(string wordToFind, int x, int y, List<string> used)
        {
            //Does this text box match the start of the word we are looking
            var here = this.textboxes[x, y].Text;
            if (here != wordToFind.Substring(0, 1)) return false;
            used.Add($"{x}_{y}");   //Track the letters we have used

            //We have found it
            if (wordToFind.Length == 1) return true;

            // Can we go up?
            if (y > 0 && !used.Contains($"{x}_{y - 1}"))
            {
                if (InGrid(wordToFind.Substring(1), x, y - 1, used)) return true;
            }

            // Can we go down?
            if (y < (height - 1) && !used.Contains($"{x}_{y + 1}"))
            {
                if (InGrid(wordToFind.Substring(1), x, y + 1, used)) return true;
            }

            // Can we go left?
            if (x > 0 && !used.Contains($"{x - 1}_{y}"))
            {
                if (InGrid(wordToFind.Substring(1), x - 1, y, used)) return true;
            }

            // Can we go right?
            if (x < (width - 1) && !used.Contains($"{x + 1}_{y}"))
            {
                if (InGrid(wordToFind.Substring(1), x + 1, y, used)) return true;
            }

            used.Remove($"{x}_{y}");

            return false;
        }
    }
}
