//using MNIST.IO;
using System;
using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.OpenCL;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using System.Diagnostics;

namespace MinRun
{
    public class Program
    {
        //Hardcoded for this minimal version
        public static float[] HardcodedInputs => new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.011764706f, 0.07058824f, 0.07058824f, 0.07058824f, 0.49411765f, 0.53333336f, 0.6862745f, 0.101960786f, 0.6509804f, 1f, 0.96862745f, 0.49803922f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.11764706f, 0.14117648f, 0.36862746f, 0.6039216f, 0.6666667f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.88235295f, 0.6745098f, 0.99215686f, 0.9490196f, 0.7647059f, 0.2509804f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.19215687f, 0.93333334f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.9843137f, 0.3647059f, 0.32156864f, 0.32156864f, 0.21960784f, 0.15294118f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.07058824f, 0.85882354f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.7764706f, 0.7137255f, 0.96862745f, 0.94509804f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.3137255f, 0.6117647f, 0.41960785f, 0.99215686f, 0.99215686f, 0.8039216f, 0.043137256f, 0f, 0.16862746f, 0.6039216f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.05490196f, 0.003921569f, 0.6039216f, 0.99215686f, 0.3529412f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.54509807f, 0.99215686f, 0.74509805f, 0.007843138f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.043137256f, 0.74509805f, 0.99215686f, 0.27450982f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.13725491f, 0.94509804f, 0.88235295f, 0.627451f, 0.42352942f, 0.003921569f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.31764707f, 0.9411765f, 0.99215686f, 0.99215686f, 0.46666667f, 0.09803922f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.1764706f, 0.7294118f, 0.99215686f, 0.99215686f, 0.5882353f, 0.105882354f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.0627451f, 0.3647059f, 0.9882353f, 0.99215686f, 0.73333335f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.9764706f, 0.99215686f, 0.9764706f, 0.2509804f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.18039216f, 0.50980395f, 0.7176471f, 0.99215686f, 0.99215686f, 0.8117647f, 0.007843138f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.15294118f, 0.5803922f, 0.8980392f, 0.99215686f, 0.99215686f, 0.99215686f, 0.98039216f, 0.7137255f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.09411765f, 0.44705883f, 0.8666667f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.7882353f, 0.30588236f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.09019608f, 0.25882354f, 0.8352941f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.7764706f, 0.31764707f, 0.007843138f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.07058824f, 0.67058825f, 0.85882354f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.7647059f, 0.3137255f, 0.03529412f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.21568628f, 0.6745098f, 0.8862745f, 0.99215686f, 0.99215686f, 0.99215686f, 0.99215686f, 0.95686275f, 0.52156866f, 0.043137256f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.53333336f, 0.99215686f, 0.99215686f, 0.99215686f, 0.83137256f, 0.5294118f, 0.5176471f, 0.0627451f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };

