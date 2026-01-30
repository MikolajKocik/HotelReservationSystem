"use strict";
(() => {
    const scrollBtn = document.getElementById('scroll-up-button');
    if (scrollBtn) {
        scrollBtn.addEventListener('click', function (ev) {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }
})();
//# sourceMappingURL=scrollBtn.js.map