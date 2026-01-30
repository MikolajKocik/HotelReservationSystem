(() : void => {
    const availBtn : HTMLElement | null = document.getElementById('availability-rooms');
    if (availBtn) {
        availBtn.addEventListener('click', function(this: HTMLElement, ev: MouseEvent) : void {
            window.location.href = '/Room/Index';
        });
    }
})();