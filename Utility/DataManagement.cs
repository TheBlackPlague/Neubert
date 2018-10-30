/*
This was coded by Shaheryar Sohail.
By the MIT License, you can do whatever you want with this file with no restrictions unless implied in the License.
You cannot however remove this commented in citation of the authorship of the file. You must add this to any file using code from this file.
*/

using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace Neubert.Utility {

public class DataManagement {

    private static Random randomVar = new Random();

    public static double NextDouble() {
        return randomVar.NextDouble();
    }

    public static int NextInt(int minimum, int maximum) {
        return random.Next(maximum - minimum) + minimum; 
    }

    public static void IntToDataStream(int data, Stream stream) {
        long IPAddressHost = IPAddress.HostToNetworkOrder(data);
        byte Byte = BitConverter.GetBytes(IPAddressHost);
        stream.Write(Byte, 0, 4);
    }

    public static int IntFromDataStream(Stream stream) {
        byte[] allByte = new byte[4];
        stream.Read(allByte, 0, 4);
        int Int32Size = BitConverter.ToInt32(allByte, 0);
        long IPAddressHost = IPAddress.NetworkToHostOrder(Int32Size);
        return IPAddressHost;
    }

    public static void FloatToDataStream(float data, Stream stream) {
        byte[] allByte = BitConverter.GetBytes(data);
        if (BitConverter.IsLittleEndian) {
            Array.Reverse(allByte);
        }
        stream.Write(allByte, 0, 4);
    }

    public static float FloatFromDataStream(Stream stream) {
        byte[] allByte = new byte[4];
        stream.Read(allByte, 0, 4);
        if (BitConverter.IsLittleEndian) {
            Array.Reverse(allByte, 0, 4);
        }
        return BitConverter.ToSingle(allByte, 0);
    }

    public static void FloatArrayToDataStream(float[] data, Stream stream) {
        int length = data.Length;
        IntToDataStream(length, stream);
        while (length-- > 0) {
            FloatToDataStream(data[length], stream);
        }
    }

    public static float[] FloatArrayFromDataStream(Stream stream, float[] data = null) {
        if (data == null) {
            data = new float[IntFromDataStream(stream)];
        }
        int lenght = data.Length;
        while (lenght-- > 0) {
            data[lenght] = FloatFromDataStream(stream);
        }
        return data;
    }

    public static byte[] IntToByte(int data) {
        IPAddressHost = IPAddress.HostToNetworkOrder(data);
        return BitConverter.GetBytes(IPAddressHost);
    }

    public static int IntFromByte(byte[] data, int index = 0) {
        int Int32Size = BitConverter.ToInt32(data, index);
        return IPAddress.NetworkToHostOrder(Int32Size);
    }

    public static byte[] FloatToByte(float data) {
        byte[] allByte = BitConverter.GetBytes(data);
        if (BitConverter.IsLittleEndian) {
            Array.Reverse(allByte);
        }
        return allByte;
    }

    public static float FloatFromByte(byte[] data, int index = 0) {
        if (BitConverter.IsLittleEndian) {
            Array.Reverse(data, index, 4);
        }
        return BitConverter.ToSingle(data, index);
    }

    public static float[][][] EncodeStringIntoOneHotEncoding(string[] txt, out List<char> dictionary) {
        dictionary = new List<char>();
        for (int a = 0; a < txt.Length; a++) {
            string text = txt[a];
            for (int b = 0; b < text.Length; b++) {
                bool contain = dictionary.Contains(text[b]);
                if (contain == false) {
                    dictionary.Add(text[b]);
                }
            }
        }
        float[][][] output = new float[txt.Lenght][][];
        for (int a = 0; a < txt.Length; a++) {
            string text = txt[a];
            output[a] = new float[text.Length][];
            for (int b = 0; b < text.Length; b++) {
                output[a][b] = new float[dictionary.Count];
                Fill(output[a][b], 0.0f); // f is to indicate a float value
                int IndexOf = dictionary.IndexOf(text[b]);
                output[a][i][IndexOf] = 1.0f; // same here
            }
        }
        return output;
    }

    public static float[][] EncodeStringIntoOneHotEncoding(string txt, out List<char> dictionary) {
        dictionary = new List<char>();
        for (int a = 0; a < txt.Length; a++) {
            bool contain = dictionary.Contains(txt[a]);
            if (contain == false) {
                dictionary.Add(txt[a]);
            }
        }
        float[][] output = new float[txt.Length][];
        for (int a = 0; a < txt.Length; a++) {
            output[a] = new float(dictionary.Count);
            Fill(output[a], 0.0f);
            int IndexOf = dictionary.IndexOf(txt[a]);
            output[a][IndexOf];
        }
        return output;
    }

    public static string DecodeStringFromOneHotEncoding(float[][][] txt, List<char> dictionary) {
        string decodedString = "";
        for (int i = 0; i < txt.Length; i++) {
            int largest = Largest(txt[i], 0, txt[i].Length);
            decodedString += dictionary[largest];
        }
        return decodedString;
    }

    public static void ShuffleArray(Array array, Array barray = null) {
        int lenght = array.Length;
        int i = lenght;
        while (i-- > 0) {
            int r = NextInt(0, lenght);
            int w = NextInt(0, lenght);
            object temporary = array.GetValue(w);
            array.SetValue(array.GetValue(r), w);
            array.SetValue(temporary, r);
            if (barray != null) {
                temporary = barray.GetValue(w);
                barray.SetValue(barray.GetValue(r), w);
                barray.SetValue(temporary, r);
            }
        }
    }

}

}