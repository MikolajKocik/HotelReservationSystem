"use strict";
(() => {
    'use strict';
    const rules = {
        GuestFirstName: /^[A-Za-z��ʣ�ӌ������󜿟' \-]+$/,
        GuestLastName: /^[A-Za-z��ʣ�ӌ������󜿟' \-]+$/,
        GuestEmail: /^[^@\s]+@[^@\s]+\.[^@\s]+$/,
        GuestPhoneNumber: /^\+?\d{9,}$/
    };
    const prices = {
        1: 550,
        2: 700
    };
    const discounts = {
        'SUMMER10': 0.9,
        'WINTER15': 0.85
    };
    function initTotalSum(form, formType) {
        const arrivalInput = form.querySelector('[name="ArrivalDate"]');
        const departureInput = form.querySelector('[name="DepartureDate"]');
        const roomSelect = form.querySelector('[name="RoomId"]');
        const discountInput = form.querySelector('[name="DiscountCode"]');
        const totalDisplay = document.getElementById(`${formType}-total-sum`);
        const totalHidden = document.getElementById(`${formType}-total-input`);
        function calculate() {
            var _a, _b;
            const arrival = new Date((arrivalInput === null || arrivalInput === void 0 ? void 0 : arrivalInput.value) || '');
            const departure = new Date((departureInput === null || departureInput === void 0 ? void 0 : departureInput.value) || '');
            const roomId = Number(roomSelect === null || roomSelect === void 0 ? void 0 : roomSelect.value);
            const code = (discountInput === null || discountInput === void 0 ? void 0 : discountInput.value.trim().toUpperCase()) || '';
            const arrivalTime = arrival.getTime();
            const departureTime = departure.getTime();
            const days = (!Number.isFinite(arrivalTime) || !Number.isFinite(departureTime))
                ? 0
                : Math.max(0, Math.ceil((departureTime - arrivalTime) / (1000 * 60 * 60 * 24)));
            const roomPrice = (_a = prices[roomId]) !== null && _a !== void 0 ? _a : 0;
            const multiplier = (_b = discounts[code]) !== null && _b !== void 0 ? _b : 1.0;
            const total = days * roomPrice * multiplier;
            const formatted = Number.isFinite(total) ? total.toFixed(2) : '0.00';
            if (totalDisplay)
                totalDisplay.textContent = `${formatted} zł`;
            if (totalHidden)
                totalHidden.value = formatted;
        }
        arrivalInput === null || arrivalInput === void 0 ? void 0 : arrivalInput.addEventListener('change', calculate);
        arrivalInput === null || arrivalInput === void 0 ? void 0 : arrivalInput.addEventListener('input', calculate);
        departureInput === null || departureInput === void 0 ? void 0 : departureInput.addEventListener('change', calculate);
        departureInput === null || departureInput === void 0 ? void 0 : departureInput.addEventListener('input', calculate);
        roomSelect === null || roomSelect === void 0 ? void 0 : roomSelect.addEventListener('change', calculate);
        discountInput === null || discountInput === void 0 ? void 0 : discountInput.addEventListener('input', calculate);
        calculate();
    }
    function validateInput(input) {
        const pattern = rules[input.name];
        if (!pattern)
            return true;
        const value = input.value.trim();
        const isValid = value.length > 0 && pattern.test(value);
        input.classList.toggle('is-valid', isValid);
        input.classList.toggle('is-invalid', !isValid);
        return isValid;
    }
    function initForm(form) {
        var _a;
        Object.keys(rules).forEach(name => {
            const input = form.querySelector(`[name="${name}"]`);
            if (!input)
                return;
            input.addEventListener('input', () => validateInput(input));
        });
        form.addEventListener('submit', (event) => {
            let formIsValid = form.checkValidity();
            Object.keys(rules).forEach(name => {
                const input = form.querySelector(`[name="${name}"]`);
                if (!input)
                    return;
                if (!validateInput(input))
                    formIsValid = false;
            });
            if (!formIsValid) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
        const formType = ((_a = form.querySelector('[name="FormType"]')) === null || _a === void 0 ? void 0 : _a.value) || 'single';
        initTotalSum(form, formType);
    }
    document.querySelectorAll('.needs-validation').forEach(initForm);
    document.addEventListener('shown.bs.modal', (event) => {
        const modal = event.target;
        modal.querySelectorAll('.needs-validation').forEach(form => {
            if (!form.dataset.validationInit) {
                initForm(form);
                form.dataset.validationInit = 'true';
            }
        });
    });
})();
//# sourceMappingURL=form-validation.js.map