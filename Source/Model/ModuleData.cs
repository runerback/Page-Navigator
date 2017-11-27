using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PageNavigator.Model
{
	public class ModuleData : Common.ViewModelBase, IXmlSerializable, IEquatable<ModuleData>, IEqualityComparer<ModuleData>
	{
		private string name = null;
		/// <summary>
		/// Name in code. should be unique
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}

		private string title;
		/// <summary>
		/// Title in UI Menu
		/// </summary>
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

		private ObservableCollection<ModuleData> subModules = 
			new ObservableCollection<ModuleData>();
		public ObservableCollection<ModuleData> SubModules
		{
			get { return this.subModules; }
		}

		public ModuleData ModuleSet { get; private set; }

		/// <summary>
		/// whether is set of modules under this module
		/// </summary>
		public bool IsModuleSet
		{
			get { return this.SubModules.Count > 0; }
		}

		public void Add(ModuleData subModule)
		{
			if (subModule == null)
				throw new ArgumentNullException("subModule");
			var set = subModule.ModuleSet;
			while (set != null)
			{
				if (set == this)
					throw new ArgumentException("subModule. loop add");
				set = set.ModuleSet;
			}

			this.subModules.Add(subModule);
			subModule.ModuleSet = this;
		}

		protected ModuleData() { }

		public ModuleData(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");
			this.name = name;
		}

		public ModuleData(string name, string title)
			: this(name)
		{
			if (string.IsNullOrEmpty(title))
				throw new ArgumentNullException("title");
			this.Title = title;
		}

		public bool Equals(ModuleData other)
		{
			return other != null && this.Name == other.Name;
		}

		public bool Equals(ModuleData x, ModuleData y)
		{
			return x == null ? y == null : x.Equals(y);
		}

		public int GetHashCode(ModuleData obj)
		{
			return obj.Name.GetHashCode();
		}

		internal static ModuleData Empty
		{
			get { return new ModuleData(); }
		}

		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
		{
			reader.Read();
			this.name = reader.GetAttribute("name");
			this.title = reader.GetAttribute("title");
		}

		void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteStartAttribute("name");
			writer.WriteValue(this.Name);
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("title");
			writer.WriteValue(this.title);
			writer.WriteEndAttribute();
		}
	}
}
