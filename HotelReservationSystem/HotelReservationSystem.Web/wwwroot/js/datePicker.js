"use strict";
(() => {
    const inputs = document.querySelectorAll(".date");
    inputs.forEach((input) => {
        input.addEventListener('click', function () {
            if ('showPicker' in this && typeof this.showPicker === 'function') {
                this.showPicker();
            }
            else {
                console.warn("This browser doesnt support showPicker().");
            }
        });
    });
})();
//# sourceMappingURL=datePicker.js.map