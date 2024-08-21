using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace HLH.Lib.Config
{
    public class CustomSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        object IConfigurationSectionHandler.Create(
  object parent, object configContext, XmlNode section)
        {
            // Creates the configuration object that this method will return.
            // This can be a custom configuration class.
            // In this example, we use a System.Collections.Generic.IDictionary<string, IList<string>>.

            IDictionary<string, IList<IDictionary<string, string>>> myConfigObject = new Dictionary<string, IList<IDictionary<string, string>>>();

            // Gets any attributes for this section element.
            IDictionary<string, string> myAttribs = new Dictionary<string, string>();

            foreach (XmlAttribute attrib in section.Attributes)
            {
                if (XmlNodeType.Attribute == attrib.NodeType)
                    myAttribs.Add(attrib.Name, attrib.Value);
            }

            IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();

            // Puts the section name and attributes as the first config object item.
            list.Add(myAttribs);
            myConfigObject.Add(section.Name, list);

            // Gets the child element names and attributes.            
            foreach (XmlNode child in section.ChildNodes)
            {
                if (XmlNodeType.Element == child.NodeType)
                {
                    IDictionary<string, string> myChildAttribs = new Dictionary<string, string>();

                    foreach (XmlAttribute childAttrib in child.Attributes)
                    {
                        if (XmlNodeType.Attribute == childAttrib.NodeType)
                            myChildAttribs.Add(childAttrib.Name, childAttrib.Value);
                    }
                    if (myConfigObject.ContainsKey(myChildAttribs["key"]))
                    {
                        myConfigObject[myChildAttribs["key"]].Add(myChildAttribs);
                    }
                    else
                    {
                        list = new List<IDictionary<string, string>>();
                        list.Add(myChildAttribs);
                        myConfigObject.Add(myChildAttribs["key"], list);
                    }
                }
            }
            return (myConfigObject);
        }
        #endregion

    }
}
