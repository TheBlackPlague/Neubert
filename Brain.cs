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
using Neubert.Layer;

namespace Neubert {

public class Brain {

    public BrainLayer input;
    public BrainLayer[] hidden;
    public BrainLayer output;
    public Connection outputConnection;
    public Connection[] hiddenConnection;
    public Connection[] repitionConnection;
    public int maxHN;
    public int maxHC;
    public delegate float ActivateNeuronFormula(double x);

    public Brain(Brain brain) {
        input = new BrainLayer(brain.input);
        hidden = new BrainLayer[brain.hidden.Length];
        for (int i = 0; i < hidden.Length; i++) {
            hidden[i] = new BrainLayer(brain.hidden[i]);
            hidden[i].StartUp();
        }
        output = new BrainLayer(brain.output);
        output.StartUp();
        if (hidden.Length > 0) {
            hiddenConnection = new Connection[hidden.Length];
            repitionConnection = new Connection[hidden.Length];
            maxHN = 0;
            maxHC = 0;
            for (int i = 0; i < hidden.Length; i++) {
                if (i == 0) {
                    hiddenConnection[0] = new Connection(input, hidden[0]);
                } else {
                    hiddenConnection[i] = new Connection(hidden[i - 1], hidden[i]);
                }
                if (hidden[i].repition) {
                    repitionConnection[i] = new Connection(hidden[i], hidden[i]);
                } else {
                    repitionConnection[i] = null;
                }
                if (hidden[i].neuronAmount > maxHN) {
                    maxHN = hidden[i].neuronAmount;
                }
                if (hiddenConnection[i].connectionAmount > maxHC) {
                    maxHC = hiddenConnection[i].connectionAmount;
                }
            }
            outputConnection = new Connection(hidden[hidden.Length - 1], output); // considers the hidden layer as input and output layer as the output
            if (outputConnection.connectionAmount > maxHC) {
                maxHC = outputConnection.connectionAmount;
            }
        } else {
            maxHN = 0;
            maxHC = 0;
            outputConnection = new Connection(input, output);
            if (outputConnection.connectionAmount > maxHC) {
                maxHC = outputConnection.connectionAmount;
            }
        }
    }

    public Brain(BrainLayer _input, BrainLayer[] _hidden, BrainLayer _output) {
        input = _input;
        hidden = _hidden;
        output = _output;
        output.StartUp();
        if (hidden.Length > 0) {
            hiddenConnection = new Connection[hidden.Length];
            repitionConnection = new Connection[hiddenConnection];
            maxHC = 0;
            maxHN = 0;
            for (int i = 0; i < hidden.Length; i++) {
                if (i == 0) {
                    hiddenConnection[0] = new Connection(_input, hidden[0]);
                } else {
                    hiddenConnection[i] = new Connection(hidden[i - 1], hidden[i]);
                }
                hidden[i].StartUp();
                if (hidden[i].repition) {
                    repitionConnection[i] = new Connection(hidden[i], hidden[i]);
                } else {
                    repitionConnection[i] = null;
                }
                if (hidden[i].neuronAmount > maxHN) {
                    maxHN = hidden[i].neuronAmount;
                }
                if (hiddenConnection[i].connectionAmount > maxHC) {
                    maxHC = hiddenConnection[i].connectionAmount;
                }
            }
            outputConnection = new Connection(hidden[hidden.Length - 1], output);
            if (outputConnection.connectionAmount > maxHC) {
                maxHC = outputConnection.connectionAmount;
            }
        } else {
            maxHN = 0;
            maxHC = 0;
            outputConnection = new Connection(_input, _output);
            if (outputConnection.connectionAmount > maxHC) {
                maxHC = outputConnection.connectionAmount;
            }
        }
    }

