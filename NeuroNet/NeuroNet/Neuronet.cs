using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNet.NeuroNet
{
  class Neuronet
  {
		public double LearnRate { get; set; }
		public double Momentum { get; set; }
		public List<Neuron> InputLayer { get; set; }
		public List<List<Neuron>> HiddenLayers { get; set; }
		public List<Neuron> OutputLayer { get; set; }

		private static readonly System.Random Random = new System.Random();

		public Neuronet(int inputLayerSize, int hiddenLayerSize, int outputLayerSize, int numHiddenLayers = 1, double? learnRate = null, double? momentum = null)
		{
			LearnRate = learnRate ?? .4;
			Momentum = momentum ?? .9;
			InputLayer = new List<Neuron>();
			HiddenLayers = new List<List<Neuron>>();
			OutputLayer = new List<Neuron>();

			var currentLayer = 1;

			for (var i = 0; i < inputLayerSize; i++)
			{
				var neuron = new Neuron();
				neuron.Layer = currentLayer;
				neuron.SerialNumber = i + 1;
				InputLayer.Add(neuron);
			}

			for (int i = 0; i < numHiddenLayers; i++)
			{
				currentLayer++;
				HiddenLayers.Add(new List<Neuron>());
				for (var j = 0; j < hiddenLayerSize; j++)
				{
					var neuron = new Neuron(i == 0 ? InputLayer : HiddenLayers[i - 1]);
					neuron.Layer = currentLayer;
					neuron.SerialNumber = j + 1;
					HiddenLayers[i].Add(neuron);
				}
			}
			currentLayer++;
			for (var i = 0; i < outputLayerSize; i++)
			{
				var neuron = new Neuron(HiddenLayers[numHiddenLayers - 1]);
				neuron.Layer = currentLayer;
				neuron.SerialNumber = i + 1;
				OutputLayer.Add(neuron);
			}
		}

		public void Train(List<DataSet> dataSets, double minimumError, int maxEpochsNum)
		{
			var error = 1.0;
			var numEpochs = 0;

			while (error > minimumError && numEpochs < maxEpochsNum)
			{
				var errors = new List<double>();
				foreach (var dataSet in dataSets)
				{
					ForwardPropagate(dataSet.Values);
					BackPropagate(dataSet.Targets);
					errors.Add(CalculateError(dataSet.Targets));
				}
				error = errors.Average();
				numEpochs++;
			}
		}

		private void ForwardPropagate(params double[] inputs)
		{
			var i = 0;
			InputLayer.ForEach(a => a.Value = inputs[i++]);
			foreach (var layer in HiddenLayers)
				layer.ForEach(a => a.CalculateValue());
			OutputLayer.ForEach(a => a.CalculateValue());
		}

		private void BackPropagate(params double[] targets)
		{
			var i = 0;
			OutputLayer.ForEach(a => a.CalculateGradient(targets[i++]));
			foreach (var layer in HiddenLayers.AsEnumerable<List<Neuron>>().Reverse())
			{
				layer.ForEach(a => a.CalculateGradient());
				layer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
			}
			OutputLayer.ForEach(a => a.UpdateWeights(LearnRate, Momentum));
		}

		public double[] Compute(params double[] inputs)
		{
			ForwardPropagate(inputs);
			return OutputLayer.Select(a => a.Value).ToArray();
		}

		private double CalculateError(params double[] targets)
		{
			var i = 0;
			return OutputLayer.Sum(a => Math.Abs((float)a.CalculateError(targets[i++])));
		}

		public static double GetRandom()
		{
			return 2 * Random.NextDouble() - 1;
		}
	}
}
