//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bolt.Extend
{
	
	
	public class AutoBinaryUnits
	{
		
		public static Bolt.Unit GetUnit(string unitName)
		{
			switch (unitName)
			{
				case nameof(TestUnit) : return new TestUnit();
				case nameof(GeneratorUnit) : return new GeneratorUnit();
				case nameof(ITransformFuncUnit) : return new ITransformFuncUnit();
				case nameof(SceneObjectUnit) : return new SceneObjectUnit();
				case nameof(SubFlowUnit) : return new SubFlowUnit();
				case nameof(LogWarning) : return new LogWarning();
				case nameof(RandomRange) : return new RandomRange();
				case nameof(Vector3X) : return new Vector3X();
				case nameof(Vector3Y) : return new Vector3Y();
				case nameof(Vector3Z) : return new Vector3Z();
				case nameof(Vector3New) : return new Vector3New();
				case nameof(Vector3XSet) : return new Vector3XSet();
				case nameof(CreateStruct) : return new CreateStruct();
				case nameof(Expose) : return new Expose();
				case nameof(GetMember) : return new GetMember();
				case nameof(InvokeMember) : return new InvokeMember();
				case nameof(SetMember) : return new SetMember();
				case nameof(CountItems) : return new CountItems();
				case nameof(AddDictionaryItem) : return new AddDictionaryItem();
				case nameof(ClearDictionary) : return new ClearDictionary();
				case nameof(CreateDictionary) : return new CreateDictionary();
				case nameof(DictionaryContainsKey) : return new DictionaryContainsKey();
				case nameof(GetDictionaryItem) : return new GetDictionaryItem();
				case nameof(MergeDictionaries) : return new MergeDictionaries();
				case nameof(RemoveDictionaryItem) : return new RemoveDictionaryItem();
				case nameof(SetDictionaryItem) : return new SetDictionaryItem();
				case nameof(FirstItem) : return new FirstItem();
				case nameof(LastItem) : return new LastItem();
				case nameof(AddListItem) : return new AddListItem();
				case nameof(ClearList) : return new ClearList();
				case nameof(CreateList) : return new CreateList();
				case nameof(GetListItem) : return new GetListItem();
				case nameof(InsertListItem) : return new InsertListItem();
				case nameof(ListContainsItem) : return new ListContainsItem();
				case nameof(MergeLists) : return new MergeLists();
				case nameof(RemoveListItem) : return new RemoveListItem();
				case nameof(RemoveListItemAt) : return new RemoveListItemAt();
				case nameof(SetListItem) : return new SetListItem();
				case nameof(Branch) : return new Branch();
				case nameof(Break) : return new Break();
				case nameof(Cache) : return new Cache();
				case nameof(For) : return new For();
				case nameof(ForEach) : return new ForEach();
				case nameof(Once) : return new Once();
				case nameof(SelectOnEnum) : return new SelectOnEnum();
				case nameof(SelectOnFlow) : return new SelectOnFlow();
				case nameof(SelectOnInteger) : return new SelectOnInteger();
				case nameof(SelectOnString) : return new SelectOnString();
				case nameof(SelectUnit) : return new SelectUnit();
				case nameof(Sequence) : return new Sequence();
				case nameof(SwitchOnEnum) : return new SwitchOnEnum();
				case nameof(SwitchOnInteger) : return new SwitchOnInteger();
				case nameof(SwitchOnString) : return new SwitchOnString();
				case nameof(Throw) : return new Throw();
				case nameof(ToggleFlow) : return new ToggleFlow();
				case nameof(ToggleValue) : return new ToggleValue();
				case nameof(TryCatch) : return new TryCatch();
				case nameof(While) : return new While();
				case nameof(BoltAnimationEvent) : return new BoltAnimationEvent();
				case nameof(BoltNamedAnimationEvent) : return new BoltNamedAnimationEvent();
				case nameof(OnAnimatorIK) : return new OnAnimatorIK();
				case nameof(OnAnimatorMove) : return new OnAnimatorMove();
				case nameof(OnApplicationFocus) : return new OnApplicationFocus();
				case nameof(OnApplicationLostFocus) : return new OnApplicationLostFocus();
				case nameof(OnApplicationPause) : return new OnApplicationPause();
				case nameof(OnApplicationQuit) : return new OnApplicationQuit();
				case nameof(OnApplicationResume) : return new OnApplicationResume();
				case nameof(BoltUnityEvent) : return new BoltUnityEvent();
				case nameof(CustomEvent) : return new CustomEvent();
				case nameof(OnDrawGizmos) : return new OnDrawGizmos();
				case nameof(OnDrawGizmosSelected) : return new OnDrawGizmosSelected();
				case nameof(OnBeginDrag) : return new OnBeginDrag();
				case nameof(OnButtonClick) : return new OnButtonClick();
				case nameof(OnCancel) : return new OnCancel();
				case nameof(OnDeselect) : return new OnDeselect();
				case nameof(OnDrag) : return new OnDrag();
				case nameof(OnDrop) : return new OnDrop();
				case nameof(OnDropdownValueChanged) : return new OnDropdownValueChanged();
				case nameof(OnEndDrag) : return new OnEndDrag();
				case nameof(OnGUI) : return new OnGUI();
				case nameof(OnInputFieldEndEdit) : return new OnInputFieldEndEdit();
				case nameof(OnInputFieldValueChanged) : return new OnInputFieldValueChanged();
				case nameof(OnMove) : return new OnMove();
				case nameof(OnPointerClick) : return new OnPointerClick();
				case nameof(OnPointerDown) : return new OnPointerDown();
				case nameof(OnPointerEnter) : return new OnPointerEnter();
				case nameof(OnPointerExit) : return new OnPointerExit();
				case nameof(OnPointerUp) : return new OnPointerUp();
				case nameof(OnScroll) : return new OnScroll();
				case nameof(OnScrollRectValueChanged) : return new OnScrollRectValueChanged();
				case nameof(OnScrollbarValueChanged) : return new OnScrollbarValueChanged();
				case nameof(OnSelect) : return new OnSelect();
				case nameof(OnSliderValueChanged) : return new OnSliderValueChanged();
				case nameof(OnSubmit) : return new OnSubmit();
				case nameof(OnToggleValueChanged) : return new OnToggleValueChanged();
				case nameof(OnTransformChildrenChanged) : return new OnTransformChildrenChanged();
				case nameof(OnTransformParentChanged) : return new OnTransformParentChanged();
				case nameof(OnButtonInput) : return new OnButtonInput();
				case nameof(OnKeyboardInput) : return new OnKeyboardInput();
				case nameof(OnMouseDown) : return new OnMouseDown();
				case nameof(OnMouseDrag) : return new OnMouseDrag();
				case nameof(OnMouseEnter) : return new OnMouseEnter();
				case nameof(OnMouseExit) : return new OnMouseExit();
				case nameof(OnMouseInput) : return new OnMouseInput();
				case nameof(OnMouseOver) : return new OnMouseOver();
				case nameof(OnMouseUp) : return new OnMouseUp();
				case nameof(OnMouseUpAsButton) : return new OnMouseUpAsButton();
				case nameof(FixedUpdate) : return new FixedUpdate();
				case nameof(LateUpdate) : return new LateUpdate();
				case nameof(OnDestroy) : return new OnDestroy();
				case nameof(OnDisable) : return new OnDisable();
				case nameof(OnEnable) : return new OnEnable();
				case nameof(Start) : return new Start();
				case nameof(Update) : return new Update();
				case nameof(OnDestinationReached) : return new OnDestinationReached();
				case nameof(OnCollisionEnter2D) : return new OnCollisionEnter2D();
				case nameof(OnCollisionExit2D) : return new OnCollisionExit2D();
				case nameof(OnCollisionStay2D) : return new OnCollisionStay2D();
				case nameof(OnJointBreak2D) : return new OnJointBreak2D();
				case nameof(OnTriggerEnter2D) : return new OnTriggerEnter2D();
				case nameof(OnTriggerExit2D) : return new OnTriggerExit2D();
				case nameof(OnTriggerStay2D) : return new OnTriggerStay2D();
				case nameof(OnCollisionEnter) : return new OnCollisionEnter();
				case nameof(OnCollisionExit) : return new OnCollisionExit();
				case nameof(OnCollisionStay) : return new OnCollisionStay();
				case nameof(OnControllerColliderHit) : return new OnControllerColliderHit();
				case nameof(OnJointBreak) : return new OnJointBreak();
				case nameof(OnParticleCollision) : return new OnParticleCollision();
				case nameof(OnTriggerEnter) : return new OnTriggerEnter();
				case nameof(OnTriggerExit) : return new OnTriggerExit();
				case nameof(OnTriggerStay) : return new OnTriggerStay();
				case nameof(OnBecameInvisible) : return new OnBecameInvisible();
				case nameof(OnBecameVisible) : return new OnBecameVisible();
				case nameof(OnTimerElapsed) : return new OnTimerElapsed();
				case nameof(TriggerCustomEvent) : return new TriggerCustomEvent();
				case nameof(Formula) : return new Formula();
				case nameof(Literal) : return new Literal();
				case nameof(And) : return new And();
				case nameof(ApproximatelyEqual) : return new ApproximatelyEqual();
				case nameof(Comparison) : return new Comparison();
				case nameof(Equal) : return new Equal();
				case nameof(EqualityComparison) : return new EqualityComparison();
				case nameof(ExclusiveOr) : return new ExclusiveOr();
				case nameof(Greater) : return new Greater();
				case nameof(GreaterOrEqual) : return new GreaterOrEqual();
				case nameof(Less) : return new Less();
				case nameof(LessOrEqual) : return new LessOrEqual();
				case nameof(Negate) : return new Negate();
				case nameof(NotApproximatelyEqual) : return new NotApproximatelyEqual();
				case nameof(NotEqual) : return new NotEqual();
				case nameof(NumericComparison) : return new NumericComparison();
				case nameof(Or) : return new Or();
				case nameof(GenericAdd) : return new GenericAdd();
				case nameof(GenericDivide) : return new GenericDivide();
				case nameof(GenericModulo) : return new GenericModulo();
				case nameof(GenericMultiply) : return new GenericMultiply();
				case nameof(GenericSubtract) : return new GenericSubtract();
				case nameof(ScalarAbsolute) : return new ScalarAbsolute();
				case nameof(ScalarAdd) : return new ScalarAdd();
				case nameof(ScalarAverage) : return new ScalarAverage();
				case nameof(ScalarDivide) : return new ScalarDivide();
				case nameof(ScalarExponentiate) : return new ScalarExponentiate();
				case nameof(ScalarLerp) : return new ScalarLerp();
				case nameof(ScalarMaximum) : return new ScalarMaximum();
				case nameof(ScalarMinimum) : return new ScalarMinimum();
				case nameof(ScalarModulo) : return new ScalarModulo();
				case nameof(ScalarMoveTowards) : return new ScalarMoveTowards();
				case nameof(ScalarMultiply) : return new ScalarMultiply();
				case nameof(ScalarNormalize) : return new ScalarNormalize();
				case nameof(ScalarPerSecond) : return new ScalarPerSecond();
				case nameof(ScalarRoot) : return new ScalarRoot();
				case nameof(ScalarRound) : return new ScalarRound();
				case nameof(ScalarSubtract) : return new ScalarSubtract();
				case nameof(ScalarSum) : return new ScalarSum();
				case nameof(Vector2Absolute) : return new Vector2Absolute();
				case nameof(Vector2Add) : return new Vector2Add();
				case nameof(Vector2Angle) : return new Vector2Angle();
				case nameof(Vector2Average) : return new Vector2Average();
				case nameof(Vector2Distance) : return new Vector2Distance();
				case nameof(Vector2Divide) : return new Vector2Divide();
				case nameof(Vector2DotProduct) : return new Vector2DotProduct();
				case nameof(Vector2Lerp) : return new Vector2Lerp();
				case nameof(Vector2Maximum) : return new Vector2Maximum();
				case nameof(Vector2Minimum) : return new Vector2Minimum();
				case nameof(Vector2Modulo) : return new Vector2Modulo();
				case nameof(Vector2MoveTowards) : return new Vector2MoveTowards();
				case nameof(Vector2Multiply) : return new Vector2Multiply();
				case nameof(Vector2Normalize) : return new Vector2Normalize();
				case nameof(Vector2PerSecond) : return new Vector2PerSecond();
				case nameof(Vector2Project) : return new Vector2Project();
				case nameof(Vector2Round) : return new Vector2Round();
				case nameof(Vector2Subtract) : return new Vector2Subtract();
				case nameof(Vector2Sum) : return new Vector2Sum();
				case nameof(Vector3Absolute) : return new Vector3Absolute();
				case nameof(Vector3Add) : return new Vector3Add();
				case nameof(Vector3Angle) : return new Vector3Angle();
				case nameof(Vector3Average) : return new Vector3Average();
				case nameof(Vector3CrossProduct) : return new Vector3CrossProduct();
				case nameof(Vector3Distance) : return new Vector3Distance();
				case nameof(Vector3Divide) : return new Vector3Divide();
				case nameof(Vector3DotProduct) : return new Vector3DotProduct();
				case nameof(Vector3Lerp) : return new Vector3Lerp();
				case nameof(Vector3Maximum) : return new Vector3Maximum();
				case nameof(Vector3Minimum) : return new Vector3Minimum();
				case nameof(Vector3Modulo) : return new Vector3Modulo();
				case nameof(Vector3MoveTowards) : return new Vector3MoveTowards();
				case nameof(Vector3Multiply) : return new Vector3Multiply();
				case nameof(Vector3Normalize) : return new Vector3Normalize();
				case nameof(Vector3PerSecond) : return new Vector3PerSecond();
				case nameof(Vector3Project) : return new Vector3Project();
				case nameof(Vector3Round) : return new Vector3Round();
				case nameof(Vector3Subtract) : return new Vector3Subtract();
				case nameof(Vector3Sum) : return new Vector3Sum();
				case nameof(Vector4Absolute) : return new Vector4Absolute();
				case nameof(Vector4Add) : return new Vector4Add();
				case nameof(Vector4Average) : return new Vector4Average();
				case nameof(Vector4Distance) : return new Vector4Distance();
				case nameof(Vector4Divide) : return new Vector4Divide();
				case nameof(Vector4DotProduct) : return new Vector4DotProduct();
				case nameof(Vector4Lerp) : return new Vector4Lerp();
				case nameof(Vector4Maximum) : return new Vector4Maximum();
				case nameof(Vector4Minimum) : return new Vector4Minimum();
				case nameof(Vector4Modulo) : return new Vector4Modulo();
				case nameof(Vector4MoveTowards) : return new Vector4MoveTowards();
				case nameof(Vector4Multiply) : return new Vector4Multiply();
				case nameof(Vector4Normalize) : return new Vector4Normalize();
				case nameof(Vector4PerSecond) : return new Vector4PerSecond();
				case nameof(Vector4Round) : return new Vector4Round();
				case nameof(Vector4Subtract) : return new Vector4Subtract();
				case nameof(Vector4Sum) : return new Vector4Sum();
				case nameof(GraphInput) : return new GraphInput();
				case nameof(GraphOutput) : return new GraphOutput();
				case nameof(Null) : return new Null();
				case nameof(NullCheck) : return new NullCheck();
				case nameof(NullCoalesce) : return new NullCoalesce();
				case nameof(Self) : return new Self();
				case nameof(Cooldown) : return new Cooldown();
				case nameof(Timer) : return new Timer();
				case nameof(WaitForEndOfFrameUnit) : return new WaitForEndOfFrameUnit();
				case nameof(WaitForFlow) : return new WaitForFlow();
				case nameof(WaitForNextFrameUnit) : return new WaitForNextFrameUnit();
				case nameof(WaitForSecondsUnit) : return new WaitForSecondsUnit();
				case nameof(WaitUntilUnit) : return new WaitUntilUnit();
				case nameof(WaitWhileUnit) : return new WaitWhileUnit();
				case nameof(GetVariable) : return new GetVariable();
				case nameof(IsVariableDefined) : return new IsVariableDefined();
				case nameof(GetApplicationVariable) : return new GetApplicationVariable();
				case nameof(GetGraphVariable) : return new GetGraphVariable();
				case nameof(GetObjectVariable) : return new GetObjectVariable();
				case nameof(GetSavedVariable) : return new GetSavedVariable();
				case nameof(GetSceneVariable) : return new GetSceneVariable();
				case nameof(IsApplicationVariableDefined) : return new IsApplicationVariableDefined();
				case nameof(IsGraphVariableDefined) : return new IsGraphVariableDefined();
				case nameof(IsObjectVariableDefined) : return new IsObjectVariableDefined();
				case nameof(IsSavedVariableDefined) : return new IsSavedVariableDefined();
				case nameof(IsSceneVariableDefined) : return new IsSceneVariableDefined();
				case nameof(SetApplicationVariable) : return new SetApplicationVariable();
				case nameof(SetGraphVariable) : return new SetGraphVariable();
				case nameof(SetObjectVariable) : return new SetObjectVariable();
				case nameof(SetSavedVariable) : return new SetSavedVariable();
				case nameof(SetSceneVariable) : return new SetSceneVariable();
				case nameof(SaveVariables) : return new SaveVariables();
				case nameof(SetVariable) : return new SetVariable();
				case nameof(SuperUnit) : return new SuperUnit();
				case nameof(CustomSuperUnit) : return new CustomSuperUnit();
				case nameof(FunctionSuperUnit) : return new FunctionSuperUnit();
				case nameof(OnEnterState) : return new OnEnterState();
				case nameof(OnExitState) : return new OnExitState();
				case nameof(StateUnit) : return new StateUnit();
				case nameof(TriggerStateTransition) : return new TriggerStateTransition();
			}
			return null;
		}
	}
}
