using System;
using Cake.Core;

namespace Cake.TestAddin
{
	public static class TestAddinAliases
	{
		public static void ExecuteTestAddin(this ICakeContext context)
		{
		}

		public static void InvokeTestAddin(this ICakeContext context, string execution)
		{
		}
	}
}
