"use strict";
(() => {
    'use strict';
    const form = document.getElementById('loginForm');
    form === null || form === void 0 ? void 0 : form.addEventListener('submit', function (event) {
        if (!form.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
        }
        form.classList.add('was-validated');
    }, false);
})();
//# sourceMappingURL=loginForm.js.map