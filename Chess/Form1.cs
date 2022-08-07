using System;
using System.Drawing;
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

        // массив кнопок
        public Button[,] butts = new Button[8, 8];

        // текущий игрок
        public int currentPlayer;

        // предыдущая нажатая кнопка
        public Button prevButton;

        // фигура в движении  
        public bool isMoving = false;

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
            // карта 8х8
            map = new int[8, 8]
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

            currentPlayer = 1;

            // создание карты
            CreateMap();
        }

        // создание карты
        public void CreateMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    butts[i, j] = new Button();

                    Button butt = new Button();
                    // размер кнопки
                    butt.Size = new Size(50, 50);
                    // позиция кнопки
                    butt.Location = new Point(j * 50, i * 50);

                    switch (map[i, j] / 10)
                    {
                        // первый игрок
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            // спрайты фигур
                            g.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part;
                            break;
                        // второй игрок
                        case 2:
                            Image part1 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part1);
                            // спрайты фигур
                            g1.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part1;
                            break;
                    }

                    butt.Click += new EventHandler(OnFigurePress);
                    this.Controls.Add(butt);

                    // в ячейку записываем кнопку butt
                    butts[i, j] = butt;
                }
            }
        }

        // нажатие на фигуру
        public void OnFigurePress(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = Color.Transparent;

            Button pressButton = sender as Button;

            //pressButton.Enabled = false;

            if (map[pressButton.Location.Y / 50, pressButton.Location.X / 50] != 0 && map[pressButton.Location.Y / 50, pressButton.Location.X / 50] / 10 == currentPlayer)
            {
                CloseSteps();
                pressButton.BackColor = Color.Red;
                DeactivateAllButtons();
                pressButton.Enabled = true;

                // показ шагов
                ShowSteps(pressButton.Location.Y / 50, pressButton.Location.X / 50, map[pressButton.Location.Y / 50, pressButton.Location.X / 50]);

                if (isMoving)
                {
                    // закрытие шагов
                    CloseSteps();

                    // поменять цвет                   
                    pressButton.BackColor = Color.Transparent;

                    // активировать кнопку
                    ActivateAllButtons();

                    // конец движение
                    isMoving = false;
                }
                else
                    isMoving = true;
            }
            else
            {
                if (isMoving)
                {
                    int temp = map[pressButton.Location.Y / 50, pressButton.Location.X / 50];
                    map[pressButton.Location.Y / 50, pressButton.Location.X / 50] = map[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                    map[prevButton.Location.Y / 50, prevButton.Location.X / 50] = temp;
                    pressButton.BackgroundImage = prevButton.BackgroundImage;
                    prevButton.BackgroundImage = null;

                    // конец движения
                    isMoving = false;

                    CloseSteps();
                    ActivateAllButtons();
                    SwitchPlayer();
                }
            }

            prevButton = pressButton;
        }

        // показывать шаги для выбранной фигуры
        public void ShowSteps(int IcurrentFigure, int JcurrentFigure, int currentFigure)
        {
            // текущий игрок
            int dir = currentPlayer == 1 ? 1 : -1;

            switch (currentFigure % 10)
            {
                // для пешки
                case 6:
                    if (InsideBorder(IcurrentFigure + 1 * dir, JcurrentFigure))
                    {
                        if (map[IcurrentFigure + 1 * dir, JcurrentFigure] == 0)
                        {
                            // ход фигуры
                            butts[IcurrentFigure + 1 * dir, JcurrentFigure].BackColor = Color.Yellow;
                            butts[IcurrentFigure + 1 * dir, JcurrentFigure].Enabled = true;
                        }
                    }

                    if (InsideBorder(IcurrentFigure + 1 * dir, JcurrentFigure + 1))
                    {
                        if (map[IcurrentFigure + 1 * dir, JcurrentFigure + 1] != 0 && map[IcurrentFigure + 1 * dir, JcurrentFigure + 1] / 10 != currentPlayer)
                        {
                            butts[IcurrentFigure + 1 * dir, JcurrentFigure].BackColor = Color.Yellow;
                            butts[IcurrentFigure + 1 * dir, JcurrentFigure].Enabled = true;
                        }
                    }

                    if (InsideBorder(IcurrentFigure + 1 * dir, JcurrentFigure - 1))
                    {
                        if (map[IcurrentFigure + 1 * dir, JcurrentFigure - 1] != 0 && map[IcurrentFigure + 1 * dir, JcurrentFigure - 1] / 10 != currentPlayer)
                        {
                            butts[IcurrentFigure + 1 * dir, JcurrentFigure - 1].BackColor = Color.Yellow;
                            butts[IcurrentFigure + 1 * dir, JcurrentFigure - 1].Enabled = true;
                        }
                    }
                    break;
                case 5:
                    ShowVerticalHorizontal(IcurrentFigure, JcurrentFigure);
                    break;
                case 3:
                    ShowDiagonal(IcurrentFigure, JcurrentFigure);
                    break;
                case 2:
                    ShowVerticalHorizontal(IcurrentFigure, JcurrentFigure);
                    ShowDiagonal(IcurrentFigure, JcurrentFigure);
                    break;
                case 1:
                    ShowVerticalHorizontal(IcurrentFigure, JcurrentFigure, true);
                    ShowDiagonal(IcurrentFigure, JcurrentFigure, true);
                    break;
                case 4:
                    ShowHorseSteps(IcurrentFigure, JcurrentFigure);
                    break;
            }
        }

        // показывать пути вверх вниз лево право
        public void ShowVerticalHorizontal(int IcurrentFigure, int JcurrentFigure, bool isOneStep = false)
        {
            // показывать все возможные ходы вниз
            for (int i = IcurrentFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, JcurrentFigure))
                {
                    if (!DeterminePath(i, JcurrentFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }

            // показывать все возможные ходы вверх
            for (int i = IcurrentFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, JcurrentFigure))
                {
                    if (!DeterminePath(i, JcurrentFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }

            // показывать все возможные ходы вправо
            for (int j = JcurrentFigure + 1; j < 8; j++)
            {
                if (InsideBorder(IcurrentFigure, j))
                {
                    if (!DeterminePath(IcurrentFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }

            // показывать все возможные ходы влево
            for (int j = JcurrentFigure - 1; j >= 0; j--)
            {
                if (InsideBorder(IcurrentFigure, j))
                {
                    if (!DeterminePath(IcurrentFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
        }

        public void ShowDiagonal(int IcurrentFigure, int JcurrentFigure, bool isOneStep = false)
        {
            // движение вверх
            int j = JcurrentFigure + 1;
            for (int i = IcurrentFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    // показ текущего хода
                    if (!DeterminePath(i, j))
                        break;
                }
                // край карты
                if (j < 7)
                    j++;
                else
                    break;

                if (isOneStep)
                    break;
            }

            // движение вверх и влево
            j = JcurrentFigure - 1;
            for (int i = IcurrentFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                // край карты
                if (j > 0)
                    j--;
                else
                    break;

                if (isOneStep)
                    break;
            }

            // движение вниз и влево
            j = JcurrentFigure - 1;
            for (int i = IcurrentFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else
                    break;

                if (isOneStep)
                    break;
            }

            // движение вниз и вправо
            j = JcurrentFigure + 1;
            for (int i = IcurrentFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }

                if (j < 7)
                    j++;
                else
                    break;

                if (isOneStep)
                    break;
            }
        }

        // ходя для коня
        public void ShowHorseSteps(int IcurrentFigure, int JcurrentFigure)
        {
            // 8 ходов

            // проверяем что находимся на карты
            if (InsideBorder(IcurrentFigure - 2, JcurrentFigure + 1))
            {
                DeterminePath(IcurrentFigure - 2, JcurrentFigure + 1);
            }
            if (InsideBorder(IcurrentFigure - 2, JcurrentFigure - 1))
            {
                DeterminePath(IcurrentFigure - 2, JcurrentFigure - 1);
            }
            if (InsideBorder(IcurrentFigure + 2, JcurrentFigure + 1))
            {
                DeterminePath(IcurrentFigure + 2, JcurrentFigure + 1);
            }
            if (InsideBorder(IcurrentFigure + 2, JcurrentFigure - 1))
            {
                DeterminePath(IcurrentFigure + 2, JcurrentFigure - 1);
            }
            if (InsideBorder(IcurrentFigure - 1, JcurrentFigure + 2))
            {
                DeterminePath(IcurrentFigure - 1, JcurrentFigure + 2);
            }
            if (InsideBorder(IcurrentFigure + 1, JcurrentFigure + 2))
            {
                DeterminePath(IcurrentFigure + 1, JcurrentFigure + 2);
            }
            if (InsideBorder(IcurrentFigure - 1, JcurrentFigure - 2))
            {
                DeterminePath(IcurrentFigure - 1, JcurrentFigure - 2);
            }
            if (InsideBorder(IcurrentFigure + 1, JcurrentFigure - 2))
            {
                DeterminePath(IcurrentFigure + 1, JcurrentFigure - 2);
            }

        }

        // проверка хода фигур
        private bool DeterminePath(int IcurrentFigure, int j)
        {
            // если ход наш то показывать возможные ходы
            if (map[IcurrentFigure, j] == 0)
            {
                butts[IcurrentFigure, j].BackColor = Color.Yellow;
                butts[IcurrentFigure, j].Enabled = true;
            }
            else
            {
                // фигура противника
                if (map[IcurrentFigure, j] / 10 != currentPlayer)
                {
                    butts[IcurrentFigure, j].BackColor = Color.Yellow;
                    butts[IcurrentFigure, j].Enabled = true;
                }
                return false;
            }
            return true;
        }

        // выключить все кнопки
        public void DeactivateAllButtons()
        {
            for (int i = 0; i < butts.GetLength(0); i++)
            {
                for (int j = 0; j < butts.GetLength(1); j++)
                {
                    butts[i, j].Enabled = false;
                }
            }
        }

        // включить все кнопки
        public void ActivateAllButtons()
        {
            for (int i = 0; i < butts.GetLength(0); i++)
            {
                for (int j = 0; j < butts.GetLength(1); j++)
                {
                    butts[i, j].Enabled = true;
                }
            }
        }

        //выбор игрока
        public void SwitchPlayer()
        {
            if (currentPlayer == 1)
                currentPlayer = 2;
            else
                currentPlayer = 1;
        }

        // значение на доске
        public bool InsideBorder(int ti, int tj)
        {
            if (ti >= 8 || tj >= 8 || ti < 0 || tj < 0)
                return false;

            return true;
        }

        // конец шага
        public void CloseSteps()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].BackColor = Color.Transparent;
                }
            }
        }

        // выход
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // кнопка рестарта
        private void button1_Click(object sender, EventArgs e)
        {
            // удалить все кнопки
            this.Controls.Clear();

            // пересоздание карты и кнопок
            Init();
        }
    }
}
