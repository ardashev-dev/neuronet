using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNeuroNet
{
	public class Synapse
	{
		public Neuron InputNeuron { get; set; }
		public Neuron OutputNeuron { get; set; }
		public double Weight { get; set; }
		public double WeightDelta { get; set; }
		public int SerialNumber { get; set; }

		public Synapse(Neuron inputNeuron, Neuron outputNeuron)
		{
			InputNeuron = inputNeuron;
			OutputNeuron = outputNeuron;
			Weight = Neuronet.GetRandom();
		}
	}
}
