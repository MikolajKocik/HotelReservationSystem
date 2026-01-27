using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Reservation
{
    /// <summary>
    /// Confirms the reservation by setting its status to Confirmed.
    /// </summary>
    public void ConfirmReservation()
    {
        this.Status = ReservationStatus.Confirmed;
    }

    /// <summary>
    /// Cancels the reservation by setting its status to Cancelled.
    /// </summary>
    public void CancelReservation()
    {
        this.Status = ReservationStatus.Cancelled;
    }

    /// <summary>
    /// Completes the reservation by setting its status to Completed.
    /// </summary>
    public void CompleteReservation()
    {
        this.Status = ReservationStatus.Completed;
    }

}
