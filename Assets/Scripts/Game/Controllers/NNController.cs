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
        protected override void Init()
        {
            runtimeModel = ModelLoader.Load(modelAsset, verbose: verbose);
            worker = WorkerFactory.CreateWorker(inferenceBackend, runtimeModel);
        }
        protected abstract void GetInputTensors(in Dictionary<string, Tensor> inputs);
        protected override sealed IEnumerator OnAction()
        {
            GetInputTensors(in inputBuffer);
            worker.Execute(inputBuffer);
            var modelOutput = worker.PeekOutput();
            yield return new WaitForCompletion(modelOutput);
            yield return ExecuteModelOutput(in modelOutput);
        }
        protected abstract IEnumerator ExecuteModelOutput(in Tensor modelOutput);
    }
}
