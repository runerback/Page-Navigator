using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageNavigator
{
	public interface IOpenModule
	{
		void Open(Model.ModuleData module);
	}
}
