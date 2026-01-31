(() => {
    'use strict'

    const rules: Record<string, RegExp> = {
        GuestFirstName: /^[A-Za-z•∆ £—”åØèπÊÍ≥ÒÛúøü' \-]+$/,
        GuestLastName: /^[A-Za-z•∆ £—”åØèπÊÍ≥ÒÛúøü' \-]+$/,
        GuestEmail: /^[^@\s]+@[^@\s]+\.[^@\s]+$/,
        GuestPhoneNumber: /^\+?\d{9,}$/
    }

    const prices: Record<number, number> = {
        1: 550,
        2: 700
    }

    const discounts: Record<string, number> = {
        'SUMMER10': 0.9,
        'WINTER15': 0.85
    }

    function initTotalSum(form: HTMLFormElement, formType: string): void {
        const arrivalInput = form.querySelector<HTMLInputElement>('[name="ArrivalDate"]')
        const departureInput = form.querySelector<HTMLInputElement>('[name="DepartureDate"]')
        const roomSelect = form.querySelector<HTMLSelectElement>('[name="RoomId"]')
        const discountInput = form.querySelector<HTMLInputElement>('[name="DiscountCode"]')
        const totalDisplay = document.getElementById(`${formType}-total-sum`)
        const totalHidden = document.getElementById(`${formType}-total-input`) as HTMLInputElement | null

        function calculate(): void {
            const arrival = new Date(arrivalInput?.value || '')
            const departure = new Date(departureInput?.value || '')
            const roomId = Number(roomSelect?.value)
            const code = discountInput?.value.trim().toUpperCase() || ''

            const days = Math.max(0, Math.round((departure.getTime() - arrival.getTime()) / (1000 * 60 * 60 * 24)))
            const roomPrice = prices[roomId] ?? 0
            const multiplier = discounts[code] ?? 1.0
            const total = days * roomPrice * multiplier

            if (totalDisplay) totalDisplay.textContent = total.toFixed(2) + ' z≥'
            if (totalHidden) totalHidden.value = total.toFixed(2)
        }

        arrivalInput?.addEventListener('change', calculate)
        departureInput?.addEventListener('change', calculate)
        roomSelect?.addEventListener('change', calculate)
        discountInput?.addEventListener('input', calculate)

        calculate()
    }
    function validateInput(input: HTMLInputElement): boolean {
        const pattern = rules[input.name]
        if (!pattern) return true

        const value = input.value.trim()
        const isValid = value.length > 0 && pattern.test(value)

        input.classList.toggle('is-valid', isValid)
        input.classList.toggle('is-invalid', !isValid)
        return isValid
    }

    function initForm(form: HTMLFormElement): void {
        Object.keys(rules).forEach(name => {
            const input = form.querySelector<HTMLInputElement>(`[name="${name}"]`)
            if (!input) return

            input.addEventListener('input', () => validateInput(input))
        })

        form.addEventListener('submit', (event: Event) => {
            let formIsValid = form.checkValidity() 

            Object.keys(rules).forEach(name => {
                const input = form.querySelector<HTMLInputElement>(`[name="${name}"]`)
                if (!input) return
                if (!validateInput(input)) formIsValid = false
            })

            if (!formIsValid) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)

        const formType = form.querySelector<HTMLInputElement>('[name="FormType"]')?.value || 'single'
        initTotalSum(form, formType)
    }

    document.querySelectorAll<HTMLFormElement>('.needs-validation').forEach(initForm)

    document.addEventListener('bs:modal:shown.bs.modal', (event: Event) => {
        const modal = (event.target as HTMLElement)
        modal.querySelectorAll<HTMLFormElement>('.needs-validation').forEach(form => {
            if (!form.dataset.validationInit) {
                initForm(form)
                form.dataset.validationInit = 'true'
            }
        })
    });
})()