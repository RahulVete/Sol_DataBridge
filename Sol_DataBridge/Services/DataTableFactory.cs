using System.Data;
using Sol_DataBridge.Models.Entities;

namespace Sol_DataBridge.Services;

public static class DataTableFactory
{
    public static DataTable CreateInvoiceTable(
        List<InvoiceEntity> invoices)
    {
        DataTable dt = new();

        dt.Columns.Add("InvoiceID", typeof(Guid));
        dt.Columns.Add("ImportBatchID", typeof(Guid));
        dt.Columns.Add("InvoiceNo");
        dt.Columns.Add("InvoiceDate", typeof(DateTime));
        dt.Columns.Add("PartyCode");
        dt.Columns.Add("PartyName");
        dt.Columns.Add("MasterID");
        dt.Columns.Add("GrossAmount", typeof(decimal));
        dt.Columns.Add("SoNo");
        dt.Columns.Add("SoDate", typeof(DateTime));
        dt.Columns.Add("PoNo");
        dt.Columns.Add("PoDate", typeof(DateTime));

        foreach (var invoice in invoices)
        {
            dt.Rows.Add(
                invoice.InvoiceID,
                invoice.ImportBatchID,
                invoice.InvoiceNo,
                invoice.InvoiceDate,
                invoice.PartyCode,
                invoice.PartyName,
                invoice.MasterID,
                invoice.GrossAmount,
                invoice.SoNo,
                invoice.SoDate,
                invoice.PoNo,
                invoice.PoDate);
        }

        return dt;
    }


    public static DataTable CreateItemTable(
    List<InvoiceItemEntity> items)
    {
        DataTable dt = new();

        dt.Columns.Add("InvoiceItemID", typeof(Guid));
        dt.Columns.Add("InvoiceID", typeof(Guid));
        dt.Columns.Add("ItemCode");
        dt.Columns.Add("ItemName");
        dt.Columns.Add("Unit");
        dt.Columns.Add("Quantity", typeof(decimal));
        dt.Columns.Add("Rate", typeof(decimal));
        dt.Columns.Add("MRP", typeof(decimal));
        dt.Columns.Add("Amount", typeof(decimal));
        dt.Columns.Add("IGST", typeof(decimal));
        dt.Columns.Add("TotalAmount", typeof(decimal));

        foreach (var item in items)
        {
            dt.Rows.Add(
                item.InvoiceItemID,
                item.InvoiceID,
                item.ItemCode,
                item.ItemName,
                item.Unit,
                item.Quantity,
                item.Rate,
                item.MRP,
                item.Amount,
                item.IGST,
                item.TotalAmount);
        }

        return dt;
    }

    public static DataTable CreateAssortmentTable(
    List<AssortmentEntity> assortments)
    {
        DataTable dt = new();

        dt.Columns.Add("AssortmentID", typeof(Guid));
        dt.Columns.Add("InvoiceID", typeof(Guid));
        dt.Columns.Add("ItemCode");
        dt.Columns.Add("UID");

        foreach (var item in assortments)
        {
            dt.Rows.Add(
                item.AssortmentID,
                item.InvoiceID,
                item.ItemCode,
                item.UID);
        }

        return dt;
    }

    public static DataTable CreatePackingTable(
    List<PackingInfoEntity> packingList)
    {
        DataTable dt = new();

        dt.Columns.Add("PackingInfoID", typeof(Guid));
        dt.Columns.Add("AssortmentID", typeof(Guid));
        dt.Columns.Add("ItemCode");
        dt.Columns.Add("Quantity", typeof(decimal));

        foreach (var item in packingList)
        {
            dt.Rows.Add(
                item.PackingInfoID,
                item.AssortmentID,
                item.ItemCode,
                item.Quantity);
        }

        return dt;
    }

    public static DataTable CreatePairTable(
    List<PairDetailEntity> pairList)
    {
        DataTable dt = new();

        dt.Columns.Add("PairDetailID", typeof(Guid));
        dt.Columns.Add("PackingInfoID", typeof(Guid));
        dt.Columns.Add("PairQR");
        dt.Columns.Add("PairUID");

        foreach (var item in pairList)
        {
            dt.Rows.Add(
                item.PairDetailID,
                item.PackingInfoID,
                item.PairQR,
                item.PairUID);
        }

        return dt;
    }


}