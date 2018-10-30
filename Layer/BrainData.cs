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

public class BrainData {

    public float[] input;
    public float[] hidden;
    public float[] output;
    public float[][] repition;

    public void DataSetup(Brain brain) {
        // ...
    }

    public void DataReset(bool inputoutputreset = false) {
        if (inputoutputreset) {
            // somehow Fill the output and input data array
        }
        // fill the hidden data array
        for (int a = 0; a < repition.Length; a++) {
            if (repition[i] != null) {
                // fill the repition data array
            }
        }
    }

}

}