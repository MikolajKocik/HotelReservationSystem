type RoomOption = { id: number; text: string; statusLabel?: string; pricePerNight: number };

document.addEventListener('DOMContentLoaded', () => {
    wireReservationFormHandlers();
    initDynamicRoomLoading();
});

function wireReservationFormHandlers(): void {
    const forms = document.querySelectorAll<HTMLFormElement>('#singleReservationForm, #doubleReservationForm');
    forms.forEach(form => {
        form.addEventListener('submit', handleReservationSubmit);
    });
}

function initDynamicRoomLoading(): void {
    ['single', 'double'].forEach(formType => {
        const form = document.getElementById(`${formType}ReservationForm`) as HTMLFormElement | null;
        if (!form) return;

        const arrivalInput = form.querySelector<HTMLInputElement>('[name="ArrivalDate"]');
        const departureInput = form.querySelector<HTMLInputElement>('[name="DepartureDate"]');
        const roomSelect = form.querySelector<HTMLSelectElement>('[name="RoomId"]');
        const roomTypeInput = form.querySelector<HTMLInputElement>('[name="RoomType"]');
        const roomType = roomTypeInput?.value || (formType.charAt(0).toUpperCase() + formType.slice(1));

        const loadRooms = async () => {
            const arrival = arrivalInput?.value;
            const departure = departureInput?.value;

            if (!arrival || !departure || !roomSelect) {
                if (roomSelect) roomSelect.innerHTML = '<option value="">Wybierz pokój...</option>';
                return;
            }

            try {
                const response = await fetch(`/Reservation/GetAvailableRoomsByDate?roomType=${encodeURIComponent(roomType)}&arrivalDate=${encodeURIComponent(arrival)}&departureDate=${encodeURIComponent(departure)}`);
                if (!response.ok) throw new Error();
                
                const data = await response.json() as { rooms: RoomOption[] };

                if (roomSelect) {
                    const currentValue = roomSelect.value;
                    roomSelect.innerHTML = '<option value="">Wybierz pokój...</option>';

                    data.rooms.forEach(room => {
                        const option = document.createElement('option');
                        option.value = room.id.toString();
                        option.textContent = `${room.text} ${room.statusLabel || ''}`.trim();
                        option.dataset.price = room.pricePerNight.toString();
                        roomSelect.appendChild(option);
                    });

                    if (currentValue && data.rooms.some(r => r.id.toString() === currentValue)) {
                        roomSelect.value = currentValue;
                    }

                    roomSelect.dispatchEvent(new Event('change'));
                }
            } catch (error) {
                console.error(error);
            }
        };

        arrivalInput?.addEventListener('change', loadRooms);
        departureInput?.addEventListener('change', loadRooms);

        roomSelect?.addEventListener('change', async () => {
            const roomId = roomSelect.value;
            const arrival = arrivalInput?.value;
            const departure = departureInput?.value;

            if (!roomId || !arrival || !departure) return;

            try {
                const response = await fetch(`/Reservation/CheckRoomAvailability?roomId=${encodeURIComponent(roomId)}&arrivalDate=${encodeURIComponent(arrival)}&departureDate=${encodeURIComponent(departure)}`);
                if (!response.ok) return;
                const data = await response.json() as { message?: string };

                const selectedOption = roomSelect.options[roomSelect.selectedIndex];
                if (selectedOption && selectedOption.value) {
                    const baseText = selectedOption.textContent!.replace(/\s*\[.*?\]\s*$/, '');
                    selectedOption.textContent = `${baseText} ${data.message || ''}`.trim();
                }
            } catch (error) {
                console.error(error);
            }
        });

        if (arrivalInput?.value && departureInput?.value) {
            loadRooms();
        }
    });
}

async function handleReservationSubmit(event: Event): Promise<void> {
    event.preventDefault();
    const form = event.target as HTMLFormElement;

    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }