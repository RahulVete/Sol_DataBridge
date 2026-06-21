using Sol_DataBridge.Models.Entities;

public interface IValidationService
{
    List<ValidationErrorEntity>
        ValidateInvoices(
        List<InvoiceEntity> invoices);
}