using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CA1
{
    // CA1 X00130180 - Jordan Williams(Whoops GDPR :()
    class Program
    {
        static void Main()
        {
            int toDraw = 0;
            int max = 0;
            List<LotteryDraw> draws = new List<LotteryDraw>();
            for(int i=0; i<5; i++)
            {
                Console.Write("Enter the size of the draw: ");
                try
                {
                    max = Convert.ToInt32(Console.ReadLine());
                    draws.Add(new LotteryDraw(max));
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            LotteryDrawHistory history = new LotteryDrawHistory();

            foreach(LotteryDraw draw in draws)
            {
                Console.Write("\nHow many numbers do you want to draw? ");
                try
                {
                    toDraw = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                draw.DrawNumbers(toDraw);
                history.AddLotteryDraw(draw);
                Console.WriteLine(draw);
            }

            Console.WriteLine(history);
            Console.ReadLine();
        }
    }

    class LotteryDraw
    {
        List<int> numbersDrawn;
        int max;

        public List<int> NumbersDrawn
        {
            get
            {
                return this.numbersDrawn;
            }
        }

        public int Max
        {
            get
            {
                return this.max;
            }
            private set
            {
                this.max = value;
            }
        }

        public LotteryDraw() : this(47) { }

        public LotteryDraw(int max)
        {
            this.Max = max;
            this.numbersDrawn = new List<int>();
        }

        public void DrawNumbers(int toDraw)
        {
            Random r = new Random();
            HashSet<int> uniqueNumbers = new HashSet<int>();
            while (uniqueNumbers.Count < toDraw)
            {
                try
                {
                    uniqueNumbers.Add(r.Next(1, this.Max+1));
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            foreach(int number in uniqueNumbers)
            {
                this.numbersDrawn.Add(number);
            }

            this.numbersDrawn.Sort();
        }

        public override string ToString()
        {
            String stringNumbers = "";
            foreach(int number in this.NumbersDrawn)
            {
                stringNumbers += number + ",";
            }
            return "\nMax: " +this.Max + "\nNumbers drawn: " + stringNumbers;
        }
    }

    class LotteryDrawHistory
    {
        List<LotteryDraw> history;

        public List<LotteryDraw> History
        {
            get
            {
                return this.history;
            }
        }

        public LotteryDrawHistory()
        {
            this.history = new List<LotteryDraw>();
        }

        public void AddLotteryDraw(LotteryDraw lotteryDraw)
        {
            this.history.Add(lotteryDraw);
        }

        public int GetMostFrequentNumber()
        {
            IDictionary<int, int> numberOfTimesDrawn = new Dictionary<int, int>();
            foreach(LotteryDraw draw in this.History)
            {
                foreach(int number in draw.NumbersDrawn)
                {
                    if (!numberOfTimesDrawn.ContainsKey(number))
                    {
                        numberOfTimesDrawn.Add(number, 1);
                    }
                    else
                    {
                        numberOfTimesDrawn[number]++;
                    }
                }
            }
            try
            {
                var max = numberOfTimesDrawn.Where(x => x.Value == 3).Max(x => x.Key);
                return max;
            }
            catch(InvalidOperationException)
            {
                Console.WriteLine("No clear frequent number");
            }
            return 0;
        }

        public override string ToString()
        {
            String stringHistory = "";
            foreach(LotteryDraw draw in this.History)
            {
                stringHistory += draw;
            }
            if(this.GetMostFrequentNumber() == 0)
            {
                return "\nHistory of draws:\n" + stringHistory + "\nMost Frequent: " + "There was no clear frequent number.";
            }
            return "\nHistory of draws:\n" + stringHistory + "\nMost Frequent: " + this.GetMostFrequentNumber();
        }
    }
}
