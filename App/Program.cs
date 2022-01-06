using System;
using System.IO;
using System.Linq;

namespace App
{
    // Программа "Калькулятор Матриц". Класс разделен на 2 блока: первый для запуска соответсвующих функций
    // калькулятора, а второй для вспомогательных методов(обработки ввода, показа матриц и тд).
    partial class Program
    {
        // Критерий возврата в стартовое меню в любой момент времени(когда можно что-то ввести).
        static bool stop = false;
        /// <summary>
        /// Точка входа, метод для отрисовки ограничений и предоставляемого функционала.
        /// </summary>
        static void Main()
        {
            Console.Write("Это калькулятор матриц. Всё крайне просто: у него есть несколько операций " +
                "(они описаны ниже), которые он может \nвыполнять некоторые условия описаны отдельно в каждой" +
                " операции. Есть общие правила: элементы матрицы - числа\nот -100 до 100, размеры матриц" +
                " - натуральные числа [1:10]. Размеры и элементы можно выбирать случайным образом только\nпри вводе" +
                " через консоль, написав \"rnd\" вместо числа. Остановить всё можно в любой момент, написав " +
                "\"stop\".\nНачнём! Выберите то, что вы хотите сделать, нажав одну из 9 клавиш:\n\n");
            do
            {
                Console.WriteLine("Press \"1\" to find trace (нахождения следа матрицы)\nPress \"2\" to transporate " +
                    "the matrix (транспонировать)\nPress \"3\" to find sum of two matrices (сумма двух матриц)\nPress" +
                    " \"4\" to find the difference of two matrices (вычесть одну матрицу из другой)\nPress \"5\" to" +
                    " multiply two matrices (перемножить две матрицы)\nPress \"6\" to multiply the matrix by a number" +
                    "(умножить матрицу на число) \nPress \"7\" to find the determinator of the matrix (найти" +
                    " определитель матрицы)\nPress \"8\" to solve the system (решить СЛАУ)\nPress \"9\" to exit " +
                    "(выйти из калькулятора)");
                // Список клавиш, нажав на которые будет запущен какая-то функция калькулятора. 
                ConsoleKey[] vse = { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4,
                ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9};
                ConsoleKey click = Console.ReadKey(true).Key;
                while (!vse.Contains(click))
                {
                    Console.WriteLine("Вы нажали кнопку, но ей не присвоено никакое действие программы.");
                    Console.WriteLine("Попробуйте еще раз!");
                    click = Console.ReadKey(true).Key;
                }
                Console.Clear();
                if (!UserClickToChoose(click))
                    return;
                stop = false;
            } while (true);
        }

        /// <summary>
        /// Метод обработки нажатия пользователя, запуск желаемой операции.
        /// </summary>
        /// <param name="button">Кнопка, которую нажал пользователь.</param>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool UserClickToChoose(ConsoleKey button)
        {
            switch (button)
            {
                case ConsoleKey.D1:
                    return Trace();
                case ConsoleKey.D2:
                    return MatrixTransposition();
                case ConsoleKey.D3:
                    return MatrixAddition();
                case ConsoleKey.D4:
                    return MatrixDifference();
                case ConsoleKey.D5:
                    return MatrixMultiplication();
                case ConsoleKey.D6:
                    return MultiplyByTheNumber();
                case ConsoleKey.D7:
                    return Determinant();
                case ConsoleKey.D8:
                    return SolveTheSystem();
                case ConsoleKey.D9:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Метода, выполняющий запрос на нахождения разности 2 матриц.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool MatrixDifference()
        {
            Console.WriteLine("Для того, чтобы вычесть одну матрицу из другой матрицы, они должны быть" +
                " одного размера, поэтому\nвозможность выбора размеров будет только в первой матрице.\n");
            Console.WriteLine("Введите первую матрицу");
            decimal[,] matrix1 = InputMatrix();
            if (stop)
                return WhatIsNext();
            Console.WriteLine("Введите вторую матрицу");
            // Матрицы должны быть одного размера, поэтому вторая матрица создаётся с фиксированными размерами.
            decimal[,] matrix2 = InputMatrix(rows: (uint)matrix1.GetLength(0), cols: (uint)matrix1.GetLength(1));
            if (stop)
                return WhatIsNext();
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    matrix1[i, j] -= matrix2[i, j];
                }
            }
            Console.WriteLine("Результат:");
            DemonstrateTheMatrix(matrix1);
            return WhatIsNext();
        }

