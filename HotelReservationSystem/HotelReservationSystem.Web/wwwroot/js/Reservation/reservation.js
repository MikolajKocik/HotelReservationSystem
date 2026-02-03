"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
(() => {
    'use strict';
    const RULES = {
        GuestEmail: /^[^@\s]+@[^@\s]+\.[^@\s]+$/,
        GuestPhoneNumber: /^\+?\d{9,}$/
    };
    const DISCOUNTS = {
        'SUMMER10': 0.9,
        'WINTER15': 0.85
    };
    const calculateTotal = (form, type) => {
        var _a, _b, _c, _d;
        const arrival = (_a = form.querySelector('[name="ArrivalDate"]')) === null || _a === void 0 ? void 0 : _a.value;
        const departure = (_b = form.querySelector('[name="DepartureDate"]')) === null || _b === void 0 ? void 0 : _b.value;
        const roomSelect = form.querySelector('[name="RoomId"]');
        const discountCode = ((_c = form.querySelector('[name="DiscountCode"]')) === null || _c === void 0 ? void 0 : _c.value.trim().toUpperCase()) || '';
        const totalDisp = document.getElementById(`${type}-total-sum`);
        if (!arrival || !departure || !(roomSelect === null || roomSelect === void 0 ? void 0 : roomSelect.value)) {
            if (totalDisp)
                totalDisp.textContent = '0.00 zł';
            return;
        }
        const nights = Math.ceil((new Date(departure).getTime() - new Date(arrival).getTime()) / 86400000);
        const price = parseFloat(((_d = roomSelect.selectedOptions[0]) === null || _d === void 0 ? void 0 : _d.dataset.price) || '0');
        if (nights > 0 && price > 0) {
            const total = nights * price * (DISCOUNTS[discountCode] || 1.0);
            const formatted = total.toFixed(2);
            if (totalDisp)
                totalDisp.textContent = `${formatted} zł`;
        }
        else {
            if (totalDisp)
                totalDisp.textContent = '0.00 zł';
        }
    };
    const loadRooms = (form, type) => __awaiter(void 0, void 0, void 0, function* () {
        var _a, _b;
        const arrival = (_a = form.querySelector('[name="ArrivalDate"]')) === null || _a === void 0 ? void 0 : _a.value;
        const departure = (_b = form.querySelector('[name="DepartureDate"]')) === null || _b === void 0 ? void 0 : _b.value;
        const roomSelect = form.querySelector('[name="RoomId"]');
        if (!arrival || !departure || !roomSelect) {
            console.log('loadRooms: Missing arrival, departure, or roomSelect', { arrival, departure, roomSelect });
            return;
        }
        const roomType = type.charAt(0).toUpperCase() + type.slice(1);
        console.log('loadRooms: Fetching rooms...', { roomType, arrival, departure });
        try {
            const res = yield fetch(`/Reservation/GetAvailableRoomsByDate?roomType=${roomType}&arrivalDate=${arrival}&departureDate=${departure}`);
            const data = yield res.json();
            console.log('loadRooms: Received data', data);
            roomSelect.innerHTML = '<option value="">Wybierz pokój...</option>';
            if (data.rooms && Array.isArray(data.rooms)) {
                data.rooms.forEach((r) => {
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
        }
        catch (e) {
            console.error("Błąd ładowania pokoi:", e);
        }
    });
    const initReservationForm = (form) => {
        var _a, _b, _c;
        const type = ((_a = form.querySelector('[name="FormType"]')) === null || _a === void 0 ? void 0 : _a.value) || 'single';
        const arrival = (_b = form.querySelector('[name="ArrivalDate"]')) === null || _b === void 0 ? void 0 : _b.value;
        const departure = (_c = form.querySelector('[name="DepartureDate"]')) === null || _c === void 0 ? void 0 : _c.value;
        form.querySelectorAll('input, select').forEach(el => {
            el.addEventListener('change', () => {
                if (el.getAttribute('name') === 'ArrivalDate' || el.getAttribute('name') === 'DepartureDate') {
                    loadRooms(form, type);
                }
                else {
                    calculateTotal(form, type);
                }
            });
        });
        if (arrival && departure) {
            console.log('initReservationForm: Loading rooms on init', { arrival, departure });
            loadRooms(form, type);
        }
        form.addEventListener('submit', (e) => __awaiter(void 0, void 0, void 0, function* () {
            e.preventDefault();
            console.log('Form submit attempted');
            console.log('Form validity:', form.checkValidity());
            if (!form.checkValidity()) {
                console.log('Form validation failed');
                const invalidFields = form.querySelectorAll(':invalid');
                console.log('Invalid fields:', Array.from(invalidFields).map((f) => ({
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
                const res = yield fetch(form.action, { method: 'POST', body: formData });
                const data = yield res.json();
                console.log('Server response:', data);
                if (res.ok && data.redirectUrl) {
                    window.location.href = data.redirectUrl;
                }
                else {
                    alert(data.error || 'Wystąpił błąd podczas rezerwacji.');
                }
            }
            catch (err) {
                console.error(err);
                alert('Błąd połączenia z serwerem.');
            }
        }));
    };
    document.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll('#singleReservationForm, #doubleReservationForm').forEach(initReservationForm);
    });
})();
//# sourceMappingURL=reservation.js.map