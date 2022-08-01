using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        // хранится изображение фигуры
        public Image chessSprites;

        // карта 8х8
        public int[,] map = new int[8, 8]
        {
            // растановка фигур
            {5,4,3,2,1,3,4,5 }, // 1-ладья, 2-конь, 3-слон, 4-ферзь, 5-король, 
            {6,6,6,6,6,6,6,6 }, // пешки
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {6,6,6,6,6,6,6,6 }, // пешки
            {5,4,3,2,1,3,4,5 } // 1-ладья, 2-конь, 3-слон, 4-ферзь, 5-король, 
        };

        public Form1()
        {
            InitializeComponent();

            // путь
            chessSprites = new Bitmap("D:\\Valera_Doks\\Desktop\\chess.png");
            Image part = new Bitmap(50, 50);
            Graphics g = Graphics.FromImage(part);
            g.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0, 0, 150, 150, GraphicsUnit.Pixel);
            button1.BackgroundImage = part;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
