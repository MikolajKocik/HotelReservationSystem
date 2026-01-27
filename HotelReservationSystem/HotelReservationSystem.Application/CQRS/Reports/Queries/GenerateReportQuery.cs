using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using HotelReservationSystem.Application.Dtos.Report;

namespace HotelReservationSystem.Application.CQRS.Reports.Queries;

/// <summary>
/// Query to generate a report for a date range
/// </summary>
public record GenerateReportQuery(
    DateTime FromDate,
    DateTime ToDate
) : IQuery<ReportDto>;