namespace UCon {

	interface IEvaluatable {
		int NumOfParams { get; }
		Unit Invoke(Unit[] Parameters);
	}
}
