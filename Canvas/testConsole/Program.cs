using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace testConsole
{
    class Program
    {
        public delegate void DelegateClick(int a);

        public class butt

        {

            public event DelegateClick Click;

            public void OnClick(int a)

            {

                if (Click != null)

                    Click.Invoke(a);

                //Click(a);//这种方式也是可以的

                MessageBox.Show("Click();");

            }

        }

        class Frm

        {

            public static void Btn_Click(int a)

            {

                for (long i = 0; i < a; i++)

                    Console.WriteLine(i.ToString());

            }

            static void Main(string[] args)

            {

                butt b = new butt();

                //在委托中，委托对象如果是null的，直接使用+=符号，会报错，但是在事件中，初始化的时候，只能用+=

                b.Click += new DelegateClick(Btn_Click); //事件是基于委托的，所以委托推断一样适用，下面的语句一样有效：b.Click += Fm_Click;

                //b.Click(10);错误:事件“DelegateStudy.butt.Click”只能出现在 += 或 -= 的左边(从类型“DelegateStudy.butt”中使用时除外)

                b.OnClick(10000);

                MessageBox.Show("sd234234234");

                Console.ReadLine();

            }

        }
    }
}
