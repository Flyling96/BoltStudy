using System;
using System.Collections.Generic;
using DragonSlay;
using Ludiq;
using UnityEngine;

namespace Bolt.Extend
{
	[UnitCategory("Generator")]
    [CustomRutimeType]
    public sealed class GeneratorUnit : Unit, IGraphElementWithData, IGraphEventListener
	{

		public sealed class Data : IGraphElementData
        {

            public Delegate Update { get; set; }
            public bool IsListening { get; set; }
            public SceneObject Template { get; set; }
            public int Number { get; set; }
            public float Interval { get; set; }
            public Vector3 Position { get; set; }
            public Quaternion Rotation { get; set; }
            public bool Paused { get; set; }
            public float Time { get; set; }
            public List<SceneObject> List { get; set; }
            public int Index { get; set; }

		}

		[DoNotSerialize]
        [PortLabel("Start")]
        public ControlInput InputStart { get; private set; }

		[DoNotSerialize]
        [PortLabel("Pause")]
        public ControlInput InputPause { get; private set; }

        [DoNotSerialize]
        [PortLabel("Continue")]
        public ControlInput InputContinue { get; private set; }

        [DoNotSerialize]
        [PortLabel("Template")]
        public ValueInput InputTemplate { get; private set; }

        [DoNotSerialize]
        [PortLabel("Number")]
        public ValueInput InputNumber { get; private set; }

        [DoNotSerialize]
        [PortLabel("Interval")]
        public ValueInput InputInterval { get; private set; }

        [DoNotSerialize]
        [PortLabel("Position")]
        public ValueInput InputPosition { get; private set; }

        [DoNotSerialize]
        [PortLabel("Rotation")]
        public ValueInput InputRotation { get; private set; }

        [DoNotSerialize]
        [PortLabel("OnStart")]
        public ControlOutput OutputOnStart { get; private set; }

        [DoNotSerialize]
        [PortLabel("OnGenerate")]
        public ControlOutput OutputOnGenerate { get; private set; }

        [DoNotSerialize]
        [PortLabel("OnDestroy")]
        public ControlOutput OutputOnDestroy { get; private set; }

        [DoNotSerialize]
        [PortLabel("SceneObject")]
        public ValueOutput OutputSceneObject { get; private set; }

        [DoNotSerialize]
        [PortLabel("Count")]
        public ValueOutput OutputCount { get; private set; }

		protected override void Definition()
		{
			isControlRoot = true;

			InputStart = ControlInput(nameof(InputStart), Start);
			InputPause = ControlInput(nameof(InputPause), Pause);
            InputContinue = ControlInput(nameof(InputContinue), Continue);

            InputTemplate = ValueInput<SceneObject>(nameof(InputTemplate), null);
            InputNumber = ValueInput(nameof(InputNumber), 1);
            InputInterval = ValueInput(nameof(InputInterval), 0);
            InputPosition = ValueInput(nameof(InputPosition), Vector3.zero);
            InputRotation = ValueInput(nameof(InputRotation), Quaternion.identity);

            OutputOnStart = ControlOutput(nameof(OutputOnStart));
            OutputOnGenerate = ControlOutput(nameof(OutputOnGenerate));
            OutputOnDestroy = ControlOutput(nameof(OutputOnDestroy));

            OutputSceneObject = ValueOutput<SceneObject>(nameof(OutputSceneObject));
            OutputCount = ValueOutput<int>(nameof(OutputCount));
		}

		public IGraphElementData CreateData()
		{
			return new Data();
		}

		public void StartListening(GraphStack stack)
		{
            Data data = stack.GetElementData<Data>(this);

			if (data.IsListening)
			{
				return;
			}

            GraphReference reference = stack.ToReference();
            EventHook hook = new EventHook(EventHooks.Update, stack.machine);
			Action<EmptyEventArgs> update = args => TriggerUpdate(reference);
			EventBus.Register(hook, update);
			data.Update = update;
			data.IsListening = true;
		}