        /// <summary>
        /// Метода для нахождения следа матрицы.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool Trace()
        {
            Console.WriteLine("Чтобы посчитать след матрицы вам понадобиться ввести саму матрицу, предупреждение" +
                " элементы матрицы\nмогут быть от -100 до 100.\n");
            decimal[,] matrix = InputMatrix();
            decimal sumaTrace = 0m;
            if (stop)
                return WhatIsNext();
            for (int i = 0; i < Math.Min(matrix.GetLength(0), matrix.GetLength(1)); i++)
            {
                sumaTrace += matrix[i, i];
            }
            Console.WriteLine($"Trace = {sumaTrace}");
            return WhatIsNext();
        }

        /// <summary>
        /// Метода для нахождения определителя матрицы.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool Determinant()
        {
            Console.WriteLine("Только у квадратных матриц можно найти определитель, " +
                "поэтому при вводе из файла количество строк должно равняться\nколичеству солбцов," +
                "а через консоль значению количества солбцов будет присвоено количество строк.\n");
            Console.WriteLine("Введите первую матрицу");
            decimal[,] matrix1 = InputMatrix(square: true);
            if (stop)
                return WhatIsNext();
            decimal result = 1;
            // Приведение матрицы к ступенчатому виду, чтобы определитель
            // равнялся произведению элементов на гл. диагонали.
            matrix1 = GaussStepView(matrix1, out result);
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                result *= matrix1[i, i];
            }
            result = Math.Round(result, 3);
            Console.WriteLine($"Результат: {result}");
            return WhatIsNext();
        }

        /// <summary>
        /// Метод для решение СЛАУ любого размера, но численный ответ будет только при единственности решении.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool SolveTheSystem()
        {
            Console.WriteLine("Эта функция решит систему. Возможные ответы: корень уравнения, если он один, беск." +
                " количество решений или их отсутствие.\n");
            Console.WriteLine("Введите матрицу");
            decimal[,] matrix = InputMatrix();
            if (stop)
            {
                return WhatIsNext();
            }
            decimal[,] stepMatrix = GaussStepView(matrix, out decimal sgn);
            int m = stepMatrix.GetLength(1); int n = stepMatrix.GetLength(0);
            // Если есть строка где все 0, кроме последнего столбца.
            for (int i = 0; i < n; i++)
            {
                decimal rowSumma = 0;
                for (int j = 0; j < m; j++)
                {
                    stepMatrix[i, j] = Math.Round(stepMatrix[i, j], 3);
                    rowSumma += stepMatrix[i, j];
                }
                if ((rowSumma == stepMatrix[i, m - 1]) && stepMatrix[i, m - 1] != 0)
                {
                    Console.WriteLine("Нет решений.");
                    return WhatIsNext();
                }
            }
            // Проверка наличия свободных переменных.
            for (int i = 0; i < (m - 1); i++)
            {
                if (i >= n || stepMatrix[i, i] == 0)
                {
                    Console.WriteLine("Бесконечно много решений.");
                    return WhatIsNext();
                }
            }
            decimal[] answer = GaussImprovedStepView(stepMatrix);
            for (int i = 0; i < m - 1; i++)
                Console.Write($"x{i + 1}: {answer[i]} ");
            Console.WriteLine();
            return WhatIsNext();
        }

        /// <summary>
        /// Метод, который транспонирует введенную матрицу и показывает её на экран.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool MatrixTransposition()
        {
            Console.WriteLine("Эта функция меняет строки и столбцы в введенной вами матрице.\n");
            decimal[,] matrix = InputMatrix();
            if (stop)
            {
                return WhatIsNext();
            }
            decimal[,] result = new decimal[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }
            Console.WriteLine("Транспонированная матрица:");
            DemonstrateTheMatrix(result);
            return WhatIsNext();
        }

