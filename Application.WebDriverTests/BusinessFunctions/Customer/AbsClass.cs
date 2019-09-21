using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WebDriverTests.BusinessFunctions.Customer
{
  public abstract  class AbsClass
    {
        public static void Test()
        {
            Console.WriteLine("ed");
        }

        public abstract void Test1();
    }

    public class Check : AbsClass
    {
        public override void Test1()
        {
            Console.WriteLine("dd");
            throw new NotImplementedException();
        }

      public static void Main()
        {
            AbsClass.Test();
        }

    }

    
}
