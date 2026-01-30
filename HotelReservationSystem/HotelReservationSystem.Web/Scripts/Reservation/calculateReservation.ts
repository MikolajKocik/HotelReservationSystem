 (() : void => {
      const form = document.getElementById('singleReservationForm') as HTMLFormElement | null;
      form?.addEventListener('submit', function (e : Event) {
        if (!form.checkValidity()) { e.preventDefault(); e.stopPropagation(); }
        form.classList.add('was-validated');
      });
      const arrival = document.getElementById('ArrivalDate') as HTMLInputElement | null;
      const departure = document.getElementById('DepartureDate') as HTMLInputElement | null;
      if(arrival && departure){
        function validateDates(){
          if(!arrival || !departure) return;
          if(arrival.value && departure.value && new Date(departure.value) <= new Date(arrival.value)){
            departure.setCustomValidity('Data wyjazdu musi być po dacie przyjazdu');
          } else {
            departure.setCustomValidity('');
          }
        }
        arrival.addEventListener('change', validateDates);
        departure.addEventListener('change', validateDates);
      }
    })();