        public static void Main(string[] args)
        {
            NeuralNetworkGPU nn = new NeuralNetworkGPU(new int[] { 784, 400, 150, 60, 10 });

            nn.TestCompute(HardcodedInputs, out float[] output);
        }
    }
    public static class Utils2
    {
        public static Random R { get; set; } = new Random(0);
        public static float RandomStartValue() => R.NextSingle() - 0.5f;       // -0.5f to 0.499999f
    }
    public class NeuralNetworkGPU
    {

        public int NumberOfLayers { get; set; } = 0;
        public int[] LayerSizes { get; set; }
        public float[][,] Weights { get; set; }
        public float[][] Biases { get; set; }
        public float[][] Nodes { get; set; }

        public static float[] Check => new float[] { -2.50375366f, 1.53575432f, -4.74363947f, -6.23493338f, -1.9260366f, -1.04151368f, -4.87213469f, -2.173594f, -1.5570873f, 1.68777406f, -1.71339285f, 4.43441725f, 0.366425931f, -3.19932175f, 0.0813432857f, -1.3154f, -1.81602979f, -5.097834f, 3.14737058f, 4.42167759f, 2.76737666f, 0.7676046f, -4.740689f, -4.792055f, -5.11894131f, 0.4057201f, 3.590598f };


        public NeuralNetworkGPU(int[] networkShape, int seed = 0)
        {
            Utils2.R = new Random(seed);

            NumberOfLayers = networkShape.Length - 1;

            Weights = new float[NumberOfLayers][,];
            Biases = new float[NumberOfLayers][];
            Nodes = new float[NumberOfLayers][];
            LayerSizes = new int[NumberOfLayers];

            for (int layerIndex = 0; layerIndex < NumberOfLayers; layerIndex++)
            {
                Weights[layerIndex] = new float[networkShape[layerIndex + 1], networkShape[layerIndex]];
                Biases[layerIndex] = new float[networkShape[layerIndex]];
                Nodes[layerIndex] = new float[networkShape[layerIndex + 1]];
                LayerSizes[layerIndex] = networkShape[layerIndex + 1];

                for (int i = 0; i < networkShape[layerIndex]; i++)
                {
                    for (int j = 0; j < networkShape[layerIndex + 1]; j++)
                    {
                        Weights[layerIndex][j, i] = Utils2.RandomStartValue();
                    }
                    Biases[layerIndex][i] = Utils2.RandomStartValue();
                }
            }
        }
        public bool TestCompute(float[] inputs, out float[] output)
        {
            // Initialize ILGPU.

            using Context context = Context.Create(builder => builder.CPU().Cuda().
                                            //Math(MathMode.Fast).
                                            Inlining(InliningMode.Aggressive).
                                            Optimize(OptimizationLevel.O1));

            // this just gets the "best" GPU, or the CPU if none exists
            using Accelerator a = context.GetPreferredDevice(preferCPU: false).CreateAccelerator(context);

            for (int layerIndex = 0; layerIndex < LayerSizes.Length; layerIndex++)
            {
                Nodes[layerIndex] = new float[LayerSizes[layerIndex]];

                RunGPUForward(layerIndex, layerIndex == 0 ? inputs : Nodes[layerIndex - 1], out Nodes[layerIndex], a);
            }

            Console.WriteLine("On GPU :");

            for (int i = 0; i < Check.Length; i++)
            {
                Console.WriteLine($"{i} : {/*Check[i] == */Nodes[0][i]}");
            }

            using Accelerator a2 = context.CreateCPUAccelerator(0);

            for (int layerIndex = 0; layerIndex < LayerSizes.Length; layerIndex++)
            {
                Nodes[layerIndex] = new float[LayerSizes[layerIndex]];
                RunGPUForward(layerIndex, layerIndex == 0 ? inputs : Nodes[layerIndex - 1], out Nodes[layerIndex], a2);
            }

            Console.WriteLine("\nOn CPU :");
            for (int i = 0; i < Check.Length; i++)
            {
                Console.WriteLine($"{i} : {/*Check[i] == */Nodes[0][i]}");
            }

            Console.WriteLine("\nExpected was :");
            for (int i = 0; i < Check.Length; i++)
            {
                Console.WriteLine($"{i} : {Check[i]}");
            }

            output = null;
            return true;
        }
        public void RunGPUForward(int layerIndex, float[] inputs, out float[] output, Accelerator a)
        {
            /*Parallel.For(0f, LayerSizes[layerIndex], i =>
            {
                // sum weights times inputs
                for (int j = 0; j < inputs.Length; j++)
                {
                    Nodes[layerIndex][i] += Weights[layerIndex][i, j] * inputs[j];
                }
            });*/

            MemoryBuffer2D<float, Stride2D.DenseX> gpuW = a.Allocate2DDenseX(Weights[layerIndex]);
            MemoryBuffer1D<float, Stride1D.Dense> gpuI = a.Allocate1D(inputs);
            MemoryBuffer1D<float, Stride1D.Dense> gpuO = a.Allocate1D(Nodes[layerIndex]);
            gpuO.MemSetToZero();

            var kernel = a.LoadAutoGroupedStreamKernel((Index2D i, ArrayView1D<float, Stride1D.Dense> inputs, ArrayView2D<float, Stride2D.DenseX> w, ArrayView1D<float, Stride1D.Dense> o) =>
            {
                o[i.X] += inputs[i.Y] * w[i];
            });

            Index2D index = new Index2D(Weights[layerIndex].GetLength(0), Weights[layerIndex].GetLength(1));

            kernel(index, gpuI, gpuW, gpuO);

            a.Synchronize();
            output = gpuO.GetAsArray1D();
        }
    }
}