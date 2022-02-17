using System;
using System.Collections.Generic;//позволяет создавать строго типизированные коллекции
using System.Threading;//cоздает и контролирует поток, задает приоритет и возвращает статус.

namespace SnakeProject
{



    public class Snake//класс змейки
    {
        public class Part//часть змейки
        {
            public int x, y, oldx, oldy;//объявление переменных целочисленного типа данных
        }

        public int HeadX, HeadY;//объявление хвоста и головы змейки
        public List<Part> parts = new List<Part>();//Представляет строго типизированный список объектов, доступных по индексу куска/части змейки


    }
    class Program
    {

        public static bool isStarted;//переменная, отвечающая за начало игры
        public static int width = 80, height = 40;//инициализация переменных ширины и высоты
        public static Snake snake;//инициализация переменной змеи
        public enum move { up, down, left, right, stop }//инициализация именнованых констант: вверх, вниз, влево, право, стоп
        public static move dir = move.stop;//инициализация переменных движения
        public static int futX = 0, futY = 0;
        public static void Init()//вставка
        {
            snake = new Snake() { HeadX = width / 2, HeadY = height / 2, parts = new List<Snake.Part>() { new Snake.Part() { x = (width / 2) - 1, y = height / 2, oldx = (width / 2) - 1, oldy = height / 2 } } };//вычисление значения
            Console.CursorVisible = false;//видимость курсора
            isStarted = true;//старт игры
            dir = move.stop;//остановка движения змейки


            for (int i = 0; i < height; i++)//цикл
            {
                for (int j = 0; j < width; j++)
                {
                    Console.SetCursorPosition(j, i);//настройка положения курсора
                    if (j == 0 && i == 0) { Console.Write("╔"); continue; }//оператор выбора/ветвление
                    if (j == width - 1 && i == 0)
                    {
                        Console.Write("╗");
                        continue;//прерывает выполнение текущей итерации текущего или отмеченного цикла, и продолжает его выполнение на следующей итерации.
                    }
                    if (j == 0 && i == height - 1) { Console.Write("╚"); continue; }
                    if (j == width - 1 && i == height - 1)
                    {
                        Console.Write("╝");
                        continue;
                    }

                    if (i == 0 || i == height - 1)
                    {
                        Console.Write("═");
                        continue;
                    }
                    if ((j == 0 || j == width - 1))
                    {
                        Console.Write("║");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }


            SetFruit();//обозначение классов
            Game();
        }

        public static void SetFruit()//фрукт, который ест змейка
        {
            Random rnd = new Random();
            futX = rnd.Next(1, width - 1);
            futY = rnd.Next(1, height - 1);
        }

        public static void Draw()//рисование змейки
        {

            Console.SetCursorPosition(snake.HeadX, snake.HeadY);
            Console.Write("*");
            for (int i = 0; i < snake.parts.Count; i++)
            {
                Console.SetCursorPosition(snake.parts[i].oldx, snake.parts[i].oldy);
                Console.Write(" ");
                Console.SetCursorPosition(snake.parts[i].x, snake.parts[i].y);
                Console.Write("*");
            }
            Console.SetCursorPosition(futX, futY);
            Console.Write("+");
        }

        public static void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (dir != move.down)
                            dir = move.up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (dir != move.up)
                            dir = move.down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (dir != move.right)
                            dir = move.left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (dir != move.left)
                            dir = move.right;
                        break;
                }
            }
        }

        public static void Logic()
        {
            if (dir != move.stop)
            {
                if (snake.parts.FindAll(x => (x.x == snake.HeadX && x.y == snake.HeadY)).Count > 0)
                {
                    isStarted = false;
                }
            }

            int oldX = snake.HeadX, oldY = snake.HeadY;

            if (dir == move.up)
            {
                snake.HeadY--;
            }
            if (dir == move.down)
            {
                snake.HeadY++;
            }
            if (dir == move.left)
            {
                snake.HeadX--;
            }
            if (dir == move.right)
            {
                snake.HeadX++;
            }


            if (dir != move.stop)
            {
                if (snake.HeadX == width || snake.HeadX == 0 || snake.HeadY == 0 || snake.HeadY == height - 1)
                {
                    isStarted = false;
                }
                for (int i = 0; i < snake.parts.Count; i++)
                {
                    if (i == 0) 
                    {
                        snake.parts[i].oldx = snake.parts[i].x;
                        snake.parts[i].oldy = snake.parts[i].y;
                        snake.parts[i].x = oldX;
                        snake.parts[i].y = oldY;
                        continue;  
                    };

                    snake.parts[i].oldx = snake.parts[i].x;
                    snake.parts[i].oldy = snake.parts[i].y;

                    snake.parts[i].x = snake.parts[i - 1].oldx;
                    snake.parts[i].y = snake.parts[i - 1].oldy;
                }
                Console.SetCursorPosition(0, height + 2);
                Console.Write(snake.parts.Count + "    Контроль - Стрелки");
                if (snake.HeadX == futX && snake.HeadY == futY)
                {
                    snake.parts.Add(new Snake.Part() { x = snake.parts[snake.parts.Count - 1].oldx, y = snake.parts[snake.parts.Count - 1].oldy });//добавление новой части змейки
                    SetFruit();
                }
            }
        }
        public static void Game()//обозначение начала и конца игры
        {
            while (isStarted)
            {
                Draw();
                Input();
                Logic();
                Thread.Sleep(66);
            }
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Начать заново? Для повтора нажмите ENTER для выхода ESC");

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Init();
                    Game();
                    break;//применяется для прерывания текущей итерации
                }
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    break;
                    return;
                }
            }


        }
        static void Main(string[] args)//основной метод
        {
            Console.SetWindowSize(width, height + 5);
            Init();
        }
    }
}