using ModelContextProtocol.Server;
using System.ComponentModel;

namespace HotelReservationSystem.MCP.Server.Resources;

public class HotelInfoResources
{
    [McpServerResource(UriTemplate = "hotel://rules", MimeType = "text/plain", Name = "Regulamin Hotelu")]
    [Description("Ogólny regulamin hotelu i zasady pobytu.")]
    public string GetHotelRules()
    {
        return @"
        Doba hotelowa: 14:00 - 11:00.
        Śniadania: 7:00 - 10:00 w restauracji głównej.
        Cisza nocna: 22:00 - 6:00.
        Zakaz palenia w pokojach (kara 500 PLN).
        Zwierzęta są akceptowane za dopłatą 50 PLN/doba.
        ";
    }
}
