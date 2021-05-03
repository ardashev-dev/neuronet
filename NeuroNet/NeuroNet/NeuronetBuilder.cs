using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNet.NeuroNet
{
  class NeuronetBuilder
  {
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
      public float Weight { get; set; }
    }
    #endregion

    public void CreateNeuronetData()
    { 
      
    }

  }
}
