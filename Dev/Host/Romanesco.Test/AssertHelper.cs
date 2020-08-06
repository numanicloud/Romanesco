using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Romanesco.Test
{
	internal static class AssertHelper
	{
		public static bool NotNull([NotNullWhen(true)]object? subject)
		{
			return subject is {};
		}
	}
}
