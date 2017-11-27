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
			//should handle Tree structure here
			foreach (IXmlSerializable item in modules)
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

			//should handle Tree structure here
			foreach (var element in doc.Root.Elements())
			{
				yield return deserialize(element);
			}
		}

		private XElement serialize(IXmlSerializable module)
		{
			var element = new XElement("Module");
			using (var writer = element.CreateWriter())
			{
				module.WriteXml(writer);
			}
			return element;
		}

		private Model.ModuleData deserialize(XElement element)
		{
			using (var reader = element.CreateReader())
			{
				Model.ModuleData module = Model.ModuleData.Empty;
				((IXmlSerializable)module).ReadXml(reader);
				return module;
			}
		}
	}
}
