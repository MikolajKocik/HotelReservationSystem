using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Reservation
{
    /// <summary>
    /// Confirms the reservation by setting its status to Confirmed.
    /// </summary>
    public void ConfirmReservation()
    {
        UpdateStatus(ReservationStatus.Confirmed);
    }

    /// <summary>
    /// Cancels the reservation by setting its status to Cancelled.
    /// </summary>
    public void CancelReservation(string reason)
    {
        UpdateStatus(ReservationStatus.Cancelled, reason);
    }

    /// <summary>
    /// Completes the reservation by setting its status to Completed.
    /// </summary>
    public void CompleteReservation()
    {
        UpdateStatus(ReservationStatus.Completed);
    }

}

