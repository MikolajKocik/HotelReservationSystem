(() : void => {
    'use strict'
    const form = document.getElementById('loginForm') as HTMLFormElement | null;
    form?.addEventListener('submit', function (event : Event) {
        if (!form.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
        }
        form.classList.add('was-validated');
    }, false);
})();