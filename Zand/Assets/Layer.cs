using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zand.Assets
{
    public class Layer
    {
        public string Name;
        public int Id;
        public int Width;
        public int Height;
        public int[] Data;

        public Layer(XmlNode layerNode)
        {
            var attributes = layerNode.Attributes;

            Name = attributes.GetNamedItem("name").Value;
            Id = int.Parse(attributes.GetNamedItem("id").Value);
            Width = int.Parse(attributes.GetNamedItem("width").Value);
            Height = int.Parse(attributes.GetNamedItem("height").Value);

            var data = layerNode.FirstChild.InnerText.Split(',');
            LoadData(data);
            
        }

        private void LoadData(string[] strData)
        {
            Data = new int[strData.Length];
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = int.Parse(strData[i]);
            }
        }
    }
}
