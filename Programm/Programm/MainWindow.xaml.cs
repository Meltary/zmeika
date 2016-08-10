using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Programm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle grib = new Rectangle();//гриб
        List<Rectangle> zmeas = new List<Rectangle>();//змея
        List<Point> tochka = new List<Point>();//положение змеи
        List<Point> steps = new List<Point>();//направление шагов змеи
        String step = "right";
        int numzme = 0;//кол-во ячеек в змее
        Random rand = new Random();
        int shet = 0;//счет

        public MainWindow()
        {
            InitializeComponent();
        }

        public async void Go()
        {
            //пока не врежется в стену
            while (Canvas.GetLeft(zmeas[0]) >= 0 && Canvas.GetLeft(zmeas[0]) <= 380 && Canvas.GetTop(zmeas[0]) >= 0 && Canvas.GetTop(zmeas[0]) <= 360)
            {
                //если врезалась в саму себя
                for (int k = 0; k < tochka.Count; k++)
                    for (int t = 0; t < tochka.Count; t++)
                    {
                        if (tochka[k] == tochka[t] && k != t)
                            break;
                    }

                //если врезалась в гриб
                if (Canvas.GetLeft(zmeas[0]) == Canvas.GetLeft(grib) && Canvas.GetTop(zmeas[0]) == Canvas.GetTop(grib))
                {
                    numzme++;

                    //новый гриб
                    Canvas.SetLeft(grib, (rand.Next(0, 380) / 20) * 20);
                    Canvas.SetTop(grib, (rand.Next(0, 360) / 20) * 20);

                    //добавление новой ячейки в змею
                    steps.Add(new Point(steps[steps.Count - 1].X, steps[steps.Count - 1].Y));
                    tochka.Add(new Point(tochka[tochka.Count - 1].X, tochka[tochka.Count - 1].Y));
                    zmeas.Add(new Rectangle());
                    zmeas[numzme].Fill = Brushes.Black;
                    zmeas[numzme].Width = 20;
                    zmeas[numzme].Height = 20;
                    canv.Children.Add(zmeas[numzme]);
                    Canvas.SetLeft(zmeas[numzme], tochka[numzme].X);
                    Canvas.SetTop(zmeas[numzme], tochka[numzme].Y);

                    shet += 1;
                }

                //движение змеи
                for (int j = numzme; j >= 1; j--)
                {
                    tochka[j] = new Point() { X = tochka[j - 1].X, Y = tochka[j - 1].Y };
                    Canvas.SetLeft(zmeas[j], tochka[j].X);
                    Canvas.SetTop(zmeas[j], tochka[j].Y);
                }

                //движение головы змеи
                tochka[0] = new Point() { X = steps[0].X + tochka[0].X, Y = steps[0].Y + tochka[0].Y };
                Canvas.SetLeft(zmeas[0], tochka[0].X);
                Canvas.SetTop(zmeas[0], tochka[0].Y);

                //задержка
                var result = await Task<string>.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                    return "1";
                });
            }

            //конец игры
            MessageBox.Show("Счет: " + shet);
            Show();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Show();
        }

        public void Show()
        {
            //обнуление и очищение
            step = "right";
            numzme = 0;
            shet = 0;
            tochka.Clear();
            steps.Clear();
            zmeas.Clear();
            canv.Children.Clear();
            tochka.Add(new Point(0,0));
            steps.Add(new Point(20, 0));

            //добавление грибочка
            grib.Fill = Brushes.Green;
            grib.Width = 20;
            grib.Height = 20;
            Canvas.SetLeft(grib, (rand.Next(0, 380) / 20) * 20);
            Canvas.SetTop(grib, (rand.Next(0, 360) / 20) * 20);
            canv.Children.Add(grib);

            //добавление головы змеи
            zmeas.Add(new Rectangle());
            zmeas[numzme].Fill = Brushes.Black;
            zmeas[numzme].Width = 20;
            zmeas[numzme].Height = 20;
            canv.Children.Add(zmeas[numzme]);
            Canvas.SetLeft(zmeas[numzme], tochka[numzme].X);
            Canvas.SetTop(zmeas[numzme], tochka[numzme].Y);          

            //открытие/закрытие
            if (MessageBox.Show("Начать?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                 Go();
            }
            else
                Close();
        }

        private void Window_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            //изменение направления
            for (int j = 0; j <= numzme; j++)
            {
                if (e.Key == Key.Down)
                {
                    if (step == "down" || step == "up") return;
                    step = "down";
                    steps[0] = new Point() { X = 0, Y = 20 };
                }

                if (e.Key == Key.Up)
                {
                    if (step == "down" || step == "up") return;
                    step = "up";
                    steps[0] = new Point() { X = 0, Y = -20 };
                }

                if (e.Key == Key.Left)
                {
                    if (step == "right" || step == "left") return;
                    step = "left";
                    steps[0] = new Point() { X = -20, Y = 0 };
                }

                if (e.Key == Key.Right)
                {
                    if (step == "right" || step == "left") return;
                    step = "right";
                    steps[0] = new Point() { X = 20, Y = 0 };
                }
            }

        }
    }
}
