(() : void => {
  'use strict'

  var forms : NodeListOf<HTMLFormElement> = document.querySelectorAll('.needs-validation')

  Array.prototype.slice.call(forms)
    .forEach(function (form : HTMLFormElement) {
      form.addEventListener('submit', function (event : Event) {
        if (!form.checkValidity()) {
          event.preventDefault()
          event.stopPropagation()
        }

        form.classList.add('was-validated')
      }, false)
    })
})()