    public void Run(BrainData data) {
        float[] _input = data.input;
        float[] _hidden = data.hidden;
        float[] _output = data.output;
        float[][] _repition = data.repition;
        int wIndex;
        int repitionWIndex;
        ActivateNeuronFormula activeNeuronFormula;
        if (hidden.Length > 0) {
            int lastNeuronAmount = 0;
            float[] wArray;
            float[] biasArray;
            float[] repitionWArray;
            for (int i = 0; i < hidden.Length; i++) {
                wArray = hiddenConnection[i].wArray;
                biasArray = hidden[i].biasArray;
                activeNeuronFormula = hidden[i].activeNeuronFormula;
                float[] io_;
                int io__;
                if (i == 0) {
                    io_ = _input;
                    io__ = _input.Length;
                } else {
                    io_ = _hidden;
                    io__ = lastNeuronAmount;
                }
                if (hidden[i].repition == true) {
                    repitionWArray = repitionConnection[i].wArray;
                    wIndex = 0;
                    repitionWIndex = 0;
                    float[] irep = _repition[i];
                    int k = biasArray.Length;
                    while (k-- > 0) {
                        float kbia = biasArray[k];
                        int j = io__;
                        while (j-- > 0) {
                            kbia += io_[j] * wArray[wIndex++];
                        }
                        j = irep.Length;
                        while (j-- > 0) {
                            kbia += irep[j] * repitionWArray[repitionWIndex++];
                        }
                        _hidden[k] = activeNeuronFormula(kbia);
                    }
                    Array.Copy(_hidden, irep, biasArray.Length);
                } else {
                    wIndex = 0;
                    int k = biasArray.Length;
                    while (k-- > 0) {
                        float kbia = biasArray[k];
                        int j = io__;
                        while (j-- > 0) {
                            kbia += io_ * wArray[wIndex++];
                        }
                        _hidden[k] = activeNeuronFormula(kbia);
                    }
                }
                lastNeuronAmount = biasArray.Length;
            }
            activeNeuronFormula = output.activeNeuronFormula;
            wArray = outputConnection.wArray;
            biasArray = output.biasArray;
            wIndex = 0;
            repitionWArray = 0;
            int i = _output.Length;
            while (i-- > 0) {
                float ibia = biasArray[i];
                int k = lastNeuronAmount;
                while (k-- > 0) {
                    ibia += _hidden[k] * wArray[wIndex++];
                }
                _output[i] = activeNeuronFormula(ibia);
            }
        } else {
            activeNeuronFormula = output.activeNeuronFormula;
            float[] wArray = outputConnection.wArray;
            float[] biasArray = output.biasArray;
            wIndex = 0;
            repitionWIndex = 0;
            int i = _output.Length;
            while (i-- > 0) {
                float ibia = biasArray[i];
                int k = _input.Length;
                while (k-- > 0) {
                    ibia += _input[k] * wArray[wIndex++];
                }
                _output[length] = activeNeuronFormula(ibia);
            }
        }
    }

    public void RunAndSaveData(BrainData data, CompleteData cData) {
        float[] _input = data.input;
        float[] _hidden = data.hidden;
        float[] _output = data.output;
        float[][] _repition = data.repition;
        int wIndex;
        int repitionWIndex;
        ActivateNeuronFormula activeNeuronFormula;
        if (hidden.Length > 0) {
            int lastNeuronAmount = 0;
            float[] wArray;
            float[] biasArray;
            float[] repitionWArray;
            for (int i = 0; i < hidden.Length; i++) {
                wArray = hiddenConnection[i].wArray;
                biasArray = hidden[i].biasArray;
                activeNeuronFormula = hidden[I].activeNeuronFormula;
                float[] io_;
                int io__;
                if (i == 0) {
                    io_ = input;
                    io__ = input.Length;
                } else {
                    io_ = _hidden;
                    io__ = lastNeuronAmount;
                }
                if (hidden[i].repition == true) {
                    float[] irep = _repition[i];
                    repitionWArray = repitionConnection[i].wArray;
                    Array.Copy(irep, cData.repitionBuffer[i], irep.Length);
                    wIndex = 0;
                    repitionWIndex = 0;
                    int k = biasArray.Length;
                    while (k-- > 0) {
                        float kbia = biasArray[k];
                        int j = io__;
                        while (j-- > 0) {
                            kbia += io_[j] * wArray[wIndex++];
                        }
                        j = irep.Length;
                        while (j-- > 0) {
                            kbia += irep[j] * repitionWArray[repitionWIndex++];
                        }
                        _hidden[k] = activeNeuronFormula(kbia);
                    }
                    Array.Copy(_hidden, irep, biasArray.Length);
                } else {
                    wIndex = 0;
                    int k = biasArray.Length;
                    while (k-- > 0) {
                        float kbia = biasArray[k];
                        int j = io__;
                        while (j-- > 0) {
                            kbia += io_[j] * wArray[wIndex++];
                        }
                        _hidden[k] = activeNeuronFormula(kbia);
                    }
                }
                Array.Copy(_hidden, cData.buffer[i], biasArray.Length);
                lastNeuronAmount = biasArray.Length;
            }
            activeNeuronFormula = output.ActivateNeuronFormula;
            wArray = outputConnection.wArray;
            biasArray = output.biasArray;
            wIndex = 0;
            repitionWIndex = 0;
            int i = _output.Length;
            while (i-- > 0) {
                float ibia = biasArray[i];
                int k = lastNeuronAmount;
                while (k-- > 0) {
                    ibia += _hidden[k] * wArray[wIndex++];
                }
                _output[i] = activeNeuronFormula(ibia);
            }
        } else {
            activeNeuronFormula = output.ActivateNeuronFormula;
            float[] wArray = outputConnection.wArray;
            float[] biasArray = output.biasArray;
            wIndex = 0;
            repitionWIndex = 0;
            int i = _output.Length;
            while (i-- > 0) {
                float ibia = biasArray[i];
                int k = _input.Length;
                while (k-- > 0) {
                    ibia += _input[k] * wArray[wIndex++];
                }
                _output[i] = activeNeuronFormula(ibia);
            }
        }
    }

}

}