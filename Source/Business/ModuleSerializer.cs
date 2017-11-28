using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PageNavigator.Business
{
	internal class ModuleSerializer
	{
		public byte[] Serialize(IEnumerable<Model.ModuleData> modules)
		{
			if(modules==null)
				throw new ArgumentNullException("modules");
			XElement root = new XElement("ModuleSets");
			foreach (var item in modules)
			{
				root.Add(serialize(item));
			}
			using (MemoryStream output = new MemoryStream())
			{
				root.Save(output);
				return output.ToArray();
			}
		}

		public IEnumerable<Model.ModuleData> Deserialize(byte[] data)
		{
			if (data == null || data.Length == 0)
				yield break;
			XDocument doc;
			using (MemoryStream stream = new MemoryStream(data))
			{
				doc = XDocument.Load(stream);
			}

			foreach (var element in doc.Root.Elements())
			{
				yield return deserialize(element);
			}
		}

		private XElement serialize(Model.ModuleData module)
		{
			var element = new XElement("Module");
			using (var writer = element.CreateWriter())
			{
				((IXmlSerializable)module).WriteXml(writer);
			}
			foreach (var item in module.SubModules)
			{
				element.Add(serialize(item));
			}
			return element;
		}

		private Model.ModuleData deserialize(XElement element)
		{
			Model.ModuleData module = Model.ModuleData.Empty;
			using (var reader = element.CreateReader())
			{
				((IXmlSerializable)module).ReadXml(reader);
			}
			foreach (XElement inner in element.Elements())
			{
				module.Add(deserialize(inner));
			}
			return module;
		}
	}
}
