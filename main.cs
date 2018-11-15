using System;
using System.IO;
using System.Collections.Generic;

#pragma warning disable 0649
namespace AI
{
    class App
    {
        public static List<decimal[]> inputs = new List<decimal[]>();
        
        public static decimal[] outputs = new decimal[] { 1, 0, 0, 1, 1, 0, 0, 1 };

        public static void Main(string[] args)
        {
            inputs.Add(new decimal[] { 0, 1, 0 });
            inputs.Add(new decimal[] { 1, 1, 0 });
            inputs.Add(new decimal[] { 1, 1, 1 });
            inputs.Add(new decimal[] { 0, 1, 1 });
            inputs.Add(new decimal[] { 0, 0, 1 });
            inputs.Add(new decimal[] { 1, 0, 1 });
            inputs.Add(new decimal[] { 1, 0, 0 });
            inputs.Add(new decimal[] { 0, 0, 0 });
            network.applyWeights(1, 1, 2, 3);
            List<decimal> os = new List<decimal>();
            List<decimal> osa = new List<decimal>();
            os.Add(network.run(inputs[0]));
            os.Add(network.run(inputs[1]));
            os.Add(network.run(inputs[2]));
            os.Add(network.run(inputs[3]));
            osa.Add(-os[0]);
            osa.Add(-os[1]);
            osa.Add(-os[2]);
            osa.Add(-os[3]);
            Console.WriteLine("Output1: " + os[0]);
            Console.WriteLine("Output2: " + os[1]);
            Console.WriteLine("Output3: " + os[2]);
            Console.WriteLine("Output4: " + os[3]);
            network.train( inputs,  outputs, 1000);

            Console.WriteLine("--------- After Training -------------");
            os = new List<decimal>();
            os.Add(network.run(inputs[0]));
            os.Add(network.run(inputs[1]));
            os.Add(network.run(inputs[2]));
            os.Add(network.run(inputs[3]));
            osa[0] += os[0];
            osa[1] += os[1];
            osa[2] += os[2];
            osa[3] += os[3];

            Console.WriteLine("Output1: " + os[0]);
            Console.WriteLine("Output2: " + os[1]);
            Console.WriteLine("Output3: " + os[2]);
            Console.WriteLine("Output4: " + os[3]);
            Console.WriteLine("Output Change 1: " + osa[0]);
            Console.WriteLine("Output Change 2: " + osa[1]);
            Console.WriteLine("Output Change 3: " + osa[2]);
            Console.WriteLine("Output Change 4: " + osa[3]);
            
            Console.WriteLine("Equivalent To: "+ Math.Round(os[0]));
            Console.WriteLine("Equivalent To: " + Math.Round(os[1]));
            Console.WriteLine("Equivalent To: " + Math.Round(os[2]));
            Console.WriteLine("Equivalent To: " + Math.Round(os[3]));
            while(true){
                Console.Read();
                Console.Clear();
                network.train(inputs, outputs, 100);
                os = new List<decimal>();
                os.Add(network.run(inputs[0]));
                os.Add(network.run(inputs[1]));
                os.Add(network.run(inputs[2]));
                os.Add(network.run(inputs[3]));
                Console.WriteLine("Output1: " + os[0]);
                Console.WriteLine("Output2: " + os[1]);
                Console.WriteLine("Output3: " + os[2]);
                Console.WriteLine("Output4: " + os[3]);
            }
        }
    }
    static class network
    {
        public static List<List<List<decimal>>> weights = new List<List<List<decimal>>>();
        public static List<List<decimal>> values = new List<List<decimal>>();
        public static List<decimal> _temp = new List<decimal>();

        public static decimal _output;
        public static decimal _error;

        public static void train(List<decimal[]> inputs, decimal[] outputs, int iterations)
        {
            for (int j = 0; j < iterations; j++) {
                for (int q = 0; q < inputs.Count; q++)
                {
                    _output=(run(inputs[q]));
                    _error=(outputs[q]-_output);
                    _error*=d_sigmoid(_output);
                   
                    weights[weights.Count-1][0][0] += values[0][0] * _error;
                    weights[weights.Count-1][0][1] += values[0][1] * _error;
                    for (int b = 0; b < weights[0].Count; b++)
                    {
                        for (int i = 0; i < weights[0][b].Count; i++)
                        {
                            weights[0][b][i] += inputs[q][i] * weights[1][0][b] * _error;
                        }    
                    }
                }
                
                //Console.WriteLine("After: "+weights0[0]);
            }

        }
        public static decimal run(decimal[] inputs)
        {
            values = new List<List<decimal>>();

            values.Add(new List<decimal>());
            decimal value = 0;
            for (int i = 0; i < inputs.Length; i++) {
                value += weights[0][0][i] * inputs[i];
            }
            values[0].Add(value);

            value = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                value += weights[0][1][i] * inputs[i];
            }
            values[0].Add(value);

            value = 0;
            value += weights[1][0][0] * values[0][0];
            value += weights[1][0][1] * values[0][1];
            return sigmoid(value);
        }
        public static void applyWeights(int numOuts, int hiddenLayers, int width, int numIns) {
            for (int q = -1; q < hiddenLayers; q++) {
                weights.Add(new List<List<decimal>>());
            }
            
            weights[1].Add(new List<decimal>());

            Random r = new Random();
            for (int q = 0; q < width; q++) {
                weights[0].Add(new List<decimal>());
                for (int i = 0; i < numIns; i++) {
                    weights[0][q].Add((decimal)r.NextDouble() * 2 - 1);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                weights[1][0].Add((decimal)r.NextDouble() * 2 - 1);
            }
        }
        public static decimal sigmoid(decimal x)
        {
            return (decimal)(1 / (1 + Math.Pow(Math.E, (double)-x)));
        }
        public static decimal r_sigmoid(decimal x)
        {
            return (decimal)(Math.Log((double)(1/((1/x)-1))));
        }
        public static decimal d_sigmoid(decimal x)
        {
            return (x) * (1 - (x));
        }
    }
}
