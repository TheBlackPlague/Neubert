/*
This was coded by Shaheryar Sohail.
By the MIT License, you can do whatever you want with this file with no restrictions unless implied in the License.
You cannot however remove this commented in citation of the authorship of the file. You must add this to any file using code from this file.
*/

using System;
using System.Net;
using System.Threading;
using System.IO;
using System.Collections.Generic;
// Project
using Neubert;
using Neubert.Utility;

namespace Neubert.Layer {

public class BrainLayer {

    public int neuronAmount;
    public bool repition;
    public static float minimumBias = 0;
    public static float maximumBias = 0;
    public float[] biasArray;
    public Brain.ActivateNeuronFormula activeNeuronFormula;

    /**
    * Methods used to construct class.
    * Class Construction Methods :
    * Layer()
    * Layer(repitionDelta, neuronAmountDelta, activateNeuronFormulaDelta)
    * Layer(data)
    */

    public BrainLayer() {
        // non-value construction
    }

    public BrainLayer(bool repitionDelta, int neuronAmountDelta, Brain.ActivateNeuronFormula activateNeuronFormulaDelta) {
        neuronAmount = neuronAmountDelta;
        repition = repitionDelta;
        activeNeuronFormula = activateNeuronFormulaDelta;
        biasArray = null;
    }

    public BrainLayer(BrainLayer data) {
        neuronAmount = data.neuronAmount;
        repition = data.repition;
        biasArray = null;
        activeNeuronFormula = data.activeNeuronFormula;
    }

    public void StartUp() {
        biasArray = new float[
            neuronAmount
        ];
    }

    public void RandomizeBiasArray() {
        int i = neuronAmount;
        while (i-- > 0) {
            biasArray[i] = (float)DataManagement.NextDouble * (maximumBias - minimumBias) + minimumBias;
        }
    }

    public void CopyBiasArrayFromAnotherLayer(BrainLayer data) {
        float _biasArray = data.biasArray;
        int i = neuronAmount;
        while (i-- > 0) {
            biasArray[i] = _biasArray[i];
        }
    }

    public void BiasMutation(float mutationChance) {
        int i = neuronAmount;
        while (i-- > 0) {
            if ((float)DataManagement.NextDouble() <= mutationChance) {
                biasArray[i] = (float)DataManagement.NextDouble() * (maximumBias - minimumBias) + minimumBias;
            }
        }
    }

    public void BiasBreeding(BrainLayer data) {
        int i = neuronAmount;
        while (i-- > 0) {
            float value = (float)DataManagement.NextDouble();
            biasArray[i] = biasArray[i] * value + data.biasArray[i] * (1.0f - value);
        }
    }

    /**
    public voud SaveStruct(Stream stream) {
        DataManagement.IntToDataStream(neuronAmount, s);
        byte byteToWrite = repition ? (byte)1 : (byte)0;
        s.WriteByte(byteToWrite);
        // ...
    }

    public void LoadStruct(Stream stream) ...
    */

    public void Save(Stream stream) {
        int i = neuronAmount;
        while (i-- > 0) {
            s.Write(DataManagement.FloatToByte(biasArray[i]), 0, 4);
        }
    }

    public void Load(Stream s) {
        byte[] allByte = new byte[4];
        int i = neuronAmount;
        while (i-- > 0) {
            s.Read(allByte, 0, 4);
            biasArray[i] = DataManagement.FloatFromByte(allByte);
        }
    }

}

}