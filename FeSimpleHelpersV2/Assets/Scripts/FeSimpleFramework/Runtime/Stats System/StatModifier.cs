namespace FeSimpleHelpers.StatsSystem
{
	public enum StatOperation
	{
		Add,
		Multiply,
		FinalAdd,
		Override
	}

	[System.Serializable]
	public class StatModifier
	{
		public StatConfig statType;
		public StatOperation operation;
		public float value;
		public int priority; //TODO remove this

		public float process(float _value)
		{
			switch (operation)
			{
				case StatOperation.Add: _value += value; break;
				case StatOperation.Multiply: _value *= value; break;
				case StatOperation.FinalAdd: _value += value; break;
				case StatOperation.Override: _value = value; break;
			}

			return _value;
		}

		//TODO pool this in some way
		public StatModifier GetCopy()
		{
			StatModifier newModifier = new StatModifier
			{
				statType = statType,
				operation = operation,
				value = value,
				priority = priority
			};

			return newModifier;
		}

		public bool FastEquals(StatModifier other)
		{
			return statType == other.statType &&
				    operation == other.operation &&
				    priority == other.priority;
		}

	}
}