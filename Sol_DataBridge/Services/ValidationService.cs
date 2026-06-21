using System.Text.Json;
using Sol_DataBridge.Models.Entities;

namespace Sol_DataBridge.Services;

public class ValidationService :
    IValidationService
{
    public List<ValidationErrorEntity>
        ValidateInvoices(
        List<InvoiceEntity> invoices)
    {
        List<ValidationErrorEntity>
            errors = new();

        foreach (var invoice in invoices)
        {
            if (string.IsNullOrWhiteSpace(
                invoice.InvoiceNo))
            {
                errors.Add(
                    new ValidationErrorEntity
                    {
                        ImportBatchID =
                            invoice.ImportBatchID,

                        EntityName =
                            "Invoice",

                        FailedData =
                            JsonSerializer.Serialize(invoice),

                        ValidationMessage =
                            "Invoice Number Missing"
                    });
            }

            if (string.IsNullOrWhiteSpace(
                invoice.PartyCode))
            {
                errors.Add(
                    new ValidationErrorEntity
                    {
                        ImportBatchID =
                            invoice.ImportBatchID,

                        EntityName =
                            "Invoice",

                        FailedData =
                            JsonSerializer.Serialize(invoice),

                        ValidationMessage =
                            "Party Code Missing"
                    });
            }

            if (invoice.GrossAmount <= 0)
            {
                errors.Add(
                    new ValidationErrorEntity
                    {
                        ImportBatchID =
                            invoice.ImportBatchID,

                        EntityName =
                            "Invoice",

                        FailedData =
                            JsonSerializer.Serialize(invoice),

                        ValidationMessage =
                            "Gross Amount Invalid"
                    });
            }
        }

        return errors;
    }
}