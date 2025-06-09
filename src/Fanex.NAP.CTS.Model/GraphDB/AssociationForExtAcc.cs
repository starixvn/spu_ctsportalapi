namespace Fanex.NAP.CTS.Model.GraphDB
{
    using Fanex.NAP.CTS.Core.Helpers.Xml;

    [XName("Association")]
    public class AssociationForExtAcc
    {
        public long CustId { get; set; }

        public int CustSubId { get; set; }

        [XName("Date")]
        public string AssociationDate { get; set; }
    }
}
