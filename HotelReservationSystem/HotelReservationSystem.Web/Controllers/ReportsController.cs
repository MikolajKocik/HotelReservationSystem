using System.Text;
using System.IO;
using HotelReservationSystem.Web.ViewModels;
using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Reports.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Application.Dtos.Report;
using HotelReservationSystem.Web.Utils.ModelMappings;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using HotelReservationSystem.Application.Interfaces;

namespace HotelReservationSystem.Controllers;

[Authorize(Policy = "RequireManager")]
public sealed class ReportsController : Controller
{
    private readonly ICQRSMediator mediator;
    private readonly IExportService exportService;

    public ReportsController(ICQRSMediator mediator, IExportService exportService)
    {
        this.mediator = mediator;
        this.exportService = exportService;
    }

    [HttpGet]
    public async Task<IActionResult> Reports()
    {
        var query = new GenerateReportQuery(DateTime.Today.AddDays(-30), DateTime.Today);
        ReportDto reportData = await mediator.SendAsync(query);

        var model = ReportMappingHelper.MapToReportViewModel(reportData);

        return View(model);
    }

    [HttpGet("generate/csv")]
    public async Task<IActionResult> GenerateCsv()
    {
        var query = new GenerateReportQuery(DateTime.Today.AddDays(-30), DateTime.Today);
        ReportDto dto = await mediator.SendAsync(query);

        var bytes = await this.exportService.GenerateCsvAsync(dto);
        var filename = $"report_{DateTime.UtcNow:yyyyMMdd}.csv";

        return File(bytes, "text/csv", filename);
    }

    [HttpGet("generate/excel")]
    public async Task<IActionResult> GenerateExcel()
    {
        var query = new GenerateReportQuery(DateTime.Today.AddDays(-30), DateTime.Today);
        ReportDto dto = await mediator.SendAsync(query);

        var bytes = await this.exportService.GenerateExcelAsync(dto);
        var filename = $"report_{DateTime.UtcNow:yyyyMMdd}.xlsx";

        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
    }

    [HttpGet("generate/pdf")]
    public async Task<IActionResult> GeneratePdf()
    {
        var query = new GenerateReportQuery(DateTime.Today.AddDays(-30), DateTime.Today);
        ReportDto dto = await mediator.SendAsync(query);

        var bytes = await this.exportService.GeneratePdfAsync(dto);
        var filename = $"report_{DateTime.UtcNow:yyyyMMdd}.pdf";

        return File(bytes, "application/pdf", filename);
    }

    [HttpPost("generate/pdf/upload")]
    public async Task<IActionResult> GeneratePdfAndUpload()
    {
        var query = new GenerateReportQuery(DateTime.Today.AddDays(-30), DateTime.Today);
        ReportDto dto = await mediator.SendAsync(query);

        var bytes = await this.exportService.GeneratePdfAsync(dto);
        return Ok(new { uploaded = true });
    }
}
