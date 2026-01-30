"use strict";
(() => {
    const form = document.getElementById('singleReservationForm');
    form === null || form === void 0 ? void 0 : form.addEventListener('submit', function (e) {
        if (!form.checkValidity()) {
            e.preventDefault();
            e.stopPropagation();
        }
        form.classList.add('was-validated');
    });
    const arrival = document.getElementById('ArrivalDate');
    const departure = document.getElementById('DepartureDate');
    if (arrival && departure) {
        function validateDates() {
            if (!arrival || !departure)
                return;
            if (arrival.value && departure.value && new Date(departure.value) <= new Date(arrival.value)) {
                departure.setCustomValidity('Data wyjazdu musi być po dacie przyjazdu');
            }
            else {
                departure.setCustomValidity('');
            }
        }
        arrival.addEventListener('change', validateDates);
        departure.addEventListener('change', validateDates);
    }
})();
//# sourceMappingURL=calculateReservation.js.map