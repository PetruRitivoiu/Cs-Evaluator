using System;
using System.Linq;

namespace DemoProiectPAW
{
    public interface ITestable
    {
        void Test();
    }

    public abstract class AbstractTest : ITestable
    {
        public string TestMessage { get; protected set; }

        public AbstractTest()
        {
            TestMessage = "Base test";
        }

        public abstract void AppendText(string s);

        public void Test()
        {
            Console.WriteLine(TestMessage);
        }
    }

    public class Class1 : AbstractTest, ICloneable, IComparable
    {
        private int _prop1;
        private int _prop2;
        private int[] vector;

        private int Prop1
        {
            get
            {
                return this._prop1;
            }
            set
            {
                this._prop1 = value;
            }
        }
        private int Prop2
        {
            get
            {
                return this._prop2;
            }
            set
            {
                this._prop2 = value;
            }
        }

        public int this[int i]
        {
            get { return vector[i]; }
        }

        public Class1(int prop1, int prop2)
        {
            this._prop1 = prop1;
            this._prop2 = prop2;

            vector = new int[1024];

            for (int i = 0; i < 1024; i++)
            {
                vector[i] = i;
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public override void AppendText(string s)
        {
            this.TestMessage += s.Reverse();
        }
    }

    public class Class2 : ICloneable, IComparable
    {
        private string _prop1;
        private string _prop2;

        private string Prop1
        {
            get
            {
                return this._prop1;
            }
            set
            {
                this._prop1 = value;
            }
        }
        private string Prop2
        {
            get
            {
                return this._prop2;
            }
            set
            {
                this._prop2 = value;
            }
        }

        public Class2(string prop1, string prop2)
        {
            this._prop1 = prop1;
            this._prop2 = prop2;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public class Metronome
    {
        public event TickHandler Tick;
        public delegate void TickHandler(Metronome m, EventArgs e);

        public void TickEvent()
        {
            if (Tick != null)
            {
                Tick(this, EventArgs.Empty);
                Console.WriteLine("Metronome ticked");
            }
        }
    }

    public class Listener
    {
        public void Subscribe(Metronome m)
        {
            m.Tick += new Metronome.TickHandler(HeardIt);
            m.Tick += new Metronome.TickHandler((m1, e1) => Console.WriteLine("Tick Handler 2 -> Inline lambda Handler"));
        }
        private void HeardIt(Metronome m, EventArgs e)
        {
            System.Console.WriteLine("Tick Handler 1");
        }

    }

    class Program
    {

        static void Main(string[] args)
        {
            Listener l = new Listener();
            Metronome m = new Metronome();

            l.Subscribe(m);
            m.TickEvent();

            Console.ReadLine();
        }
    }
}