        /// <summary>
        /// Матод для умножения матрицы на число.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool MultiplyByTheNumber()
        {
            Console.WriteLine("Эта функция умножает матрицу на введенное число.\n");
            InputNumber(out decimal number);
            if (stop)
                return WhatIsNext();
            decimal[,] matrix = InputMatrix();
            if (stop)
                return WhatIsNext();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = Math.Round(matrix[i, j] * number, 3);
                }
            }
            Console.WriteLine($"Результат умножения матрицы на {number}");
            DemonstrateTheMatrix(matrix);
            return WhatIsNext();
        }

        /// <summary>
        /// Метод для перемножения двух матриц matrix1*matrix2. Matrix1 m x n matrix2 n x k.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool MatrixMultiplication()
        {
            Console.WriteLine("Для того, чтобы перемножить одну матрицу на другую, нужно, чтобы" +
                " количество столбцов первой равнялось\nколичеству строк второй.\n");
            Console.WriteLine("Введите первую матрицу");
            decimal[,] matrix1 = InputMatrix();
            if (stop)
                return WhatIsNext();
            Console.WriteLine("Введите вторую матрицу");
            decimal[,] matrix2 = InputMatrix(rows: (uint)matrix1.GetLength(1));
            if (stop)
                return WhatIsNext();
            decimal[,] result = new decimal[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    // Специальный метод для подсчёта скалярного произведения для элемента [i,j].
                    result[i, j] = Math.Round(SumIJ(matrix1, matrix2, i, j));
                }
            }
            Console.WriteLine("Результат:");
            DemonstrateTheMatrix(result);
            return WhatIsNext();
        }

        /// <summary>
        /// Метод для сложения двух матриц.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool MatrixAddition()
        {
            Console.WriteLine("Для того, чтобы сложить матрицы, они должны быть одного размера, поэтому возможность" +
                " выбора размеров\nбудет только в первой матрице.\n");
            Console.WriteLine("Введите первую матрицу");
            decimal[,] matrix1 = InputMatrix();
            if (stop)
                return WhatIsNext();
            Console.WriteLine("Введите вторую матрицу");
            decimal[,] matrix2 = InputMatrix(rows: (uint)matrix1.GetLength(0), cols: (uint)matrix1.GetLength(1));
            if (stop)
                return WhatIsNext();
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    matrix1[i, j] += matrix2[i, j];
                }
            }
            Console.WriteLine("Результат:");
            DemonstrateTheMatrix(matrix1);
            return WhatIsNext();
        }
    }
    partial class Program
    {
        /// <summary>
        /// Метода для ввода матрицы из файла.
        /// </summary>
        /// <param name="rows">Кол-во строк, если они заданы заранее.</param>
        /// <param name="cols">Кол-во столбцов, если они заданы заранне.</param>
        /// <param name="square">Условие, что матрица должна быть квадратной.</param>
        /// <returns>Введенную матрицу.</returns>
        private static decimal[,] FileInputMatrix(uint rows = 0, uint cols = 0, bool square = false)
        {
            string fileName; decimal[,] matrix = new decimal[rows, cols]; StreamReader readFromFile;
            do
            {
                try
                {
                    Console.WriteLine("Укажите ПОЛНЫЙ путь к файлу с разрешением *.txt.");
                    fileName = Console.ReadLine();
                    stop = stop ? stop : fileName == "stop";
                    readFromFile = new StreamReader(fileName);
                    // Показ стандарта файла, то есть как внутри оформлен ввод.
                    ShowStandartOfFile(rows, cols, square);
                    uint n, m, rowToAdd = 0; string line = readFromFile.ReadLine(); ; string[] inputLine;
                    inputLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    stop = !stop ? inputLine.Contains("stop") : stop;
                    //Правильность ввода строк и столбцов.
                    if ((!uint.TryParse(inputLine[0], out n)) || (!uint.TryParse(inputLine[1], out m)) || m > 10 ||
                        m < 1 || n > 10 || n < 1 || stop || (square && n != m))
                        throw new Exception();
                    rows = rows > 0 ? rows : n; cols = cols > 0 ? cols : m;
                    matrix = new decimal[rows, cols]; line = readFromFile.ReadLine();
                    // Построчный ввод матрицы.
                    while (line != null)
                    {
                        inputLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        stop = !stop ? inputLine.Contains("stop") : stop;
                        if (stop)
                            return matrix;
                        if (!AddLineInMarix(inputLine, rowToAdd, ref matrix))
                            throw new Exception();
                        line = readFromFile.ReadLine();
                        rowToAdd++;
                    }
                    break;
                    readFromFile.Close();
                }
                catch (Exception)
                {
                    if (stop)
                        return matrix;
                    Console.WriteLine("Вы указали что-то не так в названии файла или внутри него. Попробуйте снова!\n");
                }
            } while (true);
            return matrix;
        }

        /// <summary>
        /// Метод для того, чтобы пользователю показать, как должен выглядеть файл внутри.
        /// </summary>
        /// <param name="rows">Количество строк, если они определены.</param>
        /// <param name="cols">Количество столбцов, если они определены.</param>
        private static void ShowStandartOfFile(uint rows, uint cols, bool square)
        {
            Console.WriteLine("Текст в файле должен выглядеть так:\n\nn m\nA11 A12 ... A1m\nA21 A22 ... A2m\n." +
                "..............\nAn1 An2 ... Anm\n\nВ первой строке через пробел ввод 2 натуральных чисела <=10:" +
                " количество строк и столбцов в матрице, Далее в каждой\nотдельной строке вводятся n строк, каждая" +
                " должная состоять из m элементов, разделённых пробелами -100 <= Aij <= 100.\n");
            cols = square ? rows : cols;
            if (rows > 0)
                Console.WriteLine($"Для выполнения вашей задачи количество строк может быть только {rows}," +
                    $" даже при вводе друго значения строк.\n");
            if (cols > 0)
                Console.WriteLine($"Для выполнения вашей задачи количество столбцов может быть только {cols}," +
                    $"даже при вводе друго значения столбцов.\n");
            if (square)
                Console.WriteLine("Матрица должна быть квадратной!\n");
        }

        /// <summary>
        /// Метод для ввода строки inputLine на место row в matrix.
        /// </summary>
        /// <param name="inputLine">Добавляемая строка.</param>
        /// <param name="row">Индекс строки,куда нужно добавить.</param>
        /// <param name="matrix">Матрицы, в которую добавляем.</param>
        /// <returns>true - все данные были корректны, false - иначе.</returns>
        private static bool AddLineInMarix(string[] inputLine, uint row, ref decimal[,] matrix)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (j >= inputLine.Length || !int.TryParse(inputLine[j], out _))
                    return false;
                int.TryParse(inputLine[j], out int newElement);
                matrix[row, j] = newElement;
            }
            return true;
        }

        /// <summary>
        /// Метод для выбора способа ввода матрицы. Вызывает методы для ввода через консоль и из файла.
        /// Если параметры rows или cols больше 0 => они фиксированы.
        /// </summary>
        /// <param name="rows">Количество строк, если у матрицы должно быть определенное количество строк.</param>
        /// <param name="cols">Количество столбцов,если они должны быть конкретного значения.</param>
        /// <param name="square">Параметр, если нужно, чтобы матрица обязательно была квадратной.</param>
        /// <returns>Двумерный массив(матрицу).</returns>
        private static decimal[,] InputMatrix(uint rows = 0, uint cols = 0, bool square = false)
        {
            string list;
            Console.WriteLine("Введите слово \"file\", если хотите ввести матрицу из файла и \"console\", если" +
                " ввести через консоль\n(Ввести случайное значения можно будет только через консоль)");
            list = Console.ReadLine();
            while ((list != "file") && (list != "console") && (list != "stop"))
            {
                Console.WriteLine("Не знаю такой команды. Попробуйте снова!");
                list = Console.ReadLine();
            }
            if (list == "console")
            {
                return ConsoleInputMatrix(rows, cols, square);
            }
            else if (list == "stop" || stop)
            {
                stop = true;
                decimal[,] exit = { };
                return exit;
            }
            return FileInputMatrix(rows, cols);
        }

        /// <summary>
        /// Ввод матрицы через консоль.Если параметры rows или cols больше 0 => они фиксированы.
        /// </summary>
        /// <param name="rows">Количество строк, если у матрицы должно быть определенное количество строк.</param>
        /// <param name="cols">Количество столбцов,если они должны быть конкретного значения.</param>
        /// <param name="square">Параметр, если нужно, чтобы матрица обязательно была квадратной.</param>
        /// <returns>Двумерный массив(матрицу).</returns>
        private static decimal[,] ConsoleInputMatrix(uint rows = 0, uint cols = 0, bool square = false)
        {
            var generateElement = new Random(); Tuple<uint, uint> size = HandleUserInputSize(out bool rnd, rows, cols, square);
            rows = size.Item1; cols = size.Item2; decimal[,] matrix = new decimal[rows, cols];
            if (rnd)
            {
                Console.WriteLine($"Матрица {rows} на {cols}:");
                return RandomGeneratedmatrix(rows, cols, matrix);
            }
            if (stop)
                return matrix;
            Console.WriteLine($"Матрица {rows} на {cols}, введите матрицу построчно, разделяя элементы пробелом.");
            for (int i = 1; i < rows + 1; i++)
            {
                Console.Write($"{i}: ");
                string list = Console.ReadLine();
                string[] elements = list.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (elements.Contains("stop") || stop)
                {
                    Array.Clear(matrix, 0, matrix.Length); stop = true; return matrix;
                }
                if (elements.Length < cols || elements.Length > cols)
                {
                    Console.WriteLine($"Неправильное количество элементов в строке. Их должно быть {cols}");
                    i--;
                }
                else
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (decimal.TryParse(elements[j], out decimal neww))
                        {
                            matrix[i - 1, j] = Math.Round(neww, 4);
                        }
                        else if (elements[j] == "rnd")
                        {
                            matrix[i - 1, j] = generateElement.Next(-10, 11);
                        }
                        else
                        {
                            Console.WriteLine("Какой-то введенный элемент не соответсвует" +
                                " допустимым входным данным, введите строку заново");
                            Array.Clear(matrix, (i - 1) * (int)cols, j + 1);
                            i--;
                            break;
                        }
                    }
                }
            }
            return matrix;
        }

        /// <summary>
        /// Метод, с помощью которого пользователь задёт размеры матрицы или узнаёт, что они фиксированы
        /// в определенных случаях. Если параметры rows или cols больше 0 => они фиксированы.
        /// </summary>
        /// <param name="rows">Количество строк, если у матрицы должно быть определенное количество строк.</param>
        /// <param name="cols">Количество столбцов,если они должны быть конкретного значения.</param>
        /// <param name="square">Параметр, если нужно, чтобы матрица обязательно была квадратной.</param>
        /// <returns>Двумерный массив(матрицу).</returns>
        private static Tuple<uint, uint> HandleUserInputSize(out bool rnd, uint rows, uint cols, bool square = false)
        {
            string list;
            switch (rows, cols, square)
            {
                case (0, 0, false):
                    if (InputRows(out rows) || InputCols(out cols))
                    {
                        rnd = false; return new Tuple<uint, uint>(0, 0);
                    }
                    break;
                case (0, 0, true):
                    if (InputRows(out rows))
                    {
                        rnd = false; return new Tuple<uint, uint>(0, 0);
                    }
                    cols = rows;
                    Console.WriteLine($"Для выполнения вашей задачи количество столбцов может быть только {rows}");
                    break;
                case ( > 0, 0, false):
                    Console.WriteLine($"Для выполнения вашей задачи количество строк может быть только {rows}");
                    if (InputCols(out cols))
                    {
                        rnd = false; return new Tuple<uint, uint>(0, 0);
                    }
                    break;
                case ( > 0, > 0, false):
                    Console.WriteLine($"Для выполнения вашей задачи количество строк может быть только {rows}");
                    Console.WriteLine($"Для выполнения вашей задачи количество столбцов может быть только {cols}");
                    break;
            }
            do
            {
                Console.WriteLine($"Если вы хотите, чтобы каждый элемент матрицы был создан случайным образом," +
                    $" введите \"rnd\" иначе пустую строку.\nРазмеры матрицы: {rows} на {cols}");
                list = Console.ReadLine();
            } while (list != "rnd" && list != "" && list != "stop");
            stop = list == "stop";
            rows = stop ? 0 : rows;
            cols = stop ? 0 : cols;
            rnd = (list == "rnd");
            return Tuple.Create(rows, cols);
        }

        /// <summary>
        /// Ввод количества сторок в матрице.
        /// </summary>
        /// <param name="rows">Количество строк, если у матрицы должно быть определенное количество строк.</param>
        /// <returns>true, если ввели stop иначе false.</returns>
        private static bool InputRows(out uint rows)
        {
            string list;
            var randomChioce = new Random();
            // Ввод до тех пор пока не будет введено слово stop или натуральное число от 1 д 10
            do
            {
                Console.Write("Введите количество строк матрицы (число от 1 до 10):");
                list = Console.ReadLine();
            } while ((list != "rnd") && (!uint.TryParse(list, out rows)) && (list != "stop")
            && (uint.TryParse(list, out rows) && (rows > 10) || (rows < 1)));
            switch (list)
            {
                case "rnd":
                    rows = (uint)randomChioce.Next(1, 11);
                    break;
                case "stop":
                    rows = 0;
                    stop = true;
                    break;
                default:
                    _ = uint.TryParse(list, out rows);
                    break;
            }
            return list == "stop";
        }

        /// <summary>
        /// Метод для ввода количества столбцов в матрице через консоль.
        /// </summary>
        /// <param name="cols">Количество столбцов, если они должны быть конкретного значения.</param>
        /// <returns>true, если ввели stop иначе false.</returns>
        private static bool InputCols(out uint cols)
        {
            string list;
            var randomChioce = new Random();
            // Ввод до тех пор пока не будет введено слово stop или натуральное число от 1 д 10
            do
            {
                Console.Write("Введите количество столбцов матрицы (число от 1 до 10):");
                list = Console.ReadLine();
            } while (list != "rnd" && !uint.TryParse(list, out cols) && list != "stop"
            && (uint.TryParse(list, out cols) && (cols > 10) || (cols < 1)));
            switch (list)
            {
                case "rnd":
                    cols = (uint)randomChioce.Next(1, 11);
                    break;
                case "stop":
                    cols = 0;
                    stop = true;
                    break;
                default:
                    _ = uint.TryParse(list, out cols);
                    break;
            }
            return list == "stop";
        }

        /// <summary>
        /// Метод для вывода матрицы в консоль. Выбрано примерное количество места для элементов.
        /// </summary>
        /// <param name="matrix">Двумерный массив, который необходимо вывести.</param>
        private static void DemonstrateTheMatrix(decimal[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++, Console.WriteLine(""))
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0,8} ", Math.Round(matrix[i, j], 3));
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Скалярное произведение iой строки матрицы matrix1 и jого столбца матрицы matrix2.
        /// </summary>
        /// <param name="matrix1">Двумерный массив первой матрицы.</param>
        /// <param name="matrix2">Двумерный массив второй матрицы.</param>
        /// <param name="i">Iая строка.</param>
        /// <param name="j">Jий столбец.</param>
        /// <returns>Скалярное произведение.</returns>
        private static decimal SumIJ(decimal[,] matrix1, decimal[,] matrix2, int i, int j)
        {
            decimal suma = 0;
            for (int k = 0; k < matrix1.GetLength(1); k++)
                suma += matrix1[i, k] * matrix2[k, j];
            return suma;
        }

        /// <summary>
        /// Генератор матрицы с rows строками и cols столбцами и cо случайными элементами.
        /// </summary>
        /// <param name="rows">Количество строк, если у матрицы должно быть определенное количество строк.</param>
        /// <param name="cols">Количество столбцов, если они должны быть конкретного значения.</param>
        /// <returns>Двумерный массив(матрица).</returns>
        private static decimal[,] RandomGeneratedmatrix(uint rows, uint cols, decimal[,] matrix)
        {
            var generateElement = new Random();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = generateElement.Next(-100, 101);
            DemonstrateTheMatrix(matrix);
            return matrix;
        }

        /// <summary>
        /// Метод для ввода числа, при выполнении функции перемножения матрицы на число.
        /// </summary>
        /// <param name="number">Переменная, в которую будет записано введенное число.</param>
        private static void InputNumber(out decimal number)
        {
            string list;
            do
            {
                Console.WriteLine("Введите вещественное число, на которое будет умножена матрица:");
                list = Console.ReadLine();
            } while (!decimal.TryParse(list, out number) && (list != "stop"));
            stop = (list == "stop");
        }

        /// <summary>
        /// Возможность закрыть программу или вернуться в меню после введения команды stop или после
        /// выполнения операции над матрицей, метод вызывается после каждой операции над матрицей.
        /// </summary>
        /// <returns>true, если пользователь хочет вернуться в меню и false, чтобы выйти из приложения.</returns>
        private static bool WhatIsNext()
        {
            ConsoleKey click;
            do
            {
                Console.WriteLine("Чтобы вернуться в меню нажмите \"Enter\", чтобы выйти \"Escape\"");
                click = Console.ReadKey(true).Key;
            } while (click != ConsoleKey.Enter && click != ConsoleKey.Escape);
            if (click == ConsoleKey.Enter)
            {
                Console.Clear();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Приведение матрицы к ступенчатому виду.
        /// </summary>
        /// <param name="matrix">Двумерный массив, над которым выполняются операции.</param>
        /// <param name="sgn">Знак определителя.</param>
        /// <returns>Приведенную матрицу(двумерный массив).</returns>
        private static decimal[,] GaussStepView(decimal[,] matrix, out decimal sgn)
        {
            //sgn - знак определителя, stepI - строка, на которой появится следующая ступенька.
            sgn = 1; int stepI = 0;
            int n = matrix.GetLength(0); int m = matrix.GetLength(1); decimal[,] matrixClone = new decimal[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrixClone[i, j] = matrix[i, j];
            for (int stepJ = 0; stepJ < m - 1; stepJ++)
            {
                // Если на главной диагонали появляется ноль.
                if (matrixClone[stepI, stepJ] == 0)
                    ChangeRows(matrixClone, stepI, stepJ, ref sgn);
                if (matrixClone[stepI, stepJ] != 0)
                {
                    for (int i = stepI + 1; i < n; i++)
                    {
                        //Определение коэффициента, на который домножается sepI строчка
                        //и прибавляется ко всем строчкам ниже.
                        decimal coefficient = matrixClone[i, stepJ] / matrixClone[stepI, stepJ];
                        for (int j = stepJ; j < m; j++)
                            matrixClone[i, j] = matrixClone[i, j] - matrixClone[stepI, j] * coefficient;
                    }
                    stepI++;
                }
                if (stepI >= n)
                    break;
            }
            return matrixClone;
        }

        /// <
        /// summary>
        /// Метод приведения матрицы к улучшенному ступенчатому виду.
        /// </summary>
        /// <param name="matrix">Изменяемая матрица.</param>
        /// <returns>Приведенная матрица</returns>
        private static decimal[] GaussImprovedStepView(decimal[,] matrix)
        {
            int n = matrix.GetLength(0); int m = matrix.GetLength(1);
            for (int k = n - 1; k > -1; k--)
            {
                //Только главные переменные нужны.
                if (matrix[k, k] != 0)
                {
                    for (int i = m - 1; i > -1; i--)
                        matrix[k, i] = matrix[k, i] / matrix[k, k];
                    for (int i = k - 1; i > -1; i--)
                    {
                        //Коэффициент, на который домножается элемент, чтобы вычесть его из вышестоящих.
                        decimal coefficient = matrix[i, k] / matrix[k, k];
                        for (int j = m - 1; j > -1; j--)
                            matrix[i, j] = matrix[i, j] - matrix[k, j] * coefficient;
                    }
                }
            }
            decimal[] answer = new decimal[n];
            for (int i = 0; i < n; i++)
                answer[i] = matrix[i, m - 1];
            return answer;
            //return matrix;
        }

        /// <summary>
        /// Метод для замены строки row, у которой на в столбце col стоит 0, на строку,
        /// у которой в столце col стоит не 0, если такая есть.
        /// </summary>
        /// <param name="matrix">Двумерный массив(матрица), который меняют.</param>
        /// <param name="row">Номер строки, у которой 0 в столбце col.</param>
        /// <param name="col">Номер столбца, у которого 0 в строке row.</param>
        /// <param name="sgn">Знак определителя.</param>
        /// <returns></returns>
        private static decimal[,] ChangeRows(decimal[,] matrix, int row, int col, ref decimal sgn)
        {
            decimal[] newRow = new decimal[matrix.GetLength(1)];
            for (int i = col + 1; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, col] != 0)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        newRow[j] = matrix[i, j];
                        matrix[i, j] = matrix[row, j];
                    }
                    break;
                }
            }
            // Знак меняется только, если происходит перестановка строк.
            if (newRow[col] != 0)
            {
                sgn *= -1;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[row, j] = newRow[j];
                }
            }
            return matrix;
        }
    }
}
