(() => {
    'use strict';

    const RULES: Record<string, RegExp> = {
        GuestEmail: /^[^@\s]+@[^@\s]+\.[^@\s]+$/,
        GuestPhoneNumber: /^\+?\d{9,}$/
    };

    const DISCOUNTS: Record<string, number> = {
        'SUMMER10': 0.9,
        'WINTER15': 0.85
    };

    const calculateTotal = (form: HTMLFormElement, type: string) => {
        const arrival = form.querySelector<HTMLInputElement>('[name="ArrivalDate"]')?.value;
        const departure = form.querySelector<HTMLInputElement>('[name="DepartureDate"]')?.value;
        const roomSelect = form.querySelector<HTMLSelectElement>('[name="RoomId"]');
        const discountCode = form.querySelector<HTMLInputElement>('[name="DiscountCode"]')?.value.trim().toUpperCase() || '';
        
        const totalDisp = document.getElementById(`${type}-total-sum`);

        if (!arrival || !departure || !roomSelect?.value) {
            if (totalDisp) totalDisp.textContent = '0.00 zł';
            return;
        }

        const nights = Math.ceil((new Date(departure).getTime() - new Date(arrival).getTime()) / 86400000);
        const price = parseFloat(roomSelect.selectedOptions[0]?.dataset.price || '0');
        
        if (nights > 0 && price > 0) {
            const total = nights * price * (DISCOUNTS[discountCode] || 1.0);
            const formatted = total.toFixed(2);
            if (totalDisp) totalDisp.textContent = `${formatted} zł`;
        } else {
            if (totalDisp) totalDisp.textContent = '0.00 zł';
        }
    };

    const loadRooms = async (form: HTMLFormElement, type: string) => {
        const arrival = form.querySelector<HTMLInputElement>('[name="ArrivalDate"]')?.value;
        const departure = form.querySelector<HTMLInputElement>('[name="DepartureDate"]')?.value;
        const roomSelect = form.querySelector<HTMLSelectElement>('[name="RoomId"]');

        if (!arrival || !departure || !roomSelect) {
            console.log('loadRooms: Missing arrival, departure, or roomSelect', { arrival, departure, roomSelect });
            return;
        }

        const roomType = type.charAt(0).toUpperCase() + type.slice(1);
        console.log('loadRooms: Fetching rooms...', { roomType, arrival, departure });
        
        try {
            const res = await fetch(`/Reservation/GetAvailableRoomsByDate?roomType=${roomType}&arrivalDate=${arrival}&departureDate=${departure}`);
            const data = await res.json();
            
            console.log('loadRooms: Received data', data);

            roomSelect.innerHTML = '<option value="">Wybierz pokój...</option>';
            
            if (data.rooms && Array.isArray(data.rooms)) {
                data.rooms.forEach((r: any) => {
                    console.log('Room:', r);
                    if (r.statusLabel === '[Wolny]') {
                        const opt = document.createElement('option');
                        opt.value = r.id;
                        opt.dataset.price = r.pricePerNight;
                        opt.textContent = `${r.text}`;
                        roomSelect.appendChild(opt);
                    }
                });
            }
            
            calculateTotal(form, type);
        } catch (e) {
            console.error("Błąd ładowania pokoi:", e);
        }
    };

    const initReservationForm = (form: HTMLFormElement) => {
        const type = form.querySelector<HTMLInputElement>('[name="FormType"]')?.value || 'single';    
        const arrival = form.querySelector<HTMLInputElement>('[name="ArrivalDate"]')?.value;
        const departure = form.querySelector<HTMLInputElement>('[name="DepartureDate"]')?.value;

        form.querySelectorAll('input, select').forEach(el => {
            el.addEventListener('change', () => {
                if (el.getAttribute('name') === 'ArrivalDate' || el.getAttribute('name') === 'DepartureDate') {
                    loadRooms(form, type);
                } else {
                    calculateTotal(form, type);
                }
            });
        });

        if (arrival && departure) {
            console.log('initReservationForm: Loading rooms on init', { arrival, departure });
            loadRooms(form, type);
        }

        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            
            console.log('Form submit attempted');
            console.log('Form validity:', form.checkValidity());
            
            if (!form.checkValidity()) {
                console.log('Form validation failed');
                const invalidFields = form.querySelectorAll(':invalid');
                console.log('Invalid fields:', Array.from(invalidFields).map((f: any) => ({
                    name: f.name,
                    value: f.value,
                    validationMessage: f.validationMessage
                })));
                form.classList.add('was-validated');
                return;
            }

            console.log('Form is valid, submitting...');

            const formData = new FormData(form);

            console.log('Form data:', Object.fromEntries(formData.entries()));

            try {
                const res = await fetch(form.action, { method: 'POST', body: formData });
                const data = await res.json();

                console.log('Server response:', data);

                if (res.ok && data.redirectUrl) {
                    window.location.href = data.redirectUrl;
                } else {
                    alert(data.error || 'Wystąpił błąd podczas rezerwacji.');
                }
            } catch (err) {
                console.error(err);
                alert('Błąd połączenia z serwerem.');
            }
        });
    };

    document.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll<HTMLFormElement>('#singleReservationForm, #doubleReservationForm').forEach(initReservationForm);
    });
})();