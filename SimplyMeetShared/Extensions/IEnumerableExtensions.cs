using System;
using System.Linq;
using System.Collections.Generic;

public static class IEnumerableExtensions
{
	//===========================================================================================
	// Public Static Methods
	//===========================================================================================
	public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> InSource, Func<TSource, TKey> InKeySelector)
	{
		var Comparer = new KeyComparer<TSource, TKey>(InKeySelector);
		return InSource.Distinct(Comparer);
	}

	public class KeyComparer<TSource,TKey> : IEqualityComparer<TSource>
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Fields
		private Func<TSource, TKey> _KeySelector;
		#endregion

		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public KeyComparer(Func<TSource,TKey> InKeySelector)
		{
			_KeySelector = InKeySelector;
		}

		public Int32 GetHashCode(TSource InObject)
		{
			return _KeySelector(InObject).GetHashCode();
		}
		public Boolean Equals(TSource InFirst, TSource InSecond)
		{
			if (InFirst == null && InSecond == null) return true;
			if (InFirst == null || InSecond == null) return false;
			return _KeySelector(InFirst).Equals(_KeySelector(InSecond));
		}
	}
}