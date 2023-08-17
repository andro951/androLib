using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace androLib.Common.Utility
{
	public class BoolCheck {
		private event Func<bool> eventHandler;

		public void Add(Func<bool> func) {
			eventHandler += func;
		}

		public bool Invoke() {
			if (eventHandler == null)
				return false;

			foreach (Func<bool> func in eventHandler.GetInvocationList()) {
				if (func.Invoke())
					return true;
			}

			return false;
		}
	}
}
