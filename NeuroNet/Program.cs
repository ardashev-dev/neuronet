﻿using System;
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

    public static void Train(float taskNumber, float devNumber, float testNumber, float devDays, float labor, float complNumber)
    {
      double[] input = { taskNumber, devNumber, testNumber, labor, devDays };
      double[] output = { complNumber };
      dataSets.Add(new NeuroNet.DataSet(input, output));

      neuronet.Train(dataSets, MinimumError, int.MaxValue);
    }

    public struct DataStruct
    {
      public float taskNumber;
      public float devNumber;
      public float testNumber;
      public float devDays;
      public float labor;
      public float complNumber;
    }

    static void Main(string[] args)
    {
      Console.WriteLine("Start training neuronet... ");
      dataSets = new List<NeuroNet.DataSet>();
      neuronet = new NeuroNet.Neuronet(5, 6, 1);
      var data = new List<DataStruct>();
      /* Input Data:
         -- nuber of tasks; 
         -- nuber of developer;
         -- labor intensity.

         Output Data:
         -- number of completed tasks.
       */

      data.Add(new DataStruct() { taskNumber = 26f, devNumber = 4f, testNumber = 4f, devDays = 5f, labor = 105.5f, complNumber = 11f });
      data.Add(new DataStruct() { taskNumber = 33f, devNumber = 3f, testNumber = 4f, devDays = 5f, labor = 93.5f, complNumber = 6f });
      data.Add(new DataStruct() { taskNumber = 30f, devNumber = 3f, testNumber = 4f, devDays = 5f, labor = 84f, complNumber = 12f });
      data.Add(new DataStruct() { taskNumber = 30f, devNumber = 3f, testNumber = 4f, devDays = 5f, labor = 92f, complNumber = 8f });
      data.Add(new DataStruct() { taskNumber = 26f, devNumber = 4f, testNumber = 5f, devDays = 5f, labor = 82f, complNumber = 14f });
      data.Add(new DataStruct() { taskNumber = 23f, devNumber = 4f, testNumber = 5f, devDays = 5f, labor = 71f, complNumber = 15f });
      data.Add(new DataStruct() { taskNumber = 21f, devNumber = 4f, testNumber = 3f, devDays = 4f, labor = 50f, complNumber = 13f });

      var maxTaskNumber = data.Select(x => x.taskNumber).Max();
      var maxDevNumber = data.Select(x => x.devNumber).Max();
      var maxTestNumber = data.Select(x => x.testNumber).Max();
      var maxDevDays = data.Select(x => x.devDays).Max();
      var maxLabor = data.Select(x => x.labor).Max();
      var maxComplNumber = data.Select(x => x.complNumber).Max();

      var itemCount = 0;
      foreach (var item in data)
      {
        itemCount++;
        Console.WriteLine(string.Format("Train {0} - Start", itemCount));
        Train(item.taskNumber/maxTaskNumber, item.devNumber/maxDevNumber, item.testNumber/maxTestNumber, item.labor/maxLabor, item.devDays/maxDevDays, item.complNumber/maxComplNumber);
        Console.WriteLine(string.Format("Train {0} - End", itemCount));
      }

      Console.WriteLine("End training neuronet. ");

      var readline = Console.ReadLine();

      while (!readline.Equals("quit"))
      {
        Console.Write("Количество задач: ");
        var taskNumberStr = Console.ReadLine();
        Console.Write("Число разработчиков: ");
        var devNumberStr = Console.ReadLine();
        Console.Write("Число консультантов: ");
        var testNumberStr = Console.ReadLine();
        Console.Write("Число рабочих дней: ");
        var devDaysStr = Console.ReadLine();
        Console.Write("Общая плановая трудоемкость: ");
        var laborStr = Console.ReadLine();

        var taskNumber = float.Parse(taskNumberStr);
        var devNumber = float.Parse(devNumberStr);
        var devDays = float.Parse(devDaysStr);
        var testNumber = float.Parse(testNumberStr);
        var labor = float.Parse(laborStr);

        double[] input = { taskNumber/maxTaskNumber, devNumber/maxDevNumber, testNumber / maxTestNumber, devDays/maxDevDays, labor /maxLabor };

        double[] compute = neuronet.Compute(input);

        Console.Write("result: ");

        foreach (var item in compute)
        {
          var result = item * maxComplNumber;
          if (result > taskNumber)
            result = taskNumber;
          
          Console.Write(((int)result).ToString());
        }
        Console.WriteLine();
        Console.WriteLine("===================");
        readline = Console.ReadLine();
      }


    }
  }
}
