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

public class Connection {

    public static float minimumW = 0.0f;
    public static float maximumW = 0.06f;
    public int connectionAmount;
    public float[] wArray;

    public Connection(Connection data) {
        connectionAmount = data.connectionAmount;
        wArray = data.wArray;
    }

    public Connection(BrainLayer input, BrainLayer output) {
        connectionAmount = input.neuronAmount * output.neuronAmount;
        wArray = new float[connectionAmount];
    }

    public void RandomizeWArray() {
        int i = connectionAmount;
        while (i-- > 0) {
            wArray[i] = (float)DataManagement.NextDouble() * (maximumW - minimumW) + minimumW;
        }
    }

    public void CopyWArrayFromAnotherConnection(Connection data) {
        float[] _wArray = data.wArray;
        int i = connectionAmount;
        while (i-- > 0) {
            wArray[i] = _wArray[i];
        }
    }

    public void WMutation(float mutationChance) {
        int i = connectionAmount;
        while (i-- > 0) {
            if ((float)DataManagement.NextDouble() <= mutationChance) {
                wArray[i] = (float)DataManagement.NextDouble() * (maximumW - minimumW) + minimumW;
            }
        }
    }

    public void WBreeding(Connection data) {
        int i = connectionAmount;
        while (i-- > 0) {
            float value = (float)DataManagement.NextDouble();
            wArray[i] = wArray[i] * value + data.wArray[i] * (1.0f - value);
        }
    }

    public void Save(Stream stream) {
        int i = connectionAmount;
        while (i-- > 0) {
            s.Write(DataManagement.FloatToByte(wArray[i]), 0, 4);
        }
    }

    public void Load(Stream stream) {
        byte[] allByte = new byte[4];
        int i = connectionAmount;
        while (i-- > 0) {
            s.Read(allByte, 0, 4);
            wArray[i] = DataManagement.FloatFromByte(allByte);
        }
    }



}

}