(function () {
  const form = document.getElementById('doubleReservationForm') as HTMLFormElement | null;
  if (form) {
    form.addEventListener('submit', function (e: Event) {
      if (!form.checkValidity()) { e.preventDefault(); e.stopPropagation(); }
      form.classList.add('was-validated');
    });
  }

  const arrival = document.getElementById('ArrivalDate') as HTMLInputElement | null;
  const departure = document.getElementById('DepartureDate') as HTMLInputElement | null;

  if (arrival && departure) {
    function validateDates(): void {
      if (arrival.value && departure.value && new Date(departure.value) <= new Date(arrival.value)) {
        departure.setCustomValidity('Data wyjazdu musi byæ po dacie przyjazdu');
      } else {
        departure.setCustomValidity('');
      }
    }

    arrival.addEventListener('change', validateDates);
    departure.addEventListener('change', validateDates);
  }
})();