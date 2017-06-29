using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PageNavigator.Business
{
	public static class ModulesManager
	{
		private static Dictionary<string, Model.ModuleData> modules = new Dictionary<string, Model.ModuleData>();

		public static Model.ModuleData GetModuleData(string name)
		{
			if (modules.ContainsKey(name))
			{
				return modules[name];
			}
			throw new ArgumentOutOfRangeException(
				string.Format("module with name \"{0}\" no found.", name));
		}

		public static ObservableCollection<Model.ModuleData> ModuleSets
		{
			get { return ViewModel.MenuContainerViewModel.Instance.ModuleSets; }
		}

		public static void Add(Model.ModuleData moduleSet)
		{
			if (moduleSet == null)
			{
				throw new ArgumentNullException("moduleSet");
			}
			if (!moduleSet.IsModuleSet)
			{
				throw new NotSupportedException(
					string.Format("only support container module \"{0}\"", moduleSet.Name));
			}

			List<string> insertedKeys = new List<string>();
			try
			{
				foreach (var module in moduleSet.SubModules)
				{
					if (!modules.ContainsKey(module.Name))
					{
						modules.Add(module.Name, module);
						insertedKeys.Add(module.Name);
					}
					else
					{
						throw new Exception(
							string.Format("There is already a module with same name \"{0}\" exists. Make sure to add unique modules. All added modules under current module set will be removed.", module.Name));
					}
				}
				ModuleSets.Add(moduleSet);
				if (!isRangeAdd)
				{
					ViewModel.MenuContainerViewModel.Instance.OnPropertyChanged("ModuleSets");
				}
			}
			catch
			{
				//rollback
				foreach (var key in insertedKeys)
				{
					modules.Remove(key);
				}
				throw;
			}
			finally
			{
				insertedKeys.Clear();
			}
		}

		private static bool isRangeAdd;

		public static void AddRange(IEnumerable<Model.ModuleData> moduleSets)
		{
			if (moduleSets != null)
			{
				try
				{
					isRangeAdd = true;
					foreach (Model.ModuleData current in moduleSets)
					{
						Add(current);
					}
				}
				finally
				{
					isRangeAdd = false;
					ViewModel.MenuContainerViewModel.Instance.OnPropertyChanged("ModuleSets");
				}
			}
		}

		public static void Load(string filename = null)
		{
			if (string.IsNullOrEmpty(filename))
			{
				filename = "AppData/modulesets.xml";
			}
			FileInfo info = new FileInfo(filename);
			if (!info.Exists)
			{
				throw new FileNotFoundException(info.FullName);
			}

			object result;
			XmlSerializer serializer = new XmlSerializer(typeof(Model.ModuleData));
			using (var stream = info.OpenRead())
			{
				result = serializer.Deserialize(stream);
			}
			if (result == null)
			{
				throw new Exception("cannot read module sets data from file: " + info.FullName);
			}
			Model.ModuleData moduleSets = (Model.ModuleData)result;
			AddRange(moduleSets.SubModules);
			moduleSets = null;
		}

		public static void Save(string filename = null)
		{
			if (string.IsNullOrEmpty(filename))
			{
				filename = "AppData/modulesets.xml";
			}
			FileInfo info = new FileInfo(filename);
			if (!info.Directory.Exists)
			{
				info.Directory.Create();
			}

			Model.ModuleData moduleSets = new Model.ModuleData(null, ModuleSets.ToList());
			XmlSerializer serializer = new XmlSerializer(typeof(Model.ModuleData));
			using (var stream = new FileStream(info.FullName, FileMode.Create, FileAccess.ReadWrite))
			{
				serializer.Serialize(stream, moduleSets);
			}
			moduleSets = null;
		}
	}
}
