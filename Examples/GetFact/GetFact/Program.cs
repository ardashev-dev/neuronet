using System;
using System.Collections.Generic;
using System.Text;

namespace GetFact
{
  class Program
  {
    public struct DataStruct
    {
      public string inputString;
      public string fact;
    }

    static void Main(string[] args)
    {
      var nBuilder = new SimpleNeuroNet.NeuronetBuilder(512, 1024, 512);
      var minimumError = 0.3f;
      var data = new List<DataStruct>();
      #region test data
      data.Add(
          new DataStruct()
          {
            inputString = "Продолжаем хождение по Миру Атома. На этот раз посмотрим как стример включает своё обаяние, и потом заводит жертву в ловушку.",
            fact = "Миру Атома"
          }
        );
      data.Add(
          new DataStruct()
          {
            inputString = "Немного информации о процессе разработке игры Jaws & Claws (неестественный отбор).",
            fact = "Jaws & Claws"
          }
        );
      data.Add(
          new DataStruct()
          {
            inputString = "Примите участите в гонке в фанатской игре Super Mario Kart ZX с персонажами из серии игр про Марио. Игра поддерживает до 4 игроков и широкоэкранное соотношение сторон.",
            fact = "Super Mario Kart ZX"
          }
        );
      data.Add(
          new DataStruct()
          {
            inputString = "Фанатская игра SUPERHOTline Miami совмещает в себе игровой процесс Hotline Miami и механику SUPER HOT. Время движется только тогда, когда вы двигаетесь, но на этот раз с видом сверху",
            fact = "SUPERHOTline Miami"
          }
        );

      data.Add(
          new DataStruct()
          {
            inputString = "В SHOOTOUT INC. вы играете за робота QA-Bot1986 по обеспечению качества оружия, работающего на большую и сомнительную корпорацию.",
            fact = "SHOOTOUT INC."
          }
        );

      data.Add(
          new DataStruct()
          {
            inputString = "DoomRL (Doom, the Roguelike) - это фанатский, динамичный и пошаговый рогалик с системой прогресса, который был вдохновлён такими играми, как Doom и Rogue. Доступны 3 класса перед началом сражения: Морпех, Разведчик и Техник.",
            fact = "DoomRL (Doom, the Roguelike)"
          }
        );

      data.Add(
          new DataStruct()
          {
            inputString = "Klonoa - The Dream Chapter - это фанатское переосмысление оригинальной Klonoa с некоторыми механиками ракетных прыжков на движке Unreal Engine 4.",
            fact = "Klonoa - The Dream Chapter"
          }
        );

      data.Add(
          new DataStruct()
          {
            inputString = "Станьте непобедимым героем с неограниченными сверхспособностями в игре UNDEFEATED. Патрулируйте улицы, летая над городом и останавливайте преступления или неприятности.",
            fact = "UNDEFEATED"
          }
        );

      data.Add(
          new DataStruct()
          {
            inputString = "Sanicball - это глупая гоночная игра, которая отбрасывает сложную механику игрового процесса, прогрессию персонажей и микротранзакции в пользу скорости.",
            fact = "Sanicball"
          }
        );

      data.Add(
          new DataStruct()
          {
            inputString = "Call of Abdulov - это фанатская игра от первого лица в духе олдскульных брутальный шутеров. Россию охватила эпидемия страшного вируса, который искажает разум и тело людей, превращая их в тупых и уродливых монстров.",
            fact = "Call of Abdulov"
          }
        );

      data.Add(
          new DataStruct()
          {
            inputString = "Serious Lord Of The Rings Bros. - это фанатская смесь Zelda II, Mario и The Lord of the Rings.",
            fact = "Serious Lord Of The Rings Bros."
          }
        );
      #endregion 

      var inputList = new List<double[]>();
      var outputList = new List<double[]>();

      foreach (var item in data)
      {
        Console.WriteLine(string.Format("item.inputString = {0}", item.inputString.Length));
        var inputListItem = Encoding.Default.GetBytes(item.inputString);
        Console.WriteLine(string.Format("inputListItem = {0}", inputListItem.Length));
        var inputListItemDouble = new List<double>();
        foreach (var i in inputListItem)
          inputListItemDouble.Add(i / 256);

        var inputListItemDouble512 = new double[512];
        var nom = 0;
        foreach (var element in inputListItemDouble)
        {
          inputListItemDouble512[nom] = element;
          nom++;
        }
        inputList.Add(inputListItemDouble512);

        var outputListItem = Encoding.Default.GetBytes(item.fact);
        var outputListItemDouble = new List<double>();
        foreach (var i in outputListItem)
          outputListItemDouble.Add(i / 256);

        var outputListItemDouble256 = new double[512];
        nom = 0;
        foreach (var element in inputListItemDouble)
        {
          outputListItemDouble256[nom] = element;
          nom++;
        }

        outputList.Add(outputListItemDouble256);
      }

      var resultData = nBuilder.Train(inputList, outputList, minimumError);

      nBuilder.SaveToFile(resultData, @"neuronetWeigths.txt");
      Console.WriteLine("End training neuronet. ");

      var getResultData = nBuilder.LoadFromFile(@"neuronetWeigths.txt");

      nBuilder.Load(getResultData);

      var neuronet = nBuilder.GetNeuronet();
      var readline = string.Empty;
      while (!readline.Equals("quit"))
      {
        Console.Write("Строка для извлечения факта: ");
        readline = Console.ReadLine();

        var inputListItem = Encoding.Default.GetBytes(readline);
        var inputListItemDouble = new List<double>();
        foreach (var i in inputListItem)
          inputListItemDouble.Add(i / 255);

        var inputListItemDouble256 = new double[512];
        var nom = 0;
        foreach (var element in inputListItemDouble)
        {
          inputListItemDouble256[nom] = element;
          nom++;
        }

        double[] fact = neuronet.Compute(inputListItemDouble256);
        var factByte = new List<byte>();
        foreach (var element in fact)
        {
          factByte.Add((byte)(element * 256));
        }
        Console.Write("Извлеченный факт: ");
        var strFact = Encoding.Default.GetString(factByte.ToArray());
        Console.WriteLine(strFact);
      }
    }
  }
}
