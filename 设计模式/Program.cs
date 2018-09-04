using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 设计模式
{
    class Program
    {
        static void Main(string[] args)
        {
            Computer c = new Computer();
            using(ComputerAdapter adapter=new ComputerAdapter(c))
            {
                c.PlayMoive();
            }
            Console.ReadKey();
        }
    }
    public class Computer
    {
        public void Distory()
        {
            Console.WriteLine("销户电脑");
        }
        public void PlayMoive()
        {
            Console.WriteLine("播放电影");
        }

    }
    public class ComputerAdapter:IDisposable
    {
        private Computer computer;
        public ComputerAdapter(Computer computer)
        {
            this.computer = computer;
        }

        public void Dispose()
        {
            this.computer.Distory();
        }
    }
}
