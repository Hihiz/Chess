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
            // растановка фигур      1 игрок
            {15,14,13,12,11,13,14,15 }, // 1-ладья, 2-конь, 3-слон, 4-ферзь, 5-король, 
            {16,16,16,16,16,16,16,16 }, // пешки
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },         // 2 игрок
            {26,26,26,26,26,26,26,26 }, // пешки
            {25,24,23,22,21,23,24,25 } // 1-ладья, 2-конь, 3-слон, 4-ферзь, 5-король, 
        };

        public Form1()
        {
            InitializeComponent();

            // путь
            chessSprites = new Bitmap("D:\\Valera_Doks\\Desktop\\chess.png");
            // button1.BackgroundImage = part;

            Init();
        }

        public void Init()
        {
            CreateMap();
        }

        // создание карты
        public void CreateMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Button button = new Button();
                    // размер кнопки
                    button.Size = new Size(50, 50);
                    // позиция кнопки
                    button.Location = new Point(j * 50, i * 50);

                    switch (map[i, j] / 10)
                    {
                        // первый игрок
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            // спрайты фигур
                            g.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * map[i, j] % 150, 0, 150, 150, GraphicsUnit.Pixel);
                            button.BackgroundImage = part;
                            break;
                        // второй игрок
                        case 2:
                            Image part1 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part1);
                            // спрайты фигур
                            g1.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * map[i, j] % 150, 150, 150, 150, GraphicsUnit.Pixel);
                            button.BackgroundImage = part1;
                            break;
                    }

                    this.Controls.Add(button);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
