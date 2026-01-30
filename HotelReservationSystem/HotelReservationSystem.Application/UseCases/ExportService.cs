using System.Text;
using System.IO;
using System.Net.Http;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HotelReservationSystem.Application.Services;

public sealed class ExportService : IExportService
{
    private readonly IConfiguration config;
    private readonly HttpClient httpClient;

    public ExportService(IConfiguration config, IHttpClientFactory clientFactory)
    {
        this.config = config;
        this.httpClient = clientFactory.CreateClient();
    }

    public Task<byte[]> GenerateCsvAsync(ReportDto dto)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Metric,Value");
        sb.AppendLine($"TotalReservations,{dto.TotalReservations}");
        sb.AppendLine($"ConfirmedReservations,{dto.ConfirmedReservations}");
        sb.AppendLine($"CanceledReservations,{dto.CanceledReservations}");
        sb.AppendLine($"TotalPayments,{dto.TotalPayments}");
        sb.AppendLine($"AvailableRooms,{dto.AvailableRooms}");

        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());

        if (config.GetValue<bool>("Export:EnableSharePoint"))
        {
            string? webhook = config["Export:PowerAutomate:WebhookUrl"];
            if (!string.IsNullOrEmpty(webhook))
            {
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(bytes), "file", $"report_{DateTime.UtcNow:yyyyMMdd}.csv");
                _ = httpClient.PostAsync(webhook, content);
            }
            else
            {
                string? site = config["Export:SharePoint:SiteUrl"];
                string? library = config["Export:SharePoint:Library"];
                if (!string.IsNullOrEmpty(site) && !string.IsNullOrEmpty(library))
                {
                    try
                    {
                        var uploadUrl = new Uri(new Uri(site.TrimEnd('/')), $"_api/web/GetFolderByServerRelativeUrl('{library}')/Files/add(url='report_{DateTime.UtcNow:yyyyMMdd}.csv',overwrite=true)");
                        var req = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
                        {
                            Content = new ByteArrayContent(bytes)
                        };
                        _ = httpClient.SendAsync(req);
                    }
                    catch
                    {
                    }
                }
            }
        }

        return Task.FromResult(bytes);
    }

    public Task<byte[]> GenerateExcelAsync(ReportDto dto)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var ms = new MemoryStream();
        using (var package = new ExcelPackage(ms))
        {
            var ws = package.Workbook.Worksheets.Add("Report");
            ws.Cells[1, 1].Value = "Metric";
            ws.Cells[1, 2].Value = "Value";
            ws.Cells[2, 1].Value = "TotalReservations";
            ws.Cells[2, 2].Value = dto.TotalReservations;
            ws.Cells[3, 1].Value = "ConfirmedReservations";
            ws.Cells[3, 2].Value = dto.ConfirmedReservations;
            ws.Cells[4, 1].Value = "CanceledReservations";
            ws.Cells[4, 2].Value = dto.CanceledReservations;
            ws.Cells[5, 1].Value = "TotalPayments";
            ws.Cells[5, 2].Value = dto.TotalPayments;
            ws.Cells[6, 1].Value = "AvailableRooms";
            ws.Cells[6, 2].Value = dto.AvailableRooms;
            package.Save();
        }
        ms.Position = 0;
        byte[] bytes = ms.ToArray();

        if (config.GetValue<bool>("Export:EnableSharePoint"))
        {
            string? webhook = config["Export:PowerAutomate:WebhookUrl"];
            if (!string.IsNullOrEmpty(webhook))
            {
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(bytes), "file", $"report_{DateTime.UtcNow:yyyyMMdd}.xlsx");
                _ = httpClient.PostAsync(webhook, content);
            }
            else
            {
                string? site = config["Export:SharePoint:SiteUrl"];
                string? library = config["Export:SharePoint:Library"];
                if (!string.IsNullOrEmpty(site) && !string.IsNullOrEmpty(library))
                {
                    try
                    {
                        var uploadUrl = new Uri(new Uri(site.TrimEnd('/')), $"_api/web/GetFolderByServerRelativeUrl('{library}')/Files/add(url='report_{DateTime.UtcNow:yyyyMMdd}.xlsx',overwrite=true)");
                        var req = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
                        {
                            Content = new ByteArrayContent(bytes)
                        };
                        _ = httpClient.SendAsync(req);
                    }
                    catch
                    {
                    }
                }
            }
        }

        return Task.FromResult(bytes);
    }

    public async Task<byte[]> GeneratePdfAsync(ReportDto dto)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(QuestPDF.Helpers.PageSizes.A4);
                page.Margin(20);
                page.Header().Text("Raport - System Rezerwacji Hotelowych").SemiBold().FontSize(18);
                page.Content().Column(column =>
                {
                    column.Spacing(5);
                    column.Item().Text($"TotalReservations: {dto.TotalReservations}");
                    column.Item().Text($"ConfirmedReservations: {dto.ConfirmedReservations}");
                    column.Item().Text($"CanceledReservations: {dto.CanceledReservations}");
                    column.Item().Text($"TotalPayments: {dto.TotalPayments}");
                    column.Item().Text($"AvailableRooms: {dto.AvailableRooms}");
                });
                page.Footer().AlignCenter().Text($"Wygenerowano: {DateTime.UtcNow:yyyy-MM-dd HH:mm}");
            });
        });

        byte[] pdfBytes = doc.GeneratePdf();

        string? webhook = config["Export:PowerAutomate:WebhookUrl"];
        if (!string.IsNullOrEmpty(webhook))
        {
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(pdfBytes), "file", $"report_{DateTime.UtcNow:yyyyMMdd}.pdf");
            await httpClient.PostAsync(webhook, content);
        }

        return pdfBytes;
    }
}
