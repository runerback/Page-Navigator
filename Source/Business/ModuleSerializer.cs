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
		public byte[] Serializer(IEnumerable<Model.ModuleData> modules)
		{
			if(modules==null)
				throw new ArgumentNullException("modules");
			throw new NotImplementedException();
		}

		public IEnumerable<Model.ModuleData> Deserialize(byte[] data)
		{
			if (data == null || data.Length == 0)
				return null;
			XDocument doc;
			using (MemoryStream stream = new MemoryStream(data))
			{
				doc = XDocument.Load(stream);
			}
			
			throw new NotImplementedException();
		}

		private XElement serialize(Model.ModuleData module)
		{
			var element = new XElement("Module");
			using (var writer = element.CreateWriter())
			{
				((IXmlSerializable)module).WriteXml(writer);
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
