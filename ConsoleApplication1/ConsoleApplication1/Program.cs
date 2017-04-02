using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConsoleApplication1
{
    class Dispatcher
    {
        public string Name { get; private set; }
        public int Correct { get; private set; }
        Random r = new Random();
        public Dispatcher(string name)
        {
            Name = name;
            Correct = r.Next(-200, 200);
            points = 0;
        }
        public int points { get; private set; }
        public void Check(object sender, EventArgs e)
        {
            check((Samolet)sender);
        }
        public void check(Samolet s)
        {
            int recHigh = (7 * s.Speed) - Correct;
            Console.WriteLine("Recommended High from dispatcher " + this.Name + " is " + recHigh);
            Console.WriteLine("Your High is " + s.High);
            if ((Math.Sqrt((double)(s.High - recHigh) * (s.High - recHigh)) > 300) && (Math.Sqrt((double)(s.High - recHigh) * (s.High - recHigh)) < 600))
            {
                points += 25;
                Console.WriteLine("25 points!");
            }
            else if ((Math.Sqrt((double)(s.High - recHigh) * (s.High - recHigh)) > 600) && (Math.Sqrt((double)(s.High - recHigh) * (s.High - recHigh)) < 1000))
            {
                points += 50;
                Console.WriteLine("50 points!");
            }
            else if (Math.Sqrt((double)(s.High - recHigh) * (s.High - recHigh)) > 1000)
            {
                throw new Exception("Crashed!");
            }
            else
            {
                Console.WriteLine("High is ok!");
            }
            if (points > 1000)
            {
                throw new Exception("Ne prigoden!");
            }
            if (s.Speed > 1000)
            {
                Console.WriteLine("slowly please! 100 points");
                points += 100;
            }
            if ((s.Speed <= 0) || (s.High <= 0))
            {
                throw new Exception("Crashed!");
            }
        }
    }
    class Samolet
    {
        public event EventHandler cheked;
        public void Next(object sender, EventArgs e)
        {
            if (CanFly == true)
            {
                Console.WriteLine("up = +250m, shift+up = +500m, down = -250m, shift+down = -500m;\n right = +50 km/h, shift+right = +150 km/h, left = -50 km/h, shift+left = -150km/h");
                ConsoleKeyInfo cki;
                while (true)
                {
                    cki = Console.ReadKey();
                    if (cki.Modifiers == ConsoleModifiers.Shift)
                    {
                        if (cki.Key == ConsoleKey.RightArrow)
                        {
                            Console.WriteLine("speed+150km/h");
                            this.Speed += 150;
                            break;
                        }
                        else if (cki.Key == ConsoleKey.LeftArrow)
                        {
                            Console.WriteLine("speed -150km/h");
                            this.Speed -= 150;
                            break;

                        }
                        else if (cki.Key == ConsoleKey.DownArrow)
                        {
                            Console.WriteLine("High -500m");
                            this.High -= 500;
                            break;
                        }
                        else if (cki.Key == ConsoleKey.UpArrow)
                        {
                            Console.WriteLine("High +500m");
                            this.High += 500;
                            break;
                        }
                        else { Console.WriteLine("Unrecognized key! try again"); continue; }
                    }
                    else 
                    {
                        if (cki.Key == ConsoleKey.RightArrow)
                        {
                            Console.WriteLine("speed+50km/h");
                            this.Speed += 50;
                            break;
                        }
                        else if (cki.Key == ConsoleKey.LeftArrow)
                        {
                            Console.WriteLine("speed -50km/h");
                            this.Speed -= 50;
                            break;

                        }
                        else if (cki.Key == ConsoleKey.DownArrow)
                        {
                            Console.WriteLine("High -250m");
                            this.High -= 250;
                            break;
                        }
                        else if (cki.Key == ConsoleKey.UpArrow)
                        {
                            Console.WriteLine("High +250m");
                            this.High += 250;
                            break;
                        }
                        else { Console.WriteLine("Unrecognized key! try again"); continue; }
                    }
                    
                }
                if (Speed > 50)
                {
                    cheked(this, EventArgs.Empty);
                }
            }
            else
            {
                Console.WriteLine("Not enough dispathers! add please");
            }
        }
        public int Speed { get; set; }
        public int High { get; set; }
        public int MemoryPoint { get; private set; }
        public string Letchik { get; private set; }
        public List<Dispatcher> dispatchers = new List<Dispatcher>();
        public void add(Dispatcher d)
        {
            dispatchers.Add(d);
            cheked += d.Check;
        }
        public void remove(Dispatcher d)
        {
            MemoryPoint += d.points;
            cheked -= d.Check;
            dispatchers.Remove(d);

        }
        public bool CanFly { get { return dispatchers.Count > 1; } }
        public Samolet(string name)
        {
            Letchik = name;
            Speed = 0;
            High = 0;
            MemoryPoint = 0;
        }

    }

    class Program
    {
        public static event EventHandler nextmove;
        static void Main(string[] args)
        {
            try
            {
                int SpeedTarget = 1000;
                Console.WriteLine("enter the name of pilot");
                Samolet s = new Samolet("Alex");
                nextmove += s.Next;
                bool isAchieve = false;
                bool isPassed = false;
                do
                {
                    Console.Clear();
                    Console.WriteLine("1. - next move");
                    Console.WriteLine("2. - add dispatcher");
                    Console.WriteLine("3. - delete dispatcher");
                    Console.WriteLine("4. - your points");
                    Console.WriteLine("5. - your speed");
                    Console.WriteLine("6. - your high");
                    Console.WriteLine("7. - your target");
                    Console.WriteLine("8. - is target achieve?");
                    Console.WriteLine("9. - stop engine");
                    Console.WriteLine("0. - exit");
                    Console.WriteLine("NOTE: you need 2 or more dispatchers to get possability to fly\n  TIP: get slowly to 50 km/h before landing and take accelerate before go up lol");
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            nextmove(null, EventArgs.Empty);
                            Console.ReadKey();
                            if (s.Speed >= SpeedTarget) isAchieve = true;
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Enter name of dispatcher");
                            string name = Console.ReadLine();
                            Dispatcher d = new Dispatcher(name);
                            s.add(d);
                            Console.ReadKey();
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("Enter name of dispatcher, that you want to delete");
                            string delname = Console.ReadLine();
                            if (s.dispatchers.Exists(x => x.Name == delname))
                            {

                                s.remove(s.dispatchers.Find(x => x.Name == delname));
                            }
                            else Console.WriteLine("there is no dispatcher with that name");
                            Console.ReadKey();
                            break;
                        case 4:
                            Console.Clear();
                            int total = 0;
                            foreach (Dispatcher dis in s.dispatchers)
                            {
                                total += dis.points;
                            }
                            total += s.MemoryPoint;
                            Console.WriteLine("your points now is " + total);
                            Console.ReadKey();
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine(" your speed is " + s.Speed);
                            Console.ReadKey();
                            break;
                        case 6:
                            Console.Clear();
                            Console.WriteLine("your high is " + s.High);
                            Console.ReadKey();
                            break;
                        case 7:
                            Console.Clear();
                            Console.WriteLine("your target is " + SpeedTarget);
                            Console.ReadKey();
                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("is you achieve target:" + isAchieve.ToString());
                            Console.ReadKey();
                            break;
                        case 9:
                            Console.Clear();
                            if ((s.Speed == 0) && (s.High == 0)) isPassed = true;
                            else throw new Exception("Crashed!");
                            break;
                        case 0: throw new Exception("slabak");
                        default:
                            Console.Clear();
                            Console.WriteLine("try again");
                            Console.ReadKey();
                            break;
                    }

                } while (isPassed == false);
                if (isAchieve == true)
                {
                    Console.WriteLine("You passed");
                }
                else Console.WriteLine("Landing before achieve target! not passed");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
