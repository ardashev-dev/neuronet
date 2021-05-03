using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNet
{
  class Program
  {

    private static List<NeuroNet.DataSet> dataSets;
    private static NeuroNet.Neuronet neuronet;
    private static float MinimumError = 0.01f;

    public static void Train(float taskNumber, float devNumber, float labor, float complNumber)
    {
      double[] input = { taskNumber, devNumber, labor };
      double[] output = { complNumber };
      dataSets.Add(new NeuroNet.DataSet(input, output));

      neuronet.Train(dataSets, MinimumError, int.MaxValue);
    }

    public struct DataStruct
    {
      public float taskNumber;
      public float devNumber;
      public float labor;
      public float complNumber;
    }

    static void Main(string[] args)
    {
      Console.WriteLine("Start training neuronet... ");
      dataSets = new List<NeuroNet.DataSet>();
      neuronet = new NeuroNet.Neuronet(3, 5, 1);
      var data = new List<DataStruct>();
      /* Input Data:
         -- nuber of tasks; 
         -- nuber of developer;
         -- labor intensity.

         Output Data:
         -- number of completed tasks.
       */

      data.Add(new DataStruct() { taskNumber = 30f, devNumber = 4f, labor = 68f, complNumber = 11f} );
      data.Add(new DataStruct() { taskNumber = 26f, devNumber = 4f, labor = 54f, complNumber = 14f} );
      data.Add(new DataStruct() { taskNumber = 16f, devNumber = 4f, labor = 32f, complNumber = 16f} );
      data.Add(new DataStruct() { taskNumber = 5f, devNumber = 1f, labor = 8f, complNumber = 4f });

      var maxTaskNumber = data.Select(x => x.taskNumber).Max();
      var maxDevNumber = data.Select(x => x.devNumber).Max();
      var maxLabor = data.Select(x => x.labor).Max();
      var maxComplNumber = data.Select(x => x.complNumber).Max();

      var itemCount = 0;
      foreach (var item in data)
      {
        itemCount++;
        Console.WriteLine(string.Format("Train {0} - Start", itemCount));
        Train(item.taskNumber/maxTaskNumber, item.devNumber/maxDevNumber, item.labor/maxLabor, item.complNumber/maxComplNumber);
        Console.WriteLine(string.Format("Train {0} - End", itemCount));
      }

      Console.WriteLine("End training neuronet. ");

      var readline = Console.ReadLine();

      while (!readline.Equals("quit"))
      {
        Console.Write("taskNumber: ");
        var taskNumberStr = Console.ReadLine();
        Console.Write("devNumber: ");
        var devNumberStr = Console.ReadLine();
        Console.Write("labor: ");
        var laborStr = Console.ReadLine();

        var taskNumber = float.Parse(taskNumberStr);
        var devNumber = float.Parse(devNumberStr);
        var labor = float.Parse(laborStr);

        double[] input = { taskNumber/maxTaskNumber, devNumber/maxDevNumber, labor/maxLabor };

        double[] compute = neuronet.Compute(input);

        Console.Write("result: ");

        foreach (var item in compute)
        {
          Console.Write((item* maxComplNumber).ToString());
        }
        Console.WriteLine("===================");
        readline = Console.ReadLine();
      }


    }
  }
}
