"use strict";
(() => {
    const availBtn = document.getElementById('availability-rooms');
    if (availBtn) {
        availBtn.addEventListener('click', function (ev) {
            window.location.href = '/Room/Index';
        });
    }
})();
//# sourceMappingURL=availRoomsBtn.js.map