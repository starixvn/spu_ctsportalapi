namespace Fanex.NAP.CTS.Service.Interface.CustomerClassification
{
    using Fanex.NAP.CTS.Model.RescanCustClassificationModel;

    public interface IRescanCustomerClassificationService
    {        
        bool RescanCustomerClassification(RescanCustClassificationModel parameters);       
    }
}
