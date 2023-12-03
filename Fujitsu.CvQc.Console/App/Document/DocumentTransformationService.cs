using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Fujitsu.CvQc.Console.App
{
    public class TransformationService : ITransformationService
    {
        public void Transform_Text(DocumentMappingSectionModel section, OpenXmlElement node)
        {
            for (var i = 0; i < section.Paragraphs.Count; i++)
            {
                string text = (i == 0 ? "" : " ");
                text += section.Paragraphs[i].Trim();
                var textNode = new Text()
                {
                    Text = text,
                    Space = SpaceProcessingModeValues.Preserve
                };
                node.AppendChild(textNode);
            }
        }

        public void Transform_Paragraph(DocumentMappingSectionModel section, OpenXmlElement node)
        {
            foreach (var paragraph in section.Paragraphs)
            {
                Paragraph paragraphNode = new Paragraph();
                Run run = paragraphNode.AppendChild(new Run());
                run.AppendChild(new Text(paragraph));                                                       
                node.AppendChild(paragraphNode);
            }
        }

        public void Transform_Custom(string paragraph, OpenXmlElement node) {

            string text = "";
            text += paragraph.Trim();
            var textNode = new Text()
            {
                Text = text,
                Space = SpaceProcessingModeValues.Preserve
            };

            node.AppendChild(textNode);
        }    

        public void Transform_RealiationGrid(DocumentMappingSectionModel section, OpenXmlElement node) {
            
        }


    }
}
