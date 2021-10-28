
using System;
using System.Security.Cryptography;

namespace SimplyMeetShared.Base
{
	public static class RandomStatics
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Static Properties
		public static Random RANDOM => _RANDOM ?? (_RANDOM = new Random());
		public static RandomNumberGenerator RANDOM_CRYPTO => _RANDOM_CRYPTO ?? (_RANDOM_CRYPTO = RandomNumberGenerator.Create());
		#endregion
		#region Static Fields
		[ThreadStatic]
		private static RandomNumberGenerator _RANDOM_CRYPTO;
		[ThreadStatic]
		private static Random _RANDOM;
		#endregion
	}
}