		public void StopListening(GraphStack stack)
		{
            Data data = stack.GetElementData<Data>(this);

			if (!data.IsListening)
			{
				return;
			}

            EventHook hook = new EventHook(EventHooks.Update, stack.machine);
			EventBus.Unregister(hook, data.Update);
			data.Update = null;
			data.IsListening = false;
		}
		
		public bool IsListening(GraphPointer pointer)
		{
			return pointer.GetElementData<Data>(this).IsListening;
		}

		private void TriggerUpdate(GraphReference reference)
        {
            using (Flow flow = Flow.New(reference))
            {
                Update(flow);
            }
        }

		private ControlOutput Start(Flow flow)
		{
            Data data = flow.stack.GetElementData<Data>(this);

            data.Number = flow.GetValue<int>(InputNumber);
            data.Interval = flow.GetValue<float>(InputInterval);
            data.Time = 0;
            data.Paused = false;
            data.List = new List<SceneObject>();
            data.Index = 0;

			return OutputOnStart;
		}

		private ControlOutput Pause(Flow flow)
		{
            Data data = flow.stack.GetElementData<Data>(this);

            data.Paused = true;

			return null;
        }

        private ControlOutput Continue(Flow flow)
        {
            Data data = flow.stack.GetElementData<Data>(this);

            data.Paused = failedToDefine;

            return null;
        }

        public void Update(Flow flow)
		{
            Data data = flow.stack.GetElementData<Data>(this);

			if (data.Paused)
			{
				return;
			}
			
			data.Time += Time.deltaTime;

            GraphStack stack = flow.PreserveStack();

			if (data.Time >= data.Interval && data.List.Count < data.Number)
			{
				flow.RestoreStack(stack);
                data.Time = 0;
                GenerateSceneObject(flow);
            }

			flow.DisposePreservedStack(stack);
		}

        private void GenerateSceneObject(Flow flow)
        {
            Data data = flow.stack.GetElementData<Data>(this);
            data.Template = flow.GetValue<SceneObject>(InputTemplate);
            data.Number = flow.GetValue<int>(InputNumber);
            data.Interval = flow.GetValue<float>(InputInterval);
            data.Position = flow.GetValue<Vector3>(InputPosition);
            data.Rotation = flow.GetValue<Quaternion>(InputRotation);
            SceneObject newSceneObject = SceneObject.CloneSceneObject(data.Template, data.Position, data.Rotation, data.Index);
            newSceneObject.Shell.gameObject.SetActive(true);
            GraphReference reference = flow.stack.ToReference();
            newSceneObject.Shell.OnDestroyEvent += OnDestroy;
            void OnDestroy()
            {
                Flow destroyFlow = Flow.New(reference);
                Debug.Log($"[_] {destroyFlow.stack.gameObject}");
                OnSceneObjectDestroy(destroyFlow, newSceneObject, true);
                newSceneObject.Shell.OnDestroyEvent -= OnDestroy;
            }
            data.List.Add(newSceneObject);
            data.Index++;
            flow.SetValue(OutputSceneObject, newSceneObject);
            flow.SetValue(OutputCount, data.List.Count);
            flow.Invoke(OutputOnGenerate);
        }

        private void OnSceneObjectDestroy(Flow flow, SceneObject sceneObject, bool restrict = false)
        {
            Debug.Log($"[_] {flow}, {flow?.stack}");
            Data data = flow.stack.GetElementData<Data>(this);
            if(data.List == null)
            {
                Debug.LogError($"[GeneratorUnit.OnSceneObjectDestroy] Generator not start yet, graph: {graph}, flow sponsor:{flow.stack.root}");
                return;
            }
            if(restrict && !data.List.Contains(sceneObject))
            {
                Debug.LogError($"[GeneratorUnit.OnSceneObjectDestroy] Generator do not contain the sceneObject, graph: {graph}, flow sponsor:{flow.stack.root}, sceneObject: {sceneObject}");
                return;
            }
            data.List.Remove(sceneObject);
            flow.SetValue(OutputCount, data.List.Count);
            flow.Invoke(OutputOnDestroy);
        }

	}
}