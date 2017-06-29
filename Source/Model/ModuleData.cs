using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PageNavigator.Model
{
	[Serializable]
	[XmlRoot("ModuleSets")]
	public class ModuleData : Common.ViewModelBase, IEquatable<ModuleData>
	{
		/// <summary>
		/// Name in code
		/// </summary>
		[XmlAttribute("name")]
		public string Name { get; set; }

		private string title;
		/// <summary>
		/// Title in UI
		/// </summary>
		[XmlAttribute("title")]
		public string Title
		{
			get { return this.title; }
			set
			{
				if (this.title != value)
				{
					this.title = value;
					this.OnPropertyChanged("Title");
				}
			}
		}

		private ObservableCollection<ModuleData> subModules;
		[XmlElement("Module")]
		public ObservableCollection<ModuleData> SubModules
		{
			get { return this.subModules; }
		}

		[XmlIgnore]
		public ModuleData ModuleSet { get; private set; }

		/// <summary>
		/// whether is set of modules under this module
		/// </summary>
		[XmlIgnore]
		public bool IsModuleSet
		{
			get { return this.SubModules.Count > 0; }
		}

		private void subModules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count > 0)
			{
				if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
				{
					foreach (ModuleData moduleData in e.NewItems)
					{
						moduleData.ModuleSet = this;
					}
				}
			}
		}

		public ModuleData()
		{
			this.subModules = new ObservableCollection<ModuleData>();
			this.subModules.CollectionChanged += subModules_CollectionChanged;
		}

		public ModuleData(string name, string title)
			: this()
		{
			this.Name = name;
			this.Title = title;
		}

		public ModuleData(string name)
			: this(name, name) { }

		public ModuleData(string name, string title, IEnumerable<ModuleData> subModules)
			: this(name, title)
		{
			if (subModules != null)
			{
				foreach (var module in subModules)
				{
					this.subModules.Add(module);
				}
			}
		}

		public ModuleData(string name, IEnumerable<ModuleData> subModules)
			: this(name, name, subModules) { }

		public bool Equals(ModuleData other)
		{
			return other != null && this.Name == other.Name;
		}

		/// <summary>
		/// create a copy, not reference
		/// </summary>
		public static ModuleData Copy(ModuleData origin)
		{
			return new ModuleData(origin.Name, origin.Title, origin.SubModules);
		}

		~ModuleData()
		{
			this.subModules.CollectionChanged -= subModules_CollectionChanged;
		}
	}
}
