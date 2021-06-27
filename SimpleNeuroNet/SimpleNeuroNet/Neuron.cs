using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace SimpleNeuroNet
{
	public class Neuron
	{
		public List<Synapse> InputSynapses { get; set; }
		public List<Synapse> OutputSynapses { get; set; }
		public int Layer { get; set; }
		public int SerialNumber { get; set; }
		public double Bias { get; set; }
		public double BiasDelta { get; set; }
		public double Gradient { get; set; }
		public double Value { get; set; }

		public Neuron()
		{
			InputSynapses = new List<Synapse>();
			OutputSynapses = new List<Synapse>();
			Bias = Neuronet.GetRandom();
		}

		public Neuron(IEnumerable<Neuron> inputNeurons) : this()
		{
			var synapseSerialNumber = 0;
			foreach (var inputNeuron in inputNeurons)
			{
				synapseSerialNumber++;
				var synapse = new Synapse(inputNeuron, this);
				synapse.SerialNumber = synapseSerialNumber;
				inputNeuron.OutputSynapses.Add(synapse);
				InputSynapses.Add(synapse);
			}
		}

		public double CalculateValue()
		{
			return Value = Sigmoid.Output(InputSynapses.Sum(a => a.Weight * a.InputNeuron.Value) + Bias);
		}

		public double CalculateError(double target)
		{
			return target - Value;
		}

		public double CalculateGradient(double? target = null)
		{
			if (target == null)
				return Gradient = OutputSynapses.Sum(a => a.OutputNeuron.Gradient * a.Weight) * Sigmoid.Derivative(Value);

			return Gradient = CalculateError(target.Value) * Sigmoid.Derivative(Value);
		}

		public void UpdateWeights(double learnRate, double momentum)
		{
			var prevDelta = BiasDelta;
			BiasDelta = learnRate * Gradient;
			Bias += BiasDelta + momentum * prevDelta;

			foreach (var synapse in InputSynapses)
			{
				prevDelta = synapse.WeightDelta;
				synapse.WeightDelta = learnRate * Gradient * synapse.InputNeuron.Value;
				synapse.Weight += synapse.WeightDelta + momentum * prevDelta;
			}
		}
	}
}
