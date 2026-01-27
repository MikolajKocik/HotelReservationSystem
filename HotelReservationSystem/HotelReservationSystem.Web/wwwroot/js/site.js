"use strict";
document.querySelectorAll(".date").forEach(input => {
    input.addEventListener('click', function () {
        if (typeof this.showPicker === 'function') {
            this.showPicker();
        }
        else {
            console.warn("This browser doesnt support showPicker().");
        }
    });
});
//# sourceMappingURL=site.js.map