using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace SimpleNeuroNet
{
  public class NeuronetBuilder
  {
    public static Neuronet neuronet;
    public static int InputLayerSize { get; set; }
    public static int HiddenLayerSize { get; set; }
    public static int OutputLayerSize { get; set; }
    public static int NumHiddenLayers { get; set; }

    public NeuronetBuilder(int inputLayerSize, int hiddenLayerSize, int outputLayerSize, int numHiddenLayers = 1, double? learnRate = null, double? momentum = null)
    {
      InputLayerSize = inputLayerSize;
      HiddenLayerSize = hiddenLayerSize;
      OutputLayerSize = outputLayerSize;
      NumHiddenLayers = numHiddenLayers;
      neuronet = new Neuronet(inputLayerSize, hiddenLayerSize, outputLayerSize, numHiddenLayers, learnRate, momentum);
    }

    public Neuronet GetNeuronet()
    {
      return neuronet;
    }

    #region Structures
    public struct NeuronetData
    {
      public int InputLayerSize { get; set; }
      public int HiddenLayerSize { get; set; }
      public int NumberHiddenLayer { get; set; }
      public int OutputLayerSize { get; set; }
      public List<NeuronData> Neurons { get; set; }
    }

    public struct NeuronData
    {
      public int Layer { get; set; }
      public int SerialNumber { get; set; }
      public List<SynapseData> Synapses { get; set; }
    }

    public struct SynapseData
    {
      public int SerialNumber { get; set; }
      public double Weight { get; set; }
      public double WeightDelta { get; set; }
    }
    #endregion

    public NeuronetData Train(List<double[]> inputList, List<double[]> outputList, float minimumError)
    {
      var neuronetData = new NeuronetData();
      var neurons = new List<NeuronData>();
      var number = 0;
      foreach (var input in inputList)
      {
        var output = outputList.ToArray();
        var dataSets = new List<DataSet>();
        dataSets.Add(new DataSet(input, output[number]));
        neuronet.Train(dataSets, minimumError, int.MaxValue);
        number++;
      }

      foreach (var item in neuronet.InputLayer)
      {
        var neuron = GetNeuronData(item);
        neurons.Add(neuron);
      }
      foreach (var hiddenLayer in neuronet.HiddenLayers)
        foreach (var item in hiddenLayer)
        {
          var neuron = GetNeuronData(item);
          neurons.Add(neuron);
        }
      foreach (var item in neuronet.OutputLayer)
      {
        var neuron = GetNeuronData(item);
        neurons.Add(neuron);
      }
      neuronetData.Neurons = neurons;
      neuronetData.HiddenLayerSize = HiddenLayerSize;
      neuronetData.InputLayerSize = InputLayerSize;
      neuronetData.NumberHiddenLayer = NumHiddenLayers;
      neuronetData.OutputLayerSize = OutputLayerSize;
      return neuronetData;
    }

    private static NeuronData GetNeuronData(Neuron item)
    {
      var neuron = new NeuronData();
      var synapses = new List<SynapseData>();
      neuron.Layer = item.Layer;
      neuron.SerialNumber = item.SerialNumber;
      foreach (var synItem in item.InputSynapses)
      {
        var synaps = new SynapseData();
        synaps.SerialNumber = synItem.SerialNumber;
        synaps.Weight = synItem.Weight;
        synaps.WeightDelta = synItem.WeightDelta;
        synapses.Add(synaps);
      }
      neuron.Synapses = synapses;
      return neuron;
    }

    public void Load(NeuronetData neuronetData)
    {
      var layerNum = 1;
      foreach (var item in neuronet.InputLayer)
        LoadNeuron(item, neuronetData, layerNum);
      foreach (var hiddenLayer in neuronet.HiddenLayers)
      {
        layerNum++;
        foreach (var item in hiddenLayer)
          LoadNeuron(item, neuronetData, layerNum);
      }
      layerNum++;
      foreach (var item in neuronet.OutputLayer)
        LoadNeuron(item, neuronetData, layerNum);
    }

    private void LoadNeuron(Neuron item, NeuronetData neuronetData, int layerNum)
    {
      var neuron = neuronetData.Neurons.Where(x => x.Layer == layerNum && x.SerialNumber == item.SerialNumber).FirstOrDefault();
      foreach (var synItem in item.InputSynapses)
      {
        var synaps = neuron.Synapses.Where(x => x.SerialNumber == synItem.SerialNumber).FirstOrDefault();
        synItem.Weight = synaps.Weight;
        synItem.WeightDelta = synaps.WeightDelta;
      }
    }

    public void SaveToFile(NeuronetData neuronetData, string path)
    {
      var jsonString = JsonSerializer.Serialize(neuronetData);
      System.IO.File.WriteAllText(path, jsonString);
    }

    public NeuronetData LoadFromFile(string path)
    {
      var jsonString = System.IO.File.ReadAllText(path);
      var neuronetData = JsonSerializer.Deserialize<NeuronetData>(jsonString);
      return neuronetData;
    }

  }
}
