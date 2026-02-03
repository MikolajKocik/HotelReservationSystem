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
document.addEventListener('DOMContentLoaded', () => {
    wireReservationFormHandlers();
    initDynamicRoomLoading();
});
function wireReservationFormHandlers() {
    const singleForm = document.getElementById('singleReservationForm');
    const doubleForm = document.getElementById('doubleReservationForm');
    if (singleForm) {
        singleForm.addEventListener('submit', handleReservationSubmit);
    }
    if (doubleForm) {
        doubleForm.addEventListener('submit', handleReservationSubmit);
    }
}
function initDynamicRoomLoading() {
    ['single', 'double'].forEach(formType => {
        const form = document.getElementById(`${formType}ReservationForm`);
        if (!form)
            return;
        const arrivalInput = form.querySelector('[name="ArrivalDate"]');
        const departureInput = form.querySelector('[name="DepartureDate"]');
        const roomSelect = form.querySelector('[name="RoomId"]');
        const roomTypeInput = form.querySelector('[name="RoomType"]');
        const roomType = (roomTypeInput === null || roomTypeInput === void 0 ? void 0 : roomTypeInput.value) || (formType.charAt(0).toUpperCase() + formType.slice(1));
        const loadRooms = () => __awaiter(this, void 0, void 0, function* () {
            const arrival = arrivalInput === null || arrivalInput === void 0 ? void 0 : arrivalInput.value;
            const departure = departureInput === null || departureInput === void 0 ? void 0 : departureInput.value;
            try {
                const response = yield fetch(`/Reservation/GetAvailableRoomsByDate?roomType=${roomType}&arrivalDate=${arrival || ''}&departureDate=${departure || ''}`);
                const data = yield response.json();
                if (roomSelect) {
                    const currentValue = roomSelect.value;
                    roomSelect.innerHTML = '<option value="">Wybierz pokój...</option>';
                    data.rooms.forEach((room) => {
                        const option = document.createElement('option');
                        option.value = room.id.toString();
                        option.textContent = `${room.text} ${room.statusLabel || ''}`.trim();
                        option.dataset.price = room.pricePerNight.toString();
                        roomSelect.appendChild(option);
                    });
                    if (currentValue && data.rooms.some((r) => r.id == currentValue)) {
                        roomSelect.value = currentValue;
                    }
                    roomSelect.dispatchEvent(new Event('change'));
                }
            }
            catch (error) {
                console.error('Error loading rooms:', error);
            }
        });
        arrivalInput === null || arrivalInput === void 0 ? void 0 : arrivalInput.addEventListener('change', loadRooms);
        departureInput === null || departureInput === void 0 ? void 0 : departureInput.addEventListener('change', loadRooms);
        loadRooms();
    });
}
function handleReservationSubmit(event) {
    return __awaiter(this, void 0, void 0, function* () {
        event.preventDefault();
        const form = event.target;
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
            return;
        }
        const formData = new FormData(form);
        try {
            const response = yield fetch(form.action, {
                method: 'POST',
                body: formData
            });
            if (response.ok) {
                const data = yield response.json();
                if (data.redirectUrl) {
                    window.location.href = data.redirectUrl;
                }
            }
            else {
                const errorData = yield response.json();
                if (errorData.error) {
                    alert(errorData.error);
                }
                if (errorData.errors && errorData.errors.length > 0) {
                    alert(errorData.errors.join('\n'));
                }
            }
        }
        catch (error) {
            console.error('Reservation submission error:', error);
            alert('Wystąpił błąd podczas rezerwacji. Spróbuj ponownie.');
        }
    });
}
//# sourceMappingURL=reservationForm.js.map"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
document.addEventListener('DOMContentLoaded', () => {
    wireReservationFormHandlers();
    initDynamicRoomLoading();
});
function wireReservationFormHandlers() {
    const singleForm = document.getElementById('singleReservationForm');
    const doubleForm = document.getElementById('doubleReservationForm');
    if (singleForm) {
        singleForm.addEventListener('submit', handleReservationSubmit);
    }
    if (doubleForm) {
        doubleForm.addEventListener('submit', handleReservationSubmit);
    }
}
function initDynamicRoomLoading() {
    ['single', 'double'].forEach(formType => {
        const form = document.getElementById(`${formType}ReservationForm`);
        if (!form)
            return;
        const arrivalInput = form.querySelector('[name="ArrivalDate"]');
        const departureInput = form.querySelector('[name="DepartureDate"]');
        const roomSelect = form.querySelector('[name="RoomId"]');
        const roomTypeInput = form.querySelector('[name="RoomType"]');
        const roomType = (roomTypeInput === null || roomTypeInput === void 0 ? void 0 : roomTypeInput.value) || (formType.charAt(0).toUpperCase() + formType.slice(1));
        const loadRooms = () => __awaiter(this, void 0, void 0, function* () {
            const arrival = arrivalInput === null || arrivalInput === void 0 ? void 0 : arrivalInput.value;
            const departure = departureInput === null || departureInput === void 0 ? void 0 : departureInput.value;
            try {
                const response = yield fetch(`/Reservation/GetAvailableRoomsByDate?roomType=${roomType}&arrivalDate=${arrival || ''}&departureDate=${departure || ''}`);
                const data = yield response.json();
                if (roomSelect) {
                    const currentValue = roomSelect.value;
                    roomSelect.innerHTML = '<option value="">Wybierz pokój...</option>';
                    data.rooms.forEach((room) => {
                        const option = document.createElement('option');
                        option.value = room.id.toString();
                        option.textContent = `${room.text} ${room.statusLabel || ''}`.trim();
                        option.dataset.price = room.pricePerNight.toString();
                        roomSelect.appendChild(option);
                    });
                    if (currentValue && data.rooms.some((r) => r.id == currentValue)) {
                        roomSelect.value = currentValue;
                    }
                    roomSelect.dispatchEvent(new Event('change'));
                }
            }
            catch (error) {
                console.error('Error loading rooms:', error);
            }
        });
        arrivalInput === null || arrivalInput === void 0 ? void 0 : arrivalInput.addEventListener('change', loadRooms);
        departureInput === null || departureInput === void 0 ? void 0 : departureInput.addEventListener('change', loadRooms);

        loadRooms();
    });
}
function handleReservationSubmit(event) {
    return __awaiter(this, void 0, void 0, function* () {
        event.preventDefault();
        const form = event.target;
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
            return;
        }
        const formData = new FormData(form);
        const formType = formData.get('FormType');
        try {
            const response = yield fetch(form.action, {
                method: 'POST',
                body: formData
            });
            if (response.ok) {
                const data = yield response.json();
                if (data.redirectUrl) {
                    window.location.href = data.redirectUrl;
                }
            }
            else {
                const errorData = yield response.json();
                if (errorData.error) {
                    alert(errorData.error);
                }
                if (errorData.errors && errorData.errors.length > 0) {
                    alert(errorData.errors.join('\n'));
                }
            }
        }
        catch (error) {
            console.error('Reservation submission error:', error);
            alert('Wystąpił błąd podczas rezerwacji. Spróbuj ponownie.');
        }
    });
}
//# sourceMappingURL=reservationForm.js.map