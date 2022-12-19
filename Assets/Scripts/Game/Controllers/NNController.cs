using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

namespace SortGame
{
    /// <summary>
    /// Controller base class for neural network-based AI controllers.
    /// </summary>
    public abstract class NNController : AIController
    {
        [SerializeField] NNModel modelAsset;
        #if UNITY_EDITOR
        [SerializeField] bool verbose = true;
        #endif
        [SerializeField] WorkerFactory.Type inferenceBackend;
        private Model runtimeModel;
        private IWorker worker;
        private readonly Dictionary<string, Tensor> inputBuffer = new();
        protected override sealed void AIInit()
        {
            // Load NN model from asset.
            runtimeModel = ModelLoader.Load(modelAsset, verbose: verbose);
            // Create the async thread that runs inference for us.
            worker = WorkerFactory.CreateWorker(inferenceBackend, runtimeModel);
            // Invoke the neural network's init method.
            NNInit();
        }
        /// <summary>
        /// Invoked just before the core loop starts.
        /// </summary>
        protected abstract void NNInit();
        private void OnDestroy() 
        {
            // Clean up memory allocated via Barracuda.
            worker?.Dispose();
            foreach(Tensor tensor in inputBuffer.Values) 
                tensor.Dispose();    
        }
        /// <summary>
        /// Abstract method to implement the details of assigning input tensors.
        /// </summary>
        protected abstract void GetInputTensors(in Dictionary<string, Tensor> inputs);
        protected override sealed IEnumerator OnAction()
        {
            // Retrieve input tensors from NN.
            GetInputTensors(in inputBuffer);
            // The NN starts executing asynchronously.
            worker.Execute(inputBuffer);
            // This gives us a handle to the output tensor, but it won't be ready right away.
            var modelOutput = worker.PeekOutput();
            // Wait for the output tensor to be complete, and then forward it to ExecuteModelOutput.
            yield return new WaitForCompletion(modelOutput);
            yield return ExecuteModelOutput(in modelOutput);
        }
        protected abstract IEnumerator ExecuteModelOutput(in Tensor modelOutput);
    }
}
