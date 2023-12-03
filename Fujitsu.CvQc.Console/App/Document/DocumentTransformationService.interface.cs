using DocumentFormat.OpenXml;

namespace Fujitsu.CvQc.Console.App
{
    public interface ITransformationService
    {
        void Transform_Paragraph(DocumentMappingSectionModel section, OpenXmlElement node);
        void Transform_Text(DocumentMappingSectionModel section, OpenXmlElement node);
        void Transform_Custom(string paragraph, OpenXmlElement node);
        void Transform_RealiationGrid(DocumentMappingSectionModel section, OpenXmlElement node);        
    